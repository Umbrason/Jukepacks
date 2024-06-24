using System.Text.RegularExpressions;

namespace MinecraftJukeboxPackCreator.util
{
    public static class MinecraftStringUtil
    {
        public static string SnakeCase(this string s)
        {            
            s = Regex.Replace(s, "([A-Z]+)(\\W)", m => m.Groups[1].Value.ToLower() + m.Groups[2].Value);
            s = Regex.Replace(s, "([A-Z])([A-Z]*)([A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            s = Regex.Replace(s, "([A-Z])", " $1");
            s = Regex.Replace(s, @"\W", "_");
            s = Regex.Replace(s, @"[_]+", "_");
            s = s.Trim('_');
            return s.ToLower();
        }
    }
}