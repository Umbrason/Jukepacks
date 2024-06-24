using MinecraftJukeboxPackCreator.util;

namespace MinecraftJukeboxPackCreator.packcontents
{
    public class JukeboxSong
    {
        public string SourceFile { get; set; }

        public JukeboxSong(string sourceFile)
        {
            this.SourceFile = sourceFile;
        }

        public float Duration { get; set; } = 0f;
        public string Name => Path.GetFileNameWithoutExtension(SourceFile);
        public string DiscItemJSON(string AlbumName) => $@"{{""id"": ""minecraft:music_disc_wait"", ""count"": 1, ""components"": {{""minecraft:jukebox_playable"": {{""song"": ""{AlbumName.SnakeCase()}:{Name.SnakeCase()}"", ""show_in_tooltip"": true}}}}}}";
        public string JSON => $@"
{{
    ""comparator_output"": 1,
    ""description"": {{
      ""translate"": ""{Name}""
    }},
    ""length_in_seconds"": {Duration:0.0},
    ""sound_event"": {{
        ""sound_id"": ""minecraft:music_disc.{Name.SnakeCase()}""
        }}
}}";
    }
}