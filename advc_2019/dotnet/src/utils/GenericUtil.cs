namespace Advc.Utils
{
    class GenericUtil
    {
        public static char IntTochar(int val) => (char) (val + '0');
        public static int CharToInt(char c) => (int) (c - '0');
    }
}