using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MitchBudget
{
    public class Reader
    {
        public XmlNode All;

        public void ReadXML(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(path, settings);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode topNode = doc.LastChild;

            All = topNode;
        }
    }
}
