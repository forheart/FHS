using System;
using System.Text;

namespace FHS.MarketData {
    
    public class Germany : Calendar {
        //! German calendars
		
        public enum Market {
            Settlement,             //!< generic settlement calendar
            FrankfurtStockExchange, //!< Frankfurt stock-exchange
            Xetra,                  //!< Xetra
            Eurex,                  //!< Eurex
            Euwax                   //!< Euwax
        };

        public Germany() : this(Market.FrankfurtStockExchange) { }
        public Germany(Market m)
            : base() {
            // all calendar instances on the same market share the same
            // implementation instance
            switch (m) {
                case Market.Settlement:
                    calendar_ = Settlement.Singleton;
                    break;
                case Market.FrankfurtStockExchange:
                    calendar_ = FrankfurtStockExchange.Singleton;
                    break;
                case Market.Xetra:
                    calendar_ = Xetra.Singleton;
                    break;
                case Market.Eurex:
                    calendar_ = Eurex.Singleton;
                    break;
                case Market.Euwax:
                    calendar_ = Euwax.Singleton;
                    break;
                default:
                    throw new ArgumentException("Unknown market: " + m); ;
            }
        }

		
        class Settlement : Calendar.WesternImpl {
            public static readonly Settlement Singleton = new Settlement();
            private Settlement() { }
          
            public override string name() { return "German settlement"; }
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
                    || (dd == em-3)
                    // Easter Monday
                    || (dd == em)
                    // Ascension Thursday
                    || (dd == em+38)
                    // Whit Monday
                    || (dd == em+49)
                    // Corpus Christi
                    || (dd == em+59)
                    // Labour Day
                    || (d == 1 && m == Month.May)
                    // National Day
                    || (d == 3 && m == Month.October)
                    // Christmas Eve
                    || (d == 24 && m == Month.December)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Boxing Day
                    || (d == 26 && m == Month.December)
                    // New Year's Eve
                    || (d == 31 && m == Month.December))
                    return false;
                return true;
            }
        }

		
        class FrankfurtStockExchange : Calendar.WesternImpl {
            public static readonly FrankfurtStockExchange Singleton = new FrankfurtStockExchange();
            private FrankfurtStockExchange() { }

            public override string name() { return "Frankfurt stock exchange"; }
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
                    || (dd == em-3)
                    // Easter Monday
                    || (dd == em)
                    // Labour Day
                    || (d == 1 && m == Month.May)
                    // Christmas' Eve
                    || (d == 24 && m == Month.December)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Christmas Day
                    || (d == 26 && m == Month.December)
                    // New Year's Eve
                    || (d == 31 && m == Month.December))
                    return false;
                return true;
            }
        }

		
        class Xetra : Calendar.WesternImpl {
            public static readonly Xetra Singleton = new Xetra();
            private Xetra() { }
          
            public override string name() { return "Xetra"; }
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
                    || (dd == em-3)
                    // Easter Monday
                    || (dd == em)
                    // Labour Day
                    || (d == 1 && m == Month.May)
                    // Christmas' Eve
                    || (d == 24 && m == Month.December)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Christmas Day
                    || (d == 26 && m == Month.December)
                    // New Year's Eve
                    || (d == 31 && m == Month.December))
                    return false;
                return true;
            }
        }

		
        class Eurex : Calendar.WesternImpl {
            public static readonly Eurex Singleton = new Eurex();
            private Eurex() { }
          
            public override string name() { return "Eurex"; }
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
                    || (dd == em-3)
                    // Easter Monday
                    || (dd == em)
                    // Labour Day
                    || (d == 1 && m == Month.May)
                    // Christmas' Eve
                    || (d == 24 && m == Month.December)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Christmas Day
                    || (d == 26 && m == Month.December)
                    // New Year's Eve
                    || (d == 31 && m == Month.December))
                    return false;
                return true;
            }
        };

		
        class Euwax : Calendar.WesternImpl {
            public static readonly Euwax Singleton = new Euwax();
            private Euwax() { }

            public override string name() { return "Euwax"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day, dd = date.DayOfYear;
                Month m = (Month)date.Month;
                int y = date.Year;
                int em = easterMonday(y);

                if ((w == DayOfWeek.Saturday || w == DayOfWeek.Sunday)
                    // New Year's Day
                    || (d == 1 && m == Month.January)
                    // Good Friday
                    || (dd == em - 3)
                    // Easter Monday
                    || (dd == em)
                    // Labour Day
                    || (d == 1 && m == Month.May)
                    // Whit Monday
                    || (dd == em + 49)
                    // Christmas' Eve
                    || (d == 24 && m == Month.December)
                    // Christmas
                    || (d == 25 && m == Month.December)
                    // Christmas Day
                    || (d == 26 && m == Month.December)
                    // New Year's Eve
                    || (d == 31 && m == Month.December))
                    return false;
                return true;
            }
        };
    }
}