using MitchBudget.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace MitchBudget
{
    public class Reader
    {
        public XmlNode All;

        public List<Budget> ReadXML(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Budget>));
            List<Budget> budgets = new List<Budget>();
            using (var sr = new StreamReader(path))
            {
                budgets = (List<Budget>)xs.Deserialize(sr);
            }
            return budgets;
        }

        public void WriteXML(List<Budget> budgets, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Budget>));
            TextWriter tw = new StreamWriter(path);
            xs.Serialize(tw, budgets);
        }
    }
}
