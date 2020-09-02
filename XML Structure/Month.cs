using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitchBudget
{
    public class Month
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private List<Budget> budgets;

        public List<Budget> Budgets
        {
            get { return budgets; }
            set { budgets = value; }
        }
        public Month()
        {

        }
        public Month(string name)
        {
            _value = name;
        }
    }
}
