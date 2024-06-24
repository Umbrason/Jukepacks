
using System.Diagnostics;
using System.Text;
using MinecraftJukeboxPackCreator.util;

namespace MinecraftJukeboxPackCreator.packcontents
{
    public class JukeboxResourcepack
    {
        public JukeboxResourcepack(List<JukeboxSong> songs, string path, string name)
        {
            this.Songs = songs;
            this.Path = path;
            this.Name = name;
        }
        public List<JukeboxSong> Songs { get; }
        public string Path { get; }
        public string Name { get; }

        public void SaveToDisk()
        {
            List<Process> processes = new();
            var rootDir = new DirectoryInfo(Path);
            WriteMcMeta(rootDir);
            WritePackPNG(rootDir);
            var assets = rootDir.CreateSubdirectory("assets");
            var minecraft = assets.CreateSubdirectory("minecraft");
            WriteSoundsJSON(minecraft);
            var sounds = minecraft.CreateSubdirectory("sounds");
            var records = sounds.CreateSubdirectory("records");

            foreach (var song in Songs)
            {
                var destinationFile = $"{records.FullName}/{song.Name.SnakeCase()}.ogg";
                var convertProcess = ffmpegUtil.ConvertToMono(song.SourceFile, destinationFile);
                if (convertProcess == null) continue;
                processes.Add(convertProcess);
            }
            while (processes.Any(process => !(process?.HasExited ?? true)))
            {
                var processQueue = processes.ToList();
                foreach (var process in processQueue)
                {
                    process.StandardError.ReadToEnd();
                    process.StandardOutput.ReadToEnd();
                    if (process.WaitForExit(1000))
                    {
                        process.Close();
                        processes.Remove(process);
                    }
                }
            }
        }

        private void WritePackPNG(DirectoryInfo rootDir)
        {
            var img = Image.FromStream(typeof(MinecraftJukeboxPackCreator).Assembly.GetManifestResourceStream("MinecraftJukeboxPackCreator.PackIcon.png")!);
            img.Save(rootDir.FullName + "/pack.png", System.Drawing.Imaging.ImageFormat.Png);
        }


        private void WriteMcMeta(DirectoryInfo rootDir)
        {
            var mcmetaFile = new FileInfo(rootDir.FullName + "/pack.mcmeta");
            using var stream = mcmetaFile.CreateText();
            stream.Write($@"{{
    ""pack"": {{
        ""pack_format"": 34,
		""supported_formats"": [15, 34],
        ""description"": ""{Name} Music Discs""
    }}
}}");
            stream.Flush();
        }

        private void WriteSoundsJSON(DirectoryInfo dir)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            for (int i = 0; i < Songs.Count; i++)
            {
                JukeboxSong song = Songs[i];
                stringBuilder.AppendLine(@$"    ""music_disc.{song.Name.SnakeCase()}"": {{
        ""sounds"": [
            {{
                ""name"": ""records/{song.Name.SnakeCase()}"",
                ""stream"": true
            }}
        ]
    }}{(i < Songs.Count - 1 ? ',' : null)}");
            }
            stringBuilder.Append('}');
            var soundsFile = new FileInfo(dir.FullName + "/sounds.json");
            using var stream = soundsFile.CreateText();
            stream.Write(stringBuilder.ToString());
            stream.Flush();
        }
    }
}