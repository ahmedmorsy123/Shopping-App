using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB.Enums
{
    public class Enums
    {
        public enum TimeDuration
        {
            Last24Hours,
            Last7Days,
            ThisMonth,
            Last30Days,
            Last90Days,
            ThisYear,
            ThisDay,
            AllTime
        }

        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled,
            All
        }

        public static DateTime CalculateStartDate(TimeDuration duration)
        {
            switch (duration)
            {
                case TimeDuration.Last24Hours:
                    return DateTime.Now.AddHours(-24);
                case TimeDuration.Last7Days:
                    return DateTime.Now.AddDays(-7);
                case TimeDuration.Last30Days:
                    return DateTime.Now.AddDays(-30);
                case TimeDuration.Last90Days:
                    return DateTime.Now.AddDays(-90);
                case TimeDuration.ThisMonth:
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                case TimeDuration.ThisYear:
                    return new DateTime(DateTime.Now.Year, 1, 1);
                case TimeDuration.ThisDay:
                    return DateTime.Today;
                case TimeDuration.AllTime:
                    return DateTime.MinValue; // No limit
                default:
                    return DateTime.MinValue;
            }
        }
    }


}
