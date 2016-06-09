using System.Text.RegularExpressions;

namespace PlexBird.Util
{
    internal static class StringUtil
    {
        public static string RemoveNonASCII(string str)
        {
            return Regex.Replace(str, @"[^\u0000-\u007F]", string.Empty);
        }
    }
}
