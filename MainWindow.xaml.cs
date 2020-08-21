using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using MitchBudget.Properties;
using System.Runtime.Remoting.Channels;

namespace MitchBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Budget> budgets = new List<Budget>();
        Dictionary<DataGridRow, Budget> row_to_budgets = new Dictionary<DataGridRow, Budget>();
        string path = @"C:\Users\mlaub\OneDrive\Documents\Visual Studio 2019\MitchBudget\budgetlist.xml";

        XmlNode BudgetsNode;


        public MainWindow()
        {
            InitializeComponent();
            LoadBudgetXML(path);
            budgets = SetBudget(BudgetsNode);
            SetDataGrid();
        }

        private void LoadBudgetXML(string path)
        {
            Reader reader = new Reader();
            reader.ReadXML(path);
            BudgetsNode = reader.All;
        }

        private List<Budget> SetBudget(XmlNode budgets)
        {
            List<Budget> budget_list = new List<Budget>();
            foreach(XmlNode budget in budgets.ChildNodes)
            {
                XmlNodeList values = budget.ChildNodes;
                string name = values[0].InnerText;
                float amount = (float)Convert.ToDouble(values[1].InnerText);
                float remaining = (float)Convert.ToDouble(values[2].InnerText);
                budget_list.Add(new Budget(name, amount, remaining));
            }
            return budget_list;
        }

        private void SetDataGrid()
        {
            foreach(Budget budget in budgets)
            {
                gridBudget.Items.Add(budget);
            }
        }


        private void buttonCreateBudget_Click(object sender, RoutedEventArgs e)
        {
            bool filled = !String.IsNullOrEmpty(textboxAmount.Text) && !String.IsNullOrEmpty(textboxName.Text) && !String.IsNullOrEmpty(textboxRemaining.Text);
            
            if ( filled == true)
            {
                Budget budget = new Budget(textboxName.Text, (float)Convert.ToDouble(textboxAmount.Text), (float)Convert.ToDouble(textboxRemaining.Text));
                budgets.Add(budget);
                XmlSerializer x = new XmlSerializer(budget.GetType());
                gridBudget.Items.Add(budget);
            }
        }

        private void DataGridCell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Budget budget = budgets[row.GetIndex()];
            labelAmount_Transaction_Value.Content = budget.Amount;
            labelName_Transaction_Value.Content = budget.Name;
            labelRemaining_Transaction_Value.Content = budget.Remaining;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
