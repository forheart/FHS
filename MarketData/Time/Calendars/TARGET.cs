using System;
using System.Runtime.InteropServices;

namespace FHS.MarketData {
    
    public class TARGET : Calendar {
        public TARGET() : base(Impl.Singleton) {}

		
        class Impl : Calendar.WesternImpl {
	        internal static readonly Impl Singleton = new Impl();
            private Impl() {}

            public override string name() { return "TARGET calendar"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day, dd = date.DayOfYear;
                Month m = (Month)date.Month;
                int y = date.Year;
                int em = easterMonday(y);

                if (isWeekend(w)
                    // New Year's Day
                    || (d == 1 && m == Month.January)
                    // Good Friday
                    || (dd == em - 3 && y >= 2000)
                    // Easter Monday
                    || (dd == em && y >= 2000)
                    // Labour Day
                    || (d == 1 && m == Month.May && y >= 2000)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Day of Goodwill
                    || (d == 26 && m == Month.December && y >= 2000)
                    // December 31st, 1998, 1999, and 2001 only
                    || (d == 31 && m == Month.December && (y == 1998 || y == 1999 || y == 2001)))
                    return false;
                return true;
            }

        }
    }
}