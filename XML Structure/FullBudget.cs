using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitchBudget
{
    public class FullBudget
    {
        private List<Year> _years;

        public List<Year> Years
        {
            get { return _years; }
            set { _years = value; }
        }
        public FullBudget()
        {

        }

        public void Remove(Budget budget)
        {
            foreach (Year y in Years)
            {
                foreach (Month m in y.Months)
                {
                    foreach (Budget b in m.Budgets)
                    {
                        if (Budget.isEqual(budget, b))
                        {
                            m.Budgets.Remove(b);
                            break;
                        }
                    }
                }
            }
        }
    }
}
