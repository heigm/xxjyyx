using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    public class Time
    {
        static int year = 1;
        static int month = 1;
        static int day = 1;
        static int totaldays = 0;
        /// <summary>
        /// 年
        /// </summary>
        public static int Year { get => year; set => year = value; }
        /// <summary>
        /// 月
        /// </summary>
        public static int Month { get => month; set => month = value; }
        /// <summary>
        /// 日
        /// </summary>
        public static int Day { get => day; set => day = value; }
        /// <summary>
        /// 总天数
        /// </summary>
        public static int Totaldays { get => totaldays; set => totaldays = value; }

        /// <summary>
        /// 天天增加
        /// </summary>
        public static void TimeSet()
        {
            Time.totaldays++;
            day++;
            if (day > 30)
            {
                month++;
                day = 1;
            }
            if (month > 12)
            {
                year++;
                month = 1;
            }
        }
        /// <summary>
        /// 得到当前时间，日，月，年
        /// </summary>
        /// <returns></returns>
        public static int[] GetTime()
        {
            return new int[] { day, month, year };
        }
    }
}
