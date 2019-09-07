using System;

namespace TDD_BudgetApp.Model
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime FirstDay => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);


        public DateTime LastDay => FirstDay.AddMonths(1).AddDays(-1);

        public int DaysInMonth => DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);

        public int DailyAmount => Amount / DaysInMonth;

        public Period CreatePeriod => new Period(FirstDay, LastDay);

        public decimal OverlappingAmount(Period period)
        {
            var dailyAmount = DailyAmount;

            return dailyAmount * period.OverlappingDays(CreatePeriod);
        }
    }
}