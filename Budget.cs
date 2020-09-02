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

        private List<Transaction> _transactions;

        public List<Transaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }
        public Budget()
        {

        }
        public Budget(string name = "N/A", float amount = 0, float remaining = 0)
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

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            Transactions.Remove(transaction);
        }

        public void Inherit(Budget budget)
        {
            _name = budget.Name;
            _amount = budget.Amount;
            _remaining = budget.Remaining;
            _transactions = budget.Transactions;
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
