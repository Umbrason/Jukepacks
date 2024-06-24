using System.Diagnostics;
using MinecraftJukeboxPackCreator.packcontents;

namespace MinecraftJukeboxPackCreator.util
{
    public static class ffmpegUtil
    {
        //tasks:
        /*
            read duration
            convert to mono ogg    
        */

        public static string executablePath
        {
            get => MinecraftJukeboxPackCreator.Settings.ffmpegPath;
            set => MinecraftJukeboxPackCreator.Settings.ffmpegPath = value;
        }

        public static Process? ReadDuration(string input)
        {            
            if (!File.Exists(executablePath)) throw new FileNotFoundException("ffmpeg could not be found");
            var readDurationProcess = Process.Start(startInfo: new() {
                FileName = executablePath,
                UseShellExecute = false,
                Arguments = @$"-i ""{input}""",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            });
            return readDurationProcess;
        }

        public static Process? ConvertToMono(string input, string output)
        {
            if (!File.Exists(executablePath)) throw new FileNotFoundException("ffmpeg could not be found");
            var convertProcess = Process.Start(startInfo: new() {
                FileName = executablePath,
                UseShellExecute = false,
                Arguments = @$"-i ""{input}"" -ac 1 ""{output}""",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            });
            return convertProcess;
        }
    }
}