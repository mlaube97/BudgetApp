using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitchBudget
{
    public class Transaction
    {
        public enum type
        {
            Spend,
            Receive
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private float _amount;

        public float Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date.Date; }
            set { _date = value; }
        }

        public Transaction()
        {

        }
        public Transaction(DateTime date, string description, string type, float amount)
        {
            _date = date;
            _description = description;
            _type = type;
            _amount = amount;
        }
    }
}
