using System.Collections.Generic;

namespace TDD_BudgetApp.Repos
{
    public  interface IRpos<T>
    {
        List<T> GetAll();
    }
}