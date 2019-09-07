using System;
using System.IO;

namespace TDD_BudgetApp.Model
{
    public class Period
    {
        private int _daysInMonth;

        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int Days()
        {
            return (End-Start).Days+1;
        }

        public decimal OverlappingDays(Period another)
        {

            if (NoOverLappingDay(another) || InvalidDate())
            {
                return 0;
            }

            var effectiveStart = another.Start>Start
                ? another.Start
                :Start;

            var effectiveEnd = another.End <End
                ? another.End
                : End;

            return (decimal) ((effectiveEnd - effectiveStart).TotalDays + 1);

           
        }

        private bool InvalidDate()
        {
            return Start > End;
        }

        private bool NoOverLappingDay(Period another)
        {
            return End < another.Start || Start > another.End;
        }

       
    }
}