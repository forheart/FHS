using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHS.MarketData
{
    //! Business/252 day count convention
	
    public class Business252 : DayCounter
    {
        private Calendar calendar_;

        // public Business252(Calendar c = Brazil())
        public Business252(Calendar c)
        {
            calendar_ = c;
            dayCounter_ = this;
        }

        public override string name() { return "Business/252(" + calendar_.name() + ")"; }
        public override int dayCount(Date d1, Date d2) { return calendar_.businessDaysBetween(d1, d2); }
        public override double yearFraction(Date d1, Date d2, Date d3, Date d4) { return dayCount(d1, d2) / 252.0; }
    }
}
