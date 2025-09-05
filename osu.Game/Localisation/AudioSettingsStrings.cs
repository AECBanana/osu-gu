// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Localisation
{
    public static class AudioSettingsStrings
    {
        private const string prefix = @"osu.Game.Resources.Localisation.AudioSettings";

        /// <summary>
        /// "Audio"
        /// </summary>
        public static LocalisableString AudioSectionHeader => new TranslatableString(getKey(@"audio_section_header"), @"Audio");

        /// <summary>
        /// "Devices"
        /// </summary>
        public static LocalisableString AudioDevicesHeader => new TranslatableString(getKey(@"audio_devices_header"), @"Devices");

        /// <summary>
        /// "Volume"
        /// </summary>
        public static LocalisableString VolumeHeader => new TranslatableString(getKey(@"volume_header"), @"Volume");

        /// <summary>
        /// "Output device"
        /// </summary>
        public static LocalisableString OutputDevice => new TranslatableString(getKey(@"output_device"), @"Output device");

        /// <summary>
        /// "Hitsound stereo separation"
        /// </summary>
        public static LocalisableString PositionalLevel => new TranslatableString(getKey(@"positional_hitsound_audio_level"), @"Hitsound stereo separation");

        /// <summary>
        /// "Master"
        /// </summary>
        public static LocalisableString MasterVolume => new TranslatableString(getKey(@"master_volume"), @"Master");

        /// <summary>
        /// "Master (window inactive)"
        /// </summary>
        public static LocalisableString MasterVolumeInactive => new TranslatableString(getKey(@"master_volume_inactive"), @"Master (window inactive)");

        /// <summary>
        /// "Effect"
        /// </summary>
        public static LocalisableString EffectVolume => new TranslatableString(getKey(@"effect_volume"), @"Effect");

        /// <summary>
        /// "Music"
        /// </summary>
        public static LocalisableString MusicVolume => new TranslatableString(getKey(@"music_volume"), @"Music");

        /// <summary>
        /// "Offset Adjustment"
        /// </summary>
        public static LocalisableString OffsetHeader => new TranslatableString(getKey(@"offset_header"), @"Offset Adjustment");

        /// <summary>
        /// "Audio offset"
        /// </summary>
        public static LocalisableString AudioOffset => new TranslatableString(getKey(@"audio_offset"), @"Audio offset");

        /// <summary>
        /// "Play a few beatmaps to receive a suggested offset!"
        /// </summary>
        public static LocalisableString SuggestedOffsetNote => new TranslatableString(getKey(@"suggested_offset_note"), @"Play a few beatmaps to receive a suggested offset!");

        /// <summary>
        /// "Based on the last {0} play(s), your offset is set correctly!"
        /// </summary>
        public static LocalisableString SuggestedOffsetCorrect(int plays) => new TranslatableString(getKey(@"suggested_offset_correct"), @"Based on the last {0} play(s), your offset is set correctly!", plays);

        /// <summary>
        /// "Based on the last {0} play(s), the suggested offset is {1} ms."
        /// </summary>
        public static LocalisableString SuggestedOffsetValueReceived(int plays, LocalisableString value) => new TranslatableString(getKey(@"suggested_offset_value_received"), @"Based on the last {0} play(s), the suggested offset is {1} ms.", plays, value);

        /// <summary>
        /// "Apply suggested offset"
        /// </summary>
        public static LocalisableString ApplySuggestedOffset => new TranslatableString(getKey(@"apply_suggested_offset"), @"Apply suggested offset");

        /// <summary>
        /// "Offset wizard"
        /// </summary>
        public static LocalisableString OffsetWizard => new TranslatableString(getKey(@"offset_wizard"), @"Offset wizard");

        /// <summary>
        /// "Adjust beatmap offset automatically"
        /// </summary>
        public static LocalisableString AdjustBeatmapOffsetAutomatically => new TranslatableString(getKey(@"adjust_beatmap_offset_automatically"), @"Adjust beatmap offset automatically");

        /// <summary>
        /// "If enabled, the offset suggested from last play on a beatmap is automatically applied."
        /// </summary>
        public static LocalisableString AdjustBeatmapOffsetAutomaticallyTooltip => new TranslatableString(getKey(@"adjust_beatmap_offset_automatically_tooltip"), @"If enabled, the offset suggested from last play on a beatmap is automatically applied.");

        /// <summary>
        /// "Audio device buffer length (ms)"
        /// </summary>
        public static LocalisableString AudioDeviceBufferLength => new TranslatableString(getKey(@"audio_device_buffer_length"), @"Audio device buffer length (ms)");

        /// <summary>
        /// "Adjust the audio device buffer length. Lower values reduce latency but may cause audio stuttering.On Windows, this config option only applies when WASAPI output is used. On Linux, the driver may choose to use a different Buffer Length if it decides that the specified Length is too short or long."
        /// </summary>
        public static LocalisableString AudioDeviceBufferLengthTooltip => new TranslatableString(getKey(@"audio_device_buffer_length_tooltip"), @"Adjust the audio device buffer length. Lower values reduce latency but may cause audio stuttering.On Windows, this config option only applies when WASAPI output is used. On Linux, the driver may choose to use a different Buffer Length if it decides that the specified Length is too short or long.");

        /// <summary>
        /// "Failed to change audio device. Please try selecting a different device."
        /// </summary>
        public static LocalisableString AudioDeviceError => new TranslatableString(getKey(@"audio_device_error"), @"Failed to change audio device. Please try selecting a different device.");

        /// <summary>
        /// "Prevent frame rate drop when window inactive (ASIO)"
        /// </summary>
        public static LocalisableString AsioPreventFrameRateDropOnInactive => new TranslatableString(getKey(@"asio_prevent_frame_rate_drop_on_inactive"), @"Prevent frame rate drop when window inactive (ASIO)");

        /// <summary>
        /// "When enabled, the game will maintain its normal frame rate even when running in the background. This prevents ASIO audio crackling caused by reduced update frequencies, but may use more system resources."
        /// </summary>
        public static LocalisableString AsioPreventFrameRateDropOnInactiveTooltip => new TranslatableString(getKey(@"asio_prevent_frame_rate_drop_on_inactive_tooltip"), @"When enabled, the game will maintain its normal frame rate even when running in the background. This prevents ASIO audio crackling caused by reduced update frequencies, but may use more system resources.");

        /// <summary>
        /// "Pause audio when window inactive (ASIO)"
        /// </summary>
        public static LocalisableString AsioPauseAudioOnInactive => new TranslatableString(getKey(@"asio_pause_audio_on_inactive"), @"Pause audio when window inactive (ASIO)");

        /// <summary>
        /// "Automatically pauses all audio playback when the game window becomes inactive. This completely prevents ASIO crackling and audio artifacts when the game is running in the background."
        /// </summary>
        public static LocalisableString AsioPauseAudioOnInactiveTooltip => new TranslatableString(getKey(@"asio_pause_audio_on_inactive_tooltip"), @"Automatically pauses all audio playback when the game window becomes inactive. This completely prevents ASIO crackling and audio artifacts when the game is running in the background.");

        /// <summary>
        /// "ASIO buffer size (samples)"
        /// </summary>
        public static LocalisableString AsioBufferSize => new TranslatableString(getKey(@"asio_buffer_size"), @"ASIO buffer size (samples)");

        /// <summary>
        /// "Adjust the ASIO buffer size in samples. Lower values reduce latency but may cause audio stuttering. Common values are 64, 128, 256, 512, 1024."
        /// </summary>
        public static LocalisableString AsioBufferSizeTooltip => new TranslatableString(getKey(@"asio_buffer_size_tooltip"), @"Adjust the ASIO buffer size in samples. Lower values reduce latency but may cause audio stuttering. Common values are 64, 128, 256, 512, 1024.");

        /// <summary>
        /// "ASIO buffer size when inactive (samples)"
        /// </summary>
        public static LocalisableString AsioInactiveBufferSize => new TranslatableString(getKey(@"asio_inactive_buffer_size"), @"ASIO buffer size when inactive (samples)");

        /// <summary>
        /// "Set a larger ASIO buffer size when the game is in the background to improve stability. This helps prevent audio crackling when the game loses focus."
        /// </summary>
        public static LocalisableString AsioInactiveBufferSizeTooltip => new TranslatableString(getKey(@"asio_inactive_buffer_size_tooltip"), @"Set a larger ASIO buffer size when the game is in the background to improve stability. This helps prevent audio crackling when the game loses focus.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
