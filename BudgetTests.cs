﻿using System;
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


        //快捷鍵
        // .var tab tab
        //Ctrl U + R  
        //Alt Enter
        //Ctrl Shift +R  //右鍵找Refactor 找 Introduce Field
        //移類別到別的資料在 class名按右鍵…

        [Test]
        public void no_budgets()
        {
            GivenBudgets();
            TotalAmountShouldBe(0,new DateTime(2019, 9, 1), new DateTime(2019, 9, 1));
        }

        

        [Test]
        public void period_inside_budget_month()
        {
            //GivenBudgets(new Budget { YearMonth = "201004", Amount = 30 });

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

        //[Test]
        //public void period_inside_budget_month()
        //{
        //    GivenBudgets(new Budget { YearMonth = "201004", Amount = 30 });
        //    TotalAmountShouldBe(1, new DateTime(2010, 4, 1), new DateTime(2010, 4, 1));
        //}

        //[Test(Description = "輸入的日期在有budget之前回傳0")]
        //public void period_no_overlapping_before_budget_firstDay()
        //{
        //    GivenBudgets(new Budget { YearMonth = "201004", Amount = 30 });
        //    TotalAmountShouldBe(0, new DateTime(2010, 3, 31), new DateTime(2010, 3, 31));
        //}

        //[Test(Description = "輸入的日期在有budget之後回傳0")]
        //public void period_no_overlapping_after_budget_lastDay()
        //{
        //    GivenBudgets(new Budget { YearMonth = "201003", Amount = 31 });
        //    GivenBudgets(new Budget { YearMonth = "201004", Amount = 30 });
        //    TotalAmountShouldBe(0, new DateTime(2010, 5, 1), new DateTime(2010, 5, 1));
        //}

        //[Test]
        //public void Daily_Amount_is_10()
        //{
        //    GivenBudgets(new Budget { YearMonth = "201004", Amount = 300 });
        //    TotalAmountShouldBe(20, new DateTime(2010, 4, 1), new DateTime(2010, 4, 2));
        //}

    }
}