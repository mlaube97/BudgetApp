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
        public FullBudget ReadXML(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(FullBudget));
            FullBudget budgets = new FullBudget();
            using (var sr = new StreamReader(path))
            {
                budgets = (FullBudget)xs.Deserialize(sr);
            }
            return budgets;
        }

        public void WriteXML(FullBudget budgets, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(FullBudget));
            TextWriter tw = new StreamWriter(path);
            xs.Serialize(tw, budgets);
        }
    }
}
