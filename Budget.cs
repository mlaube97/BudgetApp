using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MitchBudget
{
    public class Budget
    {
        private string _name;
        private float _amount;
        private float _remaining;

        public enum Fields
        {
            Name,
            Amount,
            Remaining
        }

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

        public Budget()
        {

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

        public void Inherit(Budget budget)
        {
            _name = budget.Name;
            _amount = budget.Amount;
            _remaining = budget.Remaining;
        }

        public Budget Duplicate()
        {
            Budget budget = new Budget(_name,_amount,_remaining);
            return budget;
        }

        public static bool Equals(Budget budget1, Budget budget2)
        {
            bool name = budget1.Name == budget2.Name;
            bool remaining = budget1.Remaining == budget2.Remaining;
            bool amount = budget1.Amount == budget2.Amount;
            bool equals = name && remaining && amount;
            return equals;
        }

    }

}
