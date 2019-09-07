using System;
using System.Linq;
using TDD_BudgetApp.Model;
using TDD_BudgetApp.Repos;

namespace TDD_BudgetApp.Service
{
    public class Accounting
    {
        private  IRpos<Budget> _budgetRpos;

        public Accounting(IRpos<Budget> budgetRpos)
        {
            _budgetRpos = budgetRpos;
            
        }

        public decimal TotalAmount(DateTime start, DateTime end)
        {
            var budgets = _budgetRpos.GetAll();
            Period period = new Period(start, end);

            decimal tottalAmount = 0;
            foreach (var budget in budgets)
            {
                
                tottalAmount+= budget.OverlappingAmount(period);

            }

            return tottalAmount;

        }
    }
}