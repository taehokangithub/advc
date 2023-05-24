using System;

namespace Advc.Utils
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