using System.Configuration;

namespace MinecraftJukeboxPackCreator
{
    public class MinecraftJukeboxPackCreatorSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ffmpegPath
        {
            get => (string)this[nameof(ffmpegPath)];
            set => this[nameof(ffmpegPath)] = value;
        }
    }
}