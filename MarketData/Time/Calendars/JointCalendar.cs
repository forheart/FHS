using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHS.MarketData {
    
    public class JointCalendar : Calendar {
        //! rules for joining calendars
		
        public enum JointCalendarRule {
            JoinHolidays,    /*!< A date is a holiday for the joint calendar if it is a holiday
                                  for any of the given calendars */
            JoinBusinessDays /*!< A date is a business day for the joint calendar if it is a business day
                                  for any of the given calendars */
        };

		
        private class Impl : Calendar {
            private JointCalendarRule rule_;
            private List<Calendar> calendars_ = new List<Calendar>();

            public Impl(Calendar c1, Calendar c2, JointCalendarRule r) {
                rule_ = r;
                calendars_.Add(c1);
                calendars_.Add(c2);
            }
            public Impl(Calendar c1, Calendar c2, Calendar c3, JointCalendarRule r) {
                rule_ = r;
                calendars_.Add(c1);
                calendars_.Add(c2);
                calendars_.Add(c3);
            }
            public Impl(Calendar c1, Calendar c2, Calendar c3, Calendar c4, JointCalendarRule r) {
                rule_ = r;
                calendars_.Add(c1);
                calendars_.Add(c2);
                calendars_.Add(c3);
                calendars_.Add(c4);
            }

            public override string name() {
                string result = "";
                switch (rule_) {
                    case JointCalendarRule.JoinHolidays:
                        result += "JoinHolidays(";
                        break;
                    case JointCalendarRule.JoinBusinessDays:
                        result += "JoinBusinessDays(";
                        break;
                    default:
                        throw new ApplicationException("unknown joint calendar rule");
                }
                result += calendars_.Find(null).name();
                for(int i = 1; i < calendars_.Count; i++)
                    result += ", " + calendars_[i].name();
                result += ")";
                return result;
            }

            public override bool isWeekend(DayOfWeek w) {
                switch (rule_) {
                    case JointCalendarRule.JoinHolidays:
                        foreach(Calendar c in calendars_)
                            if (c.isWeekend(w)) return true;
                        return false;
                    case JointCalendarRule.JoinBusinessDays:
                        foreach(Calendar c in calendars_)
                            if (c.isWeekend(w)) return false;
                        return true;
                    default:
                        throw new ApplicationException("unknown joint calendar rule");
                }
            }

            public override bool isBusinessDay(Date date) {
                switch (rule_) {
                    case JointCalendarRule.JoinHolidays:
                        foreach (Calendar c in calendars_)
                            if (c.isHoliday(date))
                                return false;
                        return true;
                    case JointCalendarRule.JoinBusinessDays:
                        foreach (Calendar c in calendars_)
                            if (c.isBusinessDay(date))
                                return true;
                        return false;
                    default:
                        throw new ApplicationException("unknown joint calendar rule");
                }
            }
        }

        public JointCalendar(Calendar c1, Calendar c2)
            : this(c1, c2, JointCalendarRule.JoinHolidays) { }
        public JointCalendar(Calendar c1, Calendar c2, JointCalendarRule r)
            : base(new Impl(c1,c2,r)) { }

        public JointCalendar(Calendar c1, Calendar c2, Calendar c3)
            : this(c1, c2, c3, JointCalendarRule.JoinHolidays) { }
        public JointCalendar(Calendar c1, Calendar c2, Calendar c3, JointCalendarRule r)
            : base(new Impl(c1,c2,c3,r)) { }

        public JointCalendar(Calendar c1, Calendar c2, Calendar c3, Calendar c4)
            : this(c1, c2, c3, c4, JointCalendarRule.JoinHolidays) { }
        public JointCalendar(Calendar c1, Calendar c2, Calendar c3, Calendar c4, JointCalendarRule r) 
            : base(new Impl(c1,c2,c3,c4,r)) { }
    }
}
