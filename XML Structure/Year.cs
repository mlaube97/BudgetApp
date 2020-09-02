using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitchBudget
{
    public class Year
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private List<Month> _months;

        public List<Month> Months
        {
            get { return _months; }
            set { _months = value; }
        }
        public Year()
        {

        }
        public Year(int name)
        {
            _value = name;
        }
    }
}
