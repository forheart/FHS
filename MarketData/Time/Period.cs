using System;
using System.Text;

namespace FHS.MarketData
{
    public class Period
    {
        private int length_;
        private TimeUnit unit_;

        // properties
        public int length() { return length_; }
        public TimeUnit units() { return unit_; }

        public Period() { length_ = 0; unit_ = TimeUnit.Days; }
        public Period(int n, TimeUnit u) { length_ = n; unit_ = u; }
        public Period(String p)
        {
            String P = p.Trim().ToUpper();
            String unit = P.Substring(P.Length - 1, 1);
            length_ = Convert.ToInt32(P.Substring(0, P.Length - 1));
            switch (unit)
            {
                case "D": unit_ = TimeUnit.Days; break;
                case "W": unit_ = TimeUnit.Weeks; break;
                case "M": unit_ = TimeUnit.Months; break;
                case "Y": unit_ = TimeUnit.Years; break;
                default: unit_ = TimeUnit.Days; break;
            }
        }

        public Period(int 기간단위수, String 기간단위구분코드)
        {
            String P = 기간단위구분코드.Trim().ToUpper();
            length_ = 기간단위수;
            switch (P)
            {
                case "1": unit_ = TimeUnit.Days; break;
                case "2": unit_ = TimeUnit.Weeks; break;
                case "3": unit_ = TimeUnit.Months; break;
                case "6": unit_ = TimeUnit.Years; break;
                case "D": unit_ = TimeUnit.Days; break;
                case "W": unit_ = TimeUnit.Weeks; break;
                case "M": unit_ = TimeUnit.Months; break;
                case "Y": unit_ = TimeUnit.Years; break;
                default: unit_ = TimeUnit.Days; break;
            }
        }

        public Period(Frequency f)
        {
            switch (f)
            {
                case Frequency.Once:
                case Frequency.NoFrequency:
                    unit_ = TimeUnit.Days;	// same as Period()
                    length_ = 0;
                    break;
                case Frequency.Annual:
                    unit_ = TimeUnit.Years;
                    length_ = 1;
                    break;
                case Frequency.Semiannual:
                case Frequency.EveryFourthMonth:
                case Frequency.Quarterly:
                case Frequency.Bimonthly:
                case Frequency.Monthly:
                    unit_ = TimeUnit.Months;
                    length_ = 12 / (int)f;
                    break;
                case Frequency.EveryFourthWeek:
                case Frequency.Biweekly:
                case Frequency.Weekly:
                    unit_ = TimeUnit.Weeks;
                    length_ = 52 / (int)f;
                    break;
                case Frequency.Daily:
                    unit_ = TimeUnit.Days;
                    length_ = 1;
                    break;
                case Frequency.OtherFrequency:
                    throw new Exception("Unknown frequency: " + f);
                default:
                    throw new Exception("Unknown frequency: " + f);
            }
        }

        public Frequency frequency()
        {
            int length = System.Math.Abs(length_);	// unsigned version

            if (length == 0) return Frequency.NoFrequency;
            switch (unit_)
            {
                case TimeUnit.Years:
                    return (length == 1) ? Frequency.Annual : Frequency.OtherFrequency;
                case TimeUnit.Months:
                    if (12 % length == 0 && length <= 12)
                        return (Frequency)(12 / length);
                    else
                        return Frequency.OtherFrequency;
                case TimeUnit.Weeks:
                    if (length == 1)
                        return Frequency.Weekly;
                    else if (length == 2)
                        return Frequency.Biweekly;
                    else if (length == 4)
                        return Frequency.EveryFourthWeek;
                    else
                        return Frequency.OtherFrequency;
                case TimeUnit.Days:
                    return (length == 1) ? Frequency.Daily : Frequency.OtherFrequency;
                default:
                    throw new ArgumentException("Unknown TimeUnit: " + unit_);
            }
        }

        public void normalize()
        {
            if (length_ != 0)
                switch (unit_)
                {
                    case TimeUnit.Days:
                        if ((length_ % 7) == 0)
                        {
                            length_ /= 7;
                            unit_ = TimeUnit.Weeks;
                        }
                        break;
                    case TimeUnit.Weeks:
                    case TimeUnit.Months:
                        break;
                    case TimeUnit.Years:
                        length_ *= 12;
                        unit_ = TimeUnit.Months;
                        break;
                    default:
                        throw new ArgumentException("Unknown TimeUnit: " + unit_);
                }
        }

        public int monthLength()
        {
            switch (unit_)
            {
                case TimeUnit.Days: return length_ / 30;
                case TimeUnit.Weeks: return length_ / 4;
                case TimeUnit.Months: return length_;
                case TimeUnit.Years: return length_ * 12;
                default:
                    throw new ArgumentException("Unknown TimeUnit: " + unit_);
            }
        }

        public static Period operator -(Period p) { return new Period(-p.length(), p.units()); }
        public static Period operator *(int n, Period p) { return new Period(n * p.length(), p.units()); }
        public static Period operator *(Period p, int n) { return new Period(n * p.length(), p.units()); }

        public static bool operator ==(Period p1, Period p2) { return !(p1 < p2 || p2 < p1); }
        public static bool operator !=(Period p1, Period p2) { return !(p1 == p2); }
        public static bool operator <=(Period p1, Period p2) { return !(p1 > p2); }
        public static bool operator >=(Period p1, Period p2) { return !(p1 < p2); }
        public static bool operator >(Period p1, Period p2) { return p2 < p1; }
        public static bool operator <(Period p1, Period p2)
        {
            // special cases
            if (p1.length() == 0) return (p2.length() > 0);
            if (p2.length() == 0) return (p1.length() < 0);

            // exact comparisons
            if (p1.units() == p2.units()) return p1.length() < p2.length();
            if (p1.units() == TimeUnit.Months && p2.units() == TimeUnit.Years) return p1.length() < 12 * p2.length();
            if (p1.units() == TimeUnit.Years && p2.units() == TimeUnit.Months) return 12 * p1.length() < p2.length();
            if (p1.units() == TimeUnit.Days && p2.units() == TimeUnit.Weeks) return p1.length() < 7 * p2.length();
            if (p1.units() == TimeUnit.Weeks && p2.units() == TimeUnit.Days) return 7 * p1.length() < p2.length();

            // inexact comparisons (handled by converting to days and using limits)
            pair p1lim = new pair(p1), p2lim = new pair(p2);
            if (p1lim.hi < p2lim.lo || p2lim.hi < p1lim.lo)
                return p1lim.hi < p2lim.lo;
            else
                throw new ArgumentException("Undecidable comparison between " + p1.ToString() + " and " + p2.ToString());
        }

        // required by operator <
        struct pair
        {
            public int lo, hi;
            public pair(Period p)
            {
                switch (p.units())
                {
                    case TimeUnit.Days:
                        lo = hi = p.length(); break;
                    case TimeUnit.Weeks:
                        lo = hi = 7 * p.length(); break;
                    case TimeUnit.Months:
                        lo = 28 * p.length(); hi = 31 * p.length(); break;
                    case TimeUnit.Years:
                        lo = 365 * p.length(); hi = 366 * p.length(); break;
                    default:
                        throw new ArgumentException("Unknown TimeUnit: " + p.units());
                }
            }
        }

        public override bool Equals(object o) { return this == (Period)o; }
        public override int GetHashCode() { return 0; }
        public override string ToString()
        {
            return "TimeUnit: " + unit_.ToString() + ", length: " + length_.ToString();
        }
        public string ToShortString()
        {
            string result = "";
            int n = length();
            int m = 0;
            switch (units())
            {
                case TimeUnit.Days:
                    if (n >= 7)
                    {
                        m = n / 7;
                        result += m + "W";
                        n = n % 7;
                    }
                    if (n != 0 || m == 0)
                        return result + n + "D";
                    else
                        return result;
                case TimeUnit.Weeks:
                    return result + n + "W";
                case TimeUnit.Months:
                    if (n >= 12)
                    {
                        m = n / 12;
                        result += n / 12 + "Y";
                        n = n % 12;
                    }
                    if (n != 0 || m == 0)
                        return result + n + "M";
                    else
                        return result;
                case TimeUnit.Years:
                    return result + n + "Y";
                default:
                    throw new ApplicationException("unknown time unit (" + units() + ")");
            }
        }

        #region 개인적인 필요에 의한 임시 추가 (2012/01/13 LSY)
        // 18M가 1Y6M으로 표현되는 것을 피하고자 하나 추가함 

        public string ToShortString_OneUnit()
        {
            string result = "";
            int n = length();
            switch (units())
            {
                case TimeUnit.Days:
                    return result + n + "D";
                case TimeUnit.Weeks:
                    return result + n + "W";
                case TimeUnit.Months:
                    return result + n + "M";
                case TimeUnit.Years:
                    return result + n + "Y";
                default:
                    throw new ApplicationException("unknown time unit (" + units() + ")");
            }
        }

        #endregion
    }
}
