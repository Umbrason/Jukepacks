namespace MinecraftJukeboxPackCreator.util
{
    public static class IOUtil
    {
        public static bool IsDirectory(this string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}