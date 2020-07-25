using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MitchBudget
{
    public class Budget
    {
        private string _name;
        private float _amount;
        private float _remaining;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public float Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public float Remaining
        {
            get { return _remaining; }
            set { _remaining = value; }
        }
        public float Surplus
        {
            get { return _amount - _remaining; }
        }

        public Budget(string name, float amount, float remaining)
        {
            _name = name;
            _amount = amount;
            _remaining = remaining;
        }
        public void Spend(float value)
        {
            _remaining -= value;
        }

        public void Receive(float value)
        {
            _remaining += value;
        }

    }

}
