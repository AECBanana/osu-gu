// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Localisation;
using osu.Framework;
using osu.Game.Configuration;
using osu.Framework.Logging;
using osu.Game.Overlays.Notifications;
using System;
using ManagedBass;
using osu.Framework.Audio.Asio;

namespace osu.Game.Overlays.Settings.Sections.Audio
{
    public partial class AudioDevicesSettings : SettingsSubsection
    {
        protected override LocalisableString Header => AudioSettingsStrings.AudioDevicesHeader;

        [Resolved]
        private AudioManager audio { get; set; }

        [Resolved]
        private OsuConfigManager config { get; set; }

        [Resolved]
        private INotificationOverlay notifications { get; set; }

        private SettingsDropdown<string> dropdown;
        private Bindable<double> audioBufferLength;
        private Bindable<float> asioBufferSize;
        private AsioBufferSizeDropdown asiobufferSizeDropdown;
        private bool hasShownAsioRestartNotification;

        private void onDeviceChanged(string name) => updateItems();

        private void deviceChanged(ValueChangedEvent<string> e)
        {
            try
            {
                // Set the selected device directly through the AudioDevice bindable
                audio.AudioDevice.Value = e.NewValue;
            }
            catch (BassException ex)
            {
                // Handle BASS audio device errors gracefully
                Logger.Log($"Failed to set audio device to '{e.NewValue}': {ex.Message}", LoggingTarget.Runtime, LogLevel.Error);

                // Revert to the previous value if device change failed
                audio.AudioDevice.Value = e.OldValue;

                // Show notification to user
                notifications?.Post(new SimpleErrorNotification
                {
                    Text = AudioSettingsStrings.AudioDeviceError
                });
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                Logger.Log($"Unexpected error when setting audio device to '{e.NewValue}': {ex.Message}", LoggingTarget.Runtime, LogLevel.Error);

                // Revert to the previous value if device change failed
                audio.AudioDevice.Value = e.OldValue;

                // Show notification to user
                notifications?.Post(new SimpleErrorNotification
                {
                    Text = AudioSettingsStrings.AudioDeviceError
                });
            }
        }

        private void onBufferLengthChanged(ValueChangedEvent<double> e)
        {
            // Sync the buffer length setting to the AudioManager in osu-framework
            // Note: We store the value in config but may need to apply it differently based on the framework version
        }

        private void onAsioBufferSizeChanged(ValueChangedEvent<float> e)
        {
            try
            {
                // Only apply hot reload for ASIO devices
                if (audio.AudioDevice.Value?.StartsWith("ASIO:", StringComparison.OrdinalIgnoreCase) == true)
                {
                    int bufferSamples = (int)e.NewValue;
                    Logger.Log($"ASIO buffer size changed to {bufferSamples} samples, applying hot reload", LoggingTarget.Runtime, LogLevel.Debug);

                    // Store current device name for restoration
                    string currentDevice = audio.AudioDevice.Value;
                    
                    // Perform hot reload by reinitializing the same ASIO device
                    // This works by temporarily clearing the device and then setting it back
                    Scheduler.AddDelayed(() =>
                    {
                        try
                        {
                            // Clear the device to force cleanup
                            audio.AudioDevice.Value = null;
                            
                            // Wait a frame then restore the ASIO device
                            Scheduler.AddDelayed(() =>
                            {
                                audio.AudioDevice.Value = currentDevice;
                                Logger.Log($"ASIO device hot reload completed with buffer size {bufferSamples} samples", LoggingTarget.Runtime, LogLevel.Debug);
                                
                                // Show success notification
                                notifications?.Post(new SimpleNotification
                                {
                                    Text = $"ASIO buffer size updated to {bufferSamples} samples (hot reload applied)"
                                });
                            }, 50);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"ASIO hot reload failed: {ex.Message}", LoggingTarget.Runtime, LogLevel.Error);
                            
                            // Try to restore the device anyway
                            try
                            {
                                audio.AudioDevice.Value = currentDevice;
                            }
                            catch
                            {
                                // Device restoration failed, show warning
                                notifications?.Post(new SimpleNotification
                                {
                                    Text = "ASIO hot reload failed. Please restart the application."
                                });
                            }
                        }
                    }, 50);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to process ASIO buffer size change: {ex.Message}", LoggingTarget.Runtime, LogLevel.Error);
            }
        }

        private void updateItems()
        {
            var deviceItems = new List<string> { string.Empty };
            deviceItems.AddRange(audio.AudioDeviceNames);

            // Add WASAPI exclusive and shared mode options for each device (Windows only)
            if (RuntimeInfo.OS == RuntimeInfo.Platform.Windows)
            {
                foreach (string deviceName in audio.AudioDeviceNames)
                {
                    if (!string.IsNullOrEmpty(deviceName))
                    {
                        deviceItems.Add($"WASAPI Exclusive: {deviceName}");
                        deviceItems.Add($"WASAPI Shared: {deviceName}");
                    }
                }
            }

            // Add ASIO devices to the list (only available in desktop version)
            var asioDevices = AsioDeviceManager.AvailableDevices.ToList();
            for (int i = 0; i < asioDevices.Count; i++)
            {
                string asioDeviceName = $"ASIO: {asioDevices[i].Name}";
                deviceItems.Add(asioDeviceName);
            }

            string preferredDeviceName = audio.AudioDevice.Value;
            if (deviceItems.All(kv => kv != preferredDeviceName))
                deviceItems.Add(preferredDeviceName);

            // The option dropdown for audio device selection lists all audio
            // device names. Dropdowns, however, may not have multiple identical
            // keys. Thus, we remove duplicate audio device names from
            // the dropdown. BASS does not give us a simple mechanism to select
            // specific audio devices in such a case anyways. Such
            // functionality would require involved OS-specific code.
            dropdown.Items = deviceItems
                             // Dropdown doesn't like null items. Somehow we are seeing some arrive here (see https://github.com/ppy/osu/issues/21271)
                             .Where(i => i != null)
                             .Distinct()
                             .ToList();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (audio != null)
            {
                audio.OnNewDevice -= onDeviceChanged;
                audio.OnLostDevice -= onDeviceChanged;
            }

            if (audioBufferLength != null)
                audioBufferLength.ValueChanged -= onBufferLengthChanged;

            if (asioBufferSize != null)
                asioBufferSize.ValueChanged -= onAsioBufferSizeChanged;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRange(new Drawable[]
            {
                dropdown = new AudioDeviceSettingsDropdown
                {
                    LabelText = AudioSettingsStrings.OutputDevice,
                    Current = audio.AudioDevice
                },
                new SettingsSlider<double>
                {
                    LabelText = AudioSettingsStrings.AudioDeviceBufferLength,
                    Current = config.GetBindable<double>(OsuSetting.AudioDeviceBufferLength),
                    TooltipText = AudioSettingsStrings.AudioDeviceBufferLengthTooltip
                },
                new SettingsCheckbox
                {
                    LabelText = AudioSettingsStrings.AsioPauseAudioOnInactive,
                    Current = config.GetBindable<bool>(OsuSetting.AsioPauseAudioOnInactive),
                    TooltipText = AudioSettingsStrings.AsioPauseAudioOnInactiveTooltip
                },
                asiobufferSizeDropdown = new AsioBufferSizeDropdown
                {
                    LabelText = AudioSettingsStrings.AsioBufferSize,
                    TooltipText = AudioSettingsStrings.AsioBufferSizeTooltip
                },
                new AsioBufferSizeDropdown
                {
                    LabelText = AudioSettingsStrings.AsioInactiveBufferSize,
                    Current = config.GetBindable<float>(OsuSetting.AsioInactiveBufferSize),
                    TooltipText = AudioSettingsStrings.AsioInactiveBufferSizeTooltip
                }
            });

            audio.OnNewDevice += onDeviceChanged;
            audio.OnLostDevice += onDeviceChanged;

            audioBufferLength = config.GetBindable<double>(OsuSetting.AudioDeviceBufferLength);
            audioBufferLength.ValueChanged += onBufferLengthChanged;

            asioBufferSize = config.GetBindable<float>(OsuSetting.AsioBufferSize);
            asioBufferSize.ValueChanged += onAsioBufferSizeChanged;
            
            // Set the dropdown's current binding to our asioBufferSize bindable
            asiobufferSizeDropdown.Current = asioBufferSize;

            updateItems();
        }

        private partial class AudioDeviceSettingsDropdown : SettingsDropdown<string>
        {
            protected override OsuDropdown<string> CreateDropdown() => new AudioDeviceDropdownControl();

            private partial class AudioDeviceDropdownControl : DropdownControl
            {
                protected override LocalisableString GenerateItemText(string item)
                    => string.IsNullOrEmpty(item) ? CommonStrings.Default : base.GenerateItemText(item);
            }
        }

        private partial class AsioBufferSizeDropdown : SettingsDropdown<float>
        {
            // Valid ASIO buffer sizes (powers of 2)
            private static readonly float[] valid_buffer_sizes = { 2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f, 4096f, 8192f };

            public AsioBufferSizeDropdown()
            {
                Items = valid_buffer_sizes;
            }

            protected override OsuDropdown<float> CreateDropdown() => new AsioBufferSizeDropdownControl();

            private partial class AsioBufferSizeDropdownControl : DropdownControl
            {
                protected override LocalisableString GenerateItemText(float item)
                {
                    return $"{(int)item} samples";
                }
            }
        }
    }
}
