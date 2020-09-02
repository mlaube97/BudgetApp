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
    }
}
