using System.Data;
using MinecraftJukeboxPackCreator.util;

namespace MinecraftJukeboxPackCreator.packcontents
{
    public class JukeboxPacks
    {
        public List<JukeboxSong> songs { get; }
        public void SaveToDisk(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            var isEmpty = !dirInfo.EnumerateDirectories().Any() && !dirInfo.EnumerateFiles().Any();
            if (!isEmpty) throw new DirectoryNotEmptyException($"Directory '{dirInfo.Name}' is not empty!");
            var datapackDir = dirInfo.CreateSubdirectory($"{dirInfo.Name}-dp");
            var resourcepackDir = dirInfo.CreateSubdirectory($"{dirInfo.Name}-rp");

            var datapack = new JukeboxDatapack(songs, datapackDir.FullName, dirInfo.Name);
            var resourcepack = new JukeboxResourcepack(songs, resourcepackDir.FullName, dirInfo.Name);

            HashSet<string> songTitles = new();
            foreach (var snakeCaseName in songs.Select(song => song.Name.SnakeCase()))
            {
                if (songTitles.Contains(snakeCaseName))
                    throw new DuplicateNameException($"{snakeCaseName} duplicate name");
                songTitles.Add(snakeCaseName);
            }

            datapack.SaveToDisk();
            resourcepack.SaveToDisk();
        }

        private static readonly string[] audioFileExtensions = { ".ogg", ".mp3", ".mp4", ".webm", ".wav" };
        private static EnumerationOptions enumOptions = new()
        {
            MaxRecursionDepth = 5,
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
            MatchType = MatchType.Simple,
            MatchCasing = MatchCasing.PlatformDefault
        };

        public JukeboxPacks(List<JukeboxSong> songs)
        {
            this.songs = songs;
        }

        public static JukeboxPacks FromPaths(string[] paths)
        {
            var dirs = paths.Where(path => path.IsDirectory());
            var files = paths.Where(path => !path.IsDirectory()).Where(file => audioFileExtensions.Any(file.ToLower().EndsWith)).ToList();
            foreach (var path in dirs)
                foreach (string file in Directory.EnumerateFiles(path, "*.*", enumOptions).Where(file => audioFileExtensions.Any(file.ToLower().EndsWith)))
                    files.Add(file);

            var jukeboxSongs = new List<JukeboxSong>();
            foreach (var audioFile in files)
            {
                var jukeboxSong = new JukeboxSong(audioFile);
                jukeboxSongs.Add(jukeboxSong);
            }
            return new(jukeboxSongs);
        }
    }
}