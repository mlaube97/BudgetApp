using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitchBudget
{
    public static class Global
    {
        public static string projectPath = @"C:\Users\mlaub\OneDrive\Documents\Visual Studio 2019\MitchBudget";
        public static string xmlFile = @"C:\Users\mlaub\OneDrive\Documents\Visual Studio 2019\MitchBudget\budgetlist.xml";
        public static string defaultFile = @"C:\Users\mlaub\OneDrive\Documents\Visual Studio 2019\MitchBudget\defaultbudgetlist.xml";
        public static List<string> Months = new List<string>() { "January", "February", "March", "April", "June", "July", "August", "September", "October", "November", "December" };
    }
}
