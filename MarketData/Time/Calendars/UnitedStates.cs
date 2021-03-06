using System;
using System.Linq;
using System.Text;

namespace FHS.MarketData {
    
    public class UnitedStates : Calendar {
        //! US calendars
		
        public enum Market {
            Settlement,     //!< generic settlement calendar
            NYSE,           //!< New York stock exchange calendar
            GovernmentBond, //!< government-bond calendar
            NERC            //!< off-peak days for NERC
        };

        public UnitedStates() : this(Market.Settlement) { }
        public UnitedStates(Market m) : base() {
            switch (m) {
                case Market.Settlement:
                    calendar_ = Settlement.Singleton;
                    break;
                case Market.NYSE:
                    calendar_ = NYSE.Singleton;
                    break;
                case Market.GovernmentBond:
                    calendar_ = GovernmentBond.Singleton;
                    break;
                case Market.NERC:
                    calendar_ = NERC.Singleton;
                    break;
                default:
                    throw new ArgumentException("Unknown market: " + m); ;
            }
        }

		
        private class Settlement : Calendar.WesternImpl {
            public static readonly Settlement Singleton = new Settlement();
            private Settlement() { }

            public override string name() { return "US settlement"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day;
                Month m = (Month)date.Month;
                if (isWeekend(w)
                    // New Year's Day (possibly moved to Monday if on Sunday)
                    || ((d == 1 || (d == 2 && w == DayOfWeek.Monday)) && m == Month.January)
                    // (or to Friday if on Saturday)
                    || (d == 31 && w == DayOfWeek.Friday && m == Month.December)
                    // Martin Luther King's birthday (third Monday in January)
                    || ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.January)
                    // Washington's birthday (third Monday in February)
                    || ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.February)
                    // Memorial Day (last Monday in May)
                    || (d >= 25 && w == DayOfWeek.Monday && m == Month.May)
                    // Independence Day (Monday if Sunday or Friday if Saturday)
                    || ((d == 4 || (d == 5 && w == DayOfWeek.Monday) ||
                         (d == 3 && w == DayOfWeek.Friday)) && m == Month.July)
                    // Labor Day (first Monday in September)
                    || (d <= 7 && w == DayOfWeek.Monday && m == Month.September)
                    // Columbus Day (second Monday in October)
                    || ((d >= 8 && d <= 14) && w == DayOfWeek.Monday && m == Month.October)
                    // Veteran's Day (Monday if Sunday or Friday if Saturday)
                    || ((d == 11 || (d == 12 && w == DayOfWeek.Monday) ||
                         (d == 10 && w == DayOfWeek.Friday)) && m == Month.November)
                    // Thanksgiving Day (fourth Thursday in November)
                    || ((d >= 22 && d <= 28) && w == DayOfWeek.Thursday && m == Month.November)
                    // Christmas (Monday if Sunday or Friday if Saturday)
                    || ((d == 25 || (d == 26 && w == DayOfWeek.Monday) ||
                         (d == 24 && w == DayOfWeek.Friday)) && m == Month.December))
                    return false;
                return true;
            }
        }

		
        private class NYSE : Calendar.WesternImpl {
            public static readonly NYSE Singleton = new NYSE();
            private NYSE() { }
            
            public override string name() { return "New York stock exchange"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day, dd = date.DayOfYear;
                Month m = (Month)date.Month;
                int y = date.Year;
                int em = easterMonday(y);
                if (isWeekend(w)
                    // New Year's Day (possibly moved to Monday if on Sunday)
                    || ((d == 1 || (d == 2 && w == DayOfWeek.Monday)) && m == Month.January)
                    // Washington's birthday (third Monday in February)
                    || ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.February)
                    // Good Friday
                    || (dd == em - 3)
                    // Memorial Day (last Monday in May)
                    || (d >= 25 && w == DayOfWeek.Monday && m == Month.May)
                    // Independence Day (Monday if Sunday or Friday if Saturday)
                    || ((d == 4 || (d == 5 && w == DayOfWeek.Monday) ||
                         (d == 3 && w == DayOfWeek.Friday)) && m == Month.July)
                    // Labor Day (first Monday in September)
                    || (d <= 7 && w == DayOfWeek.Monday && m == Month.September)
                    // Thanksgiving Day (fourth Thursday in November)
                    || ((d >= 22 && d <= 28) && w == DayOfWeek.Thursday && m == Month.November)
                    // Christmas (Monday if Sunday or Friday if Saturday)
                    || ((d == 25 || (d == 26 && w == DayOfWeek.Monday) ||
                         (d == 24 && w == DayOfWeek.Friday)) && m == Month.December)
                    ) return false;

                if (y >= 1998) {
                    if (// Martin Luther King's birthday (third Monday in January)
                        ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.January)
                        // President Reagan's funeral
                        || (y == 2004 && m == Month.June && d == 11)
                        // September 11, 2001
                        || (y == 2001 && m == Month.September && (11 <= d && d <= 14))
                        // President Ford's funeral
                        || (y == 2007 && m == Month.January && d == 2)
                        ) return false;
                } else if (y <= 1980) {
                    if (// Presidential election days
                        ((y % 4 == 0) && m == Month.November && d <= 7 && w == DayOfWeek.Tuesday)
                        // 1977 Blackout
                        || (y == 1977 && m == Month.July && d == 14)
                        // Funeral of former President Lyndon B. Johnson.
                        || (y == 1973 && m == Month.January && d == 25)
                        // Funeral of former President Harry S. Truman
                        || (y == 1972 && m == Month.December && d == 28)
                        // National Day of Participation for the lunar exploration.
                        || (y == 1969 && m == Month.July && d == 21)
                        // Funeral of former President Eisenhower.
                        || (y == 1969 && m == Month.March && d == 31)
                        // Closed all day - heavy snow.
                        || (y == 1969 && m == Month.February && d == 10)
                        // Day after Independence Day.
                        || (y == 1968 && m == Month.July && d == 5)
                        // June 12-Dec. 31, 1968
                        // Four day week (closed on Wednesdays) - Paperwork Crisis
                        || (y == 1968 && dd >= 163 && w == DayOfWeek.Wednesday)
                        ) return false;
                } else {
                    if (// Nixon's funeral
                        (y == 1994 && m == Month.April && d == 27)
                        ) return false;
                }

                return true;
            }
        }

		
        private class GovernmentBond : Calendar.WesternImpl {
            public static readonly GovernmentBond Singleton = new GovernmentBond();
            private GovernmentBond() { }

            public override string name() { return "US government bond market"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day, dd = date.DayOfYear;
                Month m = (Month)date.Month;
                int y = date.Year;
                int em = easterMonday(y);
                if (isWeekend(w)
                    // New Year's Day (possibly moved to Monday if on Sunday)
                    || ((d == 1 || (d == 2 && w == DayOfWeek.Monday)) && m == Month.January)
                    // Martin Luther King's birthday (third Monday in January)
                    || ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.January)
                    // Washington's birthday (third Monday in February)
                    || ((d >= 15 && d <= 21) && w == DayOfWeek.Monday && m == Month.February)
                    // Good Friday
                    || (dd == em - 3)
                    // Memorial Day (last Monday in May)
                    || (d >= 25 && w == DayOfWeek.Monday && m == Month.May)
                    // Independence Day (Monday if Sunday or Friday if Saturday)
                    || ((d == 4 || (d == 5 && w == DayOfWeek.Monday) ||
                         (d == 3 && w == DayOfWeek.Friday)) && m == Month.July)
                    // Labor Day (first Monday in September)
                    || (d <= 7 && w == DayOfWeek.Monday && m == Month.September)
                    // Columbus Day (second Monday in October)
                    || ((d >= 8 && d <= 14) && w == DayOfWeek.Monday && m == Month.October)
                    // Veteran's Day (Monday if Sunday or Friday if Saturday)
                    || ((d == 11 || (d == 12 && w == DayOfWeek.Monday) ||
                         (d == 10 && w == DayOfWeek.Friday)) && m == Month.November)
                    // Thanksgiving Day (fourth Thursday in November)
                    || ((d >= 22 && d <= 28) && w == DayOfWeek.Thursday && m == Month.November)
                    // Christmas (Monday if Sunday or Friday if Saturday)
                    || ((d == 25 || (d == 26 && w == DayOfWeek.Monday) ||
                         (d == 24 && w == DayOfWeek.Friday)) && m == Month.December))
                    return false;
                return true;
            }
        }

		
        private class NERC : Calendar.WesternImpl {
            public static readonly NERC Singleton = new NERC();
            private NERC() { }

            public override string name() { return "North American Energy Reliability Council"; }
            public override bool isBusinessDay(Date date) {
                DayOfWeek w = date.DayOfWeek;
                int d = date.Day;
                Month m = (Month)date.Month;
                if (isWeekend(w)
                    // New Year's Day (possibly moved to Monday if on Sunday)
                    || ((d == 1 || (d == 2 && w == DayOfWeek.Monday)) && m == Month.January)
                    // Memorial Day (last Monday in May)
                    || (d >= 25 && w == DayOfWeek.Monday && m == Month.May)
                    // Independence Day (Monday if Sunday)
                    || ((d == 4 || (d == 5 && w == DayOfWeek.Monday)) && m == Month.July)
                    // Labor Day (first Monday in September)
                    || (d <= 7 && w == DayOfWeek.Monday && m == Month.September)
                    // Thanksgiving Day (fourth Thursday in November)
                    || ((d >= 22 && d <= 28) && w == DayOfWeek.Thursday && m == Month.November)
                    // Christmas (Monday if Sunday)
                    || ((d == 25 || (d == 26 && w == DayOfWeek.Monday)) && m == Month.December))
                    return false;
                return true;
            }
        }
    }
}
