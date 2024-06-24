using System.Diagnostics;
using System.Text.RegularExpressions;
using MinecraftJukeboxPackCreator.util;

namespace MinecraftJukeboxPackCreator.packcontents
{
    //TODO: Optain discs somehow
    // problem: additive loot_tables dont seem to be a thing
    // solution?: turn some existing loot item into a custom record disc voucher or smth
    //              -> add a /trigger command to turn it into an album
    public class JukeboxDatapack
    {
        public JukeboxDatapack(List<JukeboxSong> songs, string path, string name)
        {
            this.Songs = songs;
            this.Path = path;
            this.Name = name;
        }
        public List<JukeboxSong> Songs { get; }
        public string Path { get; }
        public string Name { get; }
        private class ReadDurationProcess
        {
            public bool done = false;
            public string output = "";
            public Process? process = null;
            public float duration = 0f;
        }
        public void SaveToDisk()
        {
            var datapackRootDir = new DirectoryInfo(Path);
            WriteMcMeta(datapackRootDir);
            var dataDir = datapackRootDir.CreateSubdirectory("data");
            HardcodedDatapackFiles hardCodedFiles = new(Name, Songs);
            foreach (var file in hardCodedFiles.files)
                WriteDPFile(dataDir, file.Key, file.Value);
            var namespaceDir = dataDir.CreateSubdirectory(Name.SnakeCase());
            var jukeboxSongDir = namespaceDir.CreateSubdirectory("jukebox_song");

            Dictionary<JukeboxSong, ReadDurationProcess> DurationProcesses = new();

            foreach (var song in Songs)
            {
                var process = ffmpegUtil.ReadDuration(song.SourceFile);
                if (process == null) continue;
                DurationProcesses[song] = new()
                {
                    process = process
                };
            }
            while (DurationProcesses.Values.Any(durationProcess => !durationProcess.done))
            {
                foreach (var durationProcess in DurationProcesses.Values)
                {
                    durationProcess.process.StandardOutput.ReadToEnd();
                    durationProcess.output += durationProcess.process.StandardError.ReadToEnd();

                    if (durationProcess.process.WaitForExit(1000))
                    {
                        durationProcess.process.Close();
                        durationProcess.done = true;
                    }
                }
            }

            foreach (var song in Songs)
            {
                var songFile = new FileInfo(jukeboxSongDir.FullName + $"/{song.Name.SnakeCase()}.json");
                var output = DurationProcesses[song].output;
                var match = Regex.Match(output, @"Duration: (\d\d:\d\d:\d\d.\d\d)");
                song.Duration = (float)TimeSpan.Parse(match.Groups[1].Value).TotalSeconds;
                using var stream = songFile.CreateText();
                stream.Write(song.JSON);
                stream.Flush();
                stream.Close();
            }
        }

        private void WriteDPFile(DirectoryInfo dataDir, string dataRelativePath, string content)
        {
            var splitPath = dataRelativePath.Split('/');
            var dir = dataDir;
            foreach (var folder in splitPath.SkipLast(1))
            {
                dir = dir.CreateSubdirectory(folder);
            }
            var file = new FileInfo($"{dir.FullName}/{splitPath.Last()}");
            using var stream = file.CreateText();
            stream.Write(content);
            stream.Flush();
        }

        private void WriteMcMeta(DirectoryInfo rootDir)
        {
            var mcmetaFile = new FileInfo(rootDir.FullName + "/pack.mcmeta");
            using var stream = mcmetaFile.CreateText();
            stream.Write($@"{{
    ""pack"": {{
        ""pack_format"": 48,
        ""description"": ""{Name} Music Discs"",
        ""filter"": {{
            ""block"": [
                {{
                    ""namespace"":""*"",
                    ""path"":""umbrason_custom_album_shared""
                }}
            ]
        }}
    }}
}}");
            stream.Flush();
        }
    }
}