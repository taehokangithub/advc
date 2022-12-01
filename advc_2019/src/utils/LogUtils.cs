
namespace Advc.Utils.LogUtils
{
    class Loggable
    {
        public bool AllowLogDetail { get; set; } = false;

        public void LogDetail(string str)
        {
            if (AllowLogDetail)
            {
                Console.WriteLine(str);
            }
        }
    }
}