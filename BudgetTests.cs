using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TDD_BudgetApp.Model;
using TDD_BudgetApp.Repos;
using TDD_BudgetApp.Service;

namespace TDD_BudgetApp
{
    public class BudgetTests
    {
        private IRpos<Budget> _stubRpos;
        private Accounting _accounting;

        [SetUp]
        public void Setup()
        {
            _stubRpos = Substitute.For<IRpos<Budget>>();
            _accounting = new Accounting(_stubRpos);
        }

        [Test]
        public void no_budgets()
        {
            GivenBudgets();
            TotalAmountShouldBe(0,new DateTime(2019, 9, 1), new DateTime(2019, 9, 1));
        }

        

        [Test]
        public void period_inside_budget_month()
        {
            GivenBudgets(new Budget {YearMonth = "201909", Amount = 30});
            TotalAmountShouldBe(1, new DateTime(2019, 9, 1), new DateTime(2019, 9, 1));
        }

        [Test] 
        public void period_no_overlapping_before_budget_firstDay()
         { 
             GivenBudgets(new Budget { YearMonth = "201909", Amount = 30 }); 
             TotalAmountShouldBe(0, new DateTime(2019, 8, 30), new DateTime(2019, 8, 30)); 
         }

        [Test]
        public void period_no_overlapping_after_budget_lastDay()
        {
            GivenBudgets(new Budget { YearMonth = "201909", Amount = 30 });
            TotalAmountShouldBe(0, new DateTime(2019, 10, 1), new DateTime(2019, 10, 30));
        }

        [Test]
        public void period_overlapping_budget_firstDay()
        {
            GivenBudgets(new Budget { YearMonth = "201909", Amount = 30 });
            TotalAmountShouldBe(1, new DateTime(2019, 8, 31), new DateTime(2019, 9, 1));
        }

        [Test]
        public void period_overlapping_budget_lastDay()
        {
            GivenBudgets(new Budget { YearMonth = "201909", Amount = 30 });
            TotalAmountShouldBe(1, new DateTime(2019, 9, 30), new DateTime(2019, 10, 1));
        }

        [Test]
        public void invalid_period()
        {
            GivenBudgets(new Budget { YearMonth = "201909", Amount = 30 });
            TotalAmountShouldBe(0, new DateTime(2019, 9, 30), new DateTime(2019, 9, 1));
        }


        [Test]
        public void Daily_Amount_is_10()
        {
            GivenBudgets(new Budget { YearMonth = "201909", Amount = 300 });
            TotalAmountShouldBe(20, new DateTime(2019, 9, 1), new DateTime(2019, 9, 2));
        }

        [Test]
        public void multiple_budgets()
        {
            GivenBudgets(
                new Budget { YearMonth = "201909", Amount = 300 },
                new Budget { YearMonth = "201910", Amount = 31 }
                );
            TotalAmountShouldBe(11, new DateTime(2019, 9, 30), new DateTime(2019, 10, 1));
        }


        private void GivenBudgets(params Budget[] budgets)
        {
            _stubRpos.GetAll().Returns(budgets.ToList());
        }

        private void TotalAmountShouldBe(int expected, DateTime start, DateTime end)
        {
            var totalAmount = _accounting.TotalAmount(start, end);
            Assert.AreEqual(expected, totalAmount);
        }

        

    }
}