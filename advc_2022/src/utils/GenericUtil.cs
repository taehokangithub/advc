using System.Diagnostics;

namespace Advc.Utils
{
    class GenericUtil
    {
        public static char IntTochar(int val) => (char) (val + '0');
        public static int CharToInt(char c) => (int) (c - '0');

        public static string ExpectString(string str, string expected)
        {
            Debug.Assert(str.StartsWith(expected), $"{str} not starts with {expected}");
            return str.Substring(expected.Length);
        }
    }
}