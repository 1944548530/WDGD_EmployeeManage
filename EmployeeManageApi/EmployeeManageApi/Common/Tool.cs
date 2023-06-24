using EmployeeManageApi.Enum;
using EmployeeManageApi.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace EmployeeManageApi.Common
{
    public class Tool
    {
        private readonly string secret = "0123456789ABCDEFGHIJKLMNOPRSTUVWXYZ";
        public User GetAuthUser(string authHeader) {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var json = decoder.Decode(authHeader, secret, verify: true);
            User user = JsonConvert.DeserializeObject<User>(json);
            return user;
        }
        /// <summary>
        /// 获取当前日期的周数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int getWeekIndex(DateTime dt)
        {
            //当前时间当年的第一天
            DateTime time = Convert.ToDateTime(dt.ToString("yyyy") + "-01-01");
            TimeSpan ts = dt - time;
            //当年第一天是星期几
            int firstDayOfWeek = (int)time.DayOfWeek;
            //获取当前时间已过的总天数（四舍五入）
            int day = int.Parse(ts.TotalDays.ToString("F0")) + 1;
            //今年第一天星期日
            if (firstDayOfWeek == 0)
            {
                //总天数减1
                day--;
            }
            else
            {
                //减去第一周的天数
                day = day - (7 - firstDayOfWeek + 1);
            }
            //当前日期的星期
            int thisDayOfWeek = (int)dt.DayOfWeek;
            //星期日直接减7天
            if (thisDayOfWeek == 0)
            {
                day = day - 7;
            }
            else
            {
                day = day - thisDayOfWeek;
            }
            //第一个星期完整的7天+ 当前时间这个星期的7天 除以7
            int week = (day + 7 + 7) / 7;
            return week;
        }

        public string getWeekid(string date)
        {
            int weekIndex = getWeekIndex(DateTime.Parse(date));
            int year = DateTime.Parse(date).Year;
            string weekId = "";
            if (weekIndex < 10)
            {
                weekId = "0" + weekIndex;
            }
            else
            {
                weekId = weekIndex.ToString();
            }
            string week = year + weekId;
            return week;
        }

        public string sqlAppend(string date1, string date2, string sqlItem)
        {
            StringBuilder sb = new StringBuilder();
            if (date1 == "" && date2 != "")
            {
                sb.Append("and " + sqlItem + " <= '" + date2 + "'");
            }
            else if (date1 != "" && date2 == "")
            {
                sb.Append("and " + sqlItem + " >= '" + date1 + "'");
            }
            else if (date1 != "" && date2 != "")
            {
                sb.Append(" and " + sqlItem + " between '" + date1 + "' and '" + date2 + "'");
            }
            else
            {
                sb.Append("");
            }
            return sb.ToString();
        }

        public string getSqlAppend(List<string> li)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < li.Count; i++)
            {
                if (i != (li.Count - 1))
                {
                    sb.Append("'" + li[i] + "'" + ",");
                }
                else
                {
                    sb.Append("'" + li[i] + "'");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public string sqlAppend2(string sqlItem, string value)
        {
            StringBuilder sb = new StringBuilder();
            if (value == "All" || string.IsNullOrWhiteSpace(value))
            {
                sb.Append("");
            }
            else
            {
                sb.Append(" and " + sqlItem + " = '" + value + @"'");
            }
            return sb.ToString();
        }

        /// <summary>
        /// sql语句字符串拼接
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public string getSqlAppendByDt(DataTable dt, string item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != (dt.Rows.Count - 1))
                {
                    sb.Append("'" + dt.Rows[i][item] + "'" + ",");
                }
                else
                {
                    sb.Append("'" + dt.Rows[i][item] + "'");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// 获取日期list
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayCounts"></param>
        /// <returns></returns>
        public List<string> getDayList(string date, int dayCounts)
        {
            List<string> datList = new List<string>();
            for (int i = 0; i < dayCounts; i++)
            {
                //当前日期
                string dateTime = DateTime.Parse(date).AddDays(i).ToString("yyyy-MM-dd");
                string monthId = DateTime.Parse(dateTime).Month.ToString();
                string dayId = DateTime.Parse(dateTime).Day.ToString();
                string dateFor = monthId + "." + dayId;
                datList.Add(dateFor);
            }
            return datList;
        }

        /// <summary>
        /// 获取日期list yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayCounts"></param>
        /// <returns></returns>
        public List<string> getDayForList(string date, int dayCounts)
        {
            List<string> datList = new List<string>();
            for (int i = 0; i < dayCounts; i++)
            {
                //当前日期
                string dateTime = DateTime.Parse(date).AddDays(i).ToString("yyyy-MM-dd");
                datList.Add(dateTime);
            }
            return datList;
        }

        //计算一年有几周
        public int GetYearWeekCount(int year)
        {

            var dateTime = DateTime.Parse(year + "-01-01");
            var firstDayOfWeek = Convert.ToInt32(dateTime.DayOfWeek);//得到该年的第一天是周几 [周日、周一、周二...周六]
            if (firstDayOfWeek == 1)
            {
                var countDay = dateTime.AddYears(1).AddDays(-1).DayOfYear;
                var countWeek = countDay / 7 + 1;
                return countWeek;
            }
            else
            {
                //转换周日为最后一天
                if (firstDayOfWeek == 0)
                {
                    firstDayOfWeek = 7;
                }

                var countDay = dateTime.AddYears(1).AddDays(-1).DayOfYear;
                countDay -= (8 - firstDayOfWeek);
                var countWeek = countDay / 7 + 2;
                return countWeek;
            }
        }

        /// <summary>
        /// 根据起始日期，终止日期获取weekList
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<List<string>> getWeekList(string dateBegin, string dateEnd)
        {
            List<List<string>> weekList = new List<List<string>>();
            GregorianCalendar gc = new GregorianCalendar();
            //起始日为一年的第几周
            int dateBeginWeek = gc.GetWeekOfYear(DateTime.Parse(dateBegin), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //终止日为一年的第几周
            int dateEndWeek = gc.GetWeekOfYear(DateTime.Parse(dateEnd), CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            List<string> weekLi = new List<string>();
            List<string> weekForLi = new List<string>();
            int yearBegin = DateTime.Parse(dateBegin).Year;
            if (dateEndWeek > dateBeginWeek)
            {
                for (int k = 0; k < 5; k++)
                {
                    int week = dateBeginWeek + k;
                    string weekId = "";
                    if (week < 10)
                    {
                        weekId = "0" + week;
                    }
                    else
                    {
                        weekId = week.ToString();
                    }
                    weekForLi.Add(yearBegin + "W" + weekId);
                    weekLi.Add(yearBegin + weekId);
                }
            }
            else
            {
                int yearWeekCount = GetYearWeekCount(yearBegin);
                int yearEnd = DateTime.Parse(dateEnd).Year;
                int weekBCount = yearWeekCount - dateBeginWeek + 1;
                int weekAfterYear = 0;
                for (int i = 0; i <= weekBCount; i++)
                {
                    int week = dateBeginWeek + i;
                    weekForLi.Add(yearBegin + "W" + week.ToString());
                    weekLi.Add(yearBegin + week.ToString());
                }
                for (int i = 0; i < dateEndWeek; i++)
                {
                    int week = weekAfterYear + i;
                    weekForLi.Add(yearEnd + "W0" + week.ToString());
                    weekLi.Add(yearEnd + "0" + week.ToString());
                }
            }
            weekList.Add(weekLi);
            weekList.Add(weekForLi);
            return weekList;
        }

        

        public List<string> getMonthLi(string dateBegin, string dateEnd)
        {
            int monthBegin = DateTime.Parse(dateBegin).Month;
            int yearBegin = DateTime.Parse(dateBegin).Year;
            int monthEnd = DateTime.Parse(dateEnd).Month;
            int yearEnd = DateTime.Parse(dateEnd).Year;
            List<string> monthLi = new List<string>();
            if (monthBegin < monthEnd)
            {
                for (int i = 0; i < 5; i++)
                {
                    int month = monthBegin + i;
                    string monthStr = "";
                    if (month < 10)
                    {
                        monthStr = "0" + month;
                    }
                    else
                    {
                        monthStr = month.ToString();
                    }
                    monthLi.Add(yearBegin + monthStr);
                }
            }
            else
            {
                int yearBCount = 12 - monthBegin + 1;
                int monthABegin = 1;
                for (int i = 0; i < yearBCount; i++)
                {
                    int month = monthBegin + i;
                    string monthStr = "";
                    if (month < 10)
                    {
                        monthStr = "0" + month;
                    }
                    else
                    {
                        monthStr = month.ToString();
                    }
                    monthLi.Add(yearBegin + monthStr);
                }
                int yearACount = 5 - yearBCount;
                for (int i = 0; i < yearACount; i++)
                {
                    int month = monthABegin + i;
                    string monthStr = "0" + month;
                    monthLi.Add(yearEnd + monthStr);
                }
            }
            return monthLi;
        }

        //两个整数相除，保留两位小数
        public float perCal(int a, int b)
        {
            float i;
            if (b == 0)
            {
                return 0;
            }
            else
            {
                decimal ftemp = Math.Round(((decimal)a / b), 4);
                i = (float)(ftemp * 100);
                return i;
            }
        }

        //两个小数相除，保留4位小数
        public float perCal4(float a, float b)
        {
            float i;
            if (b == 0)
            {
                return 0;
            }
            else
            {
                decimal ftemp = Math.Round(((decimal)a / (decimal)b), 4);
                i = (float)(ftemp);
                return i;
            }
        }

        //两个小数相除保留n位小数
        public float perCal3(float a, float b, int n)
        {
            if (b == 0)
            {
                return 0;
            }
            else
            {
                decimal ftemp = Math.Round(((decimal)a / (decimal)b), n);
                return (float)ftemp;
            }
        }

        //两个整数相除，保留整数
        public float perCal1(int a, int b)
        {
            float i;
            if (b == 0)
            {
                return 100;
            }
            else
            {
                decimal ftemp = Math.Round(((decimal)a / b), 2);
                i = (float)(ftemp * 100);
                return i;
            }
        }


        public string getMonthid(string date)
        {
            int year = DateTime.Parse(date).Year;
            int monthIndex = DateTime.Parse(date).Month;
            string monthId = "";
            if (monthIndex < 10)
            {
                monthId = "0" + monthIndex;
            }
            else
            {
                monthId = monthIndex.ToString();
            }
            string month = year + monthId;
            return month;
        }

        public JObject ToJObject(string json) {
            if (json != null) {
                return JObject.Parse(json.Replace("&nbsp;", ""));
            }
            return JObject.Parse("{}");
        }

    }
}