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
using System.Text.RegularExpressions;

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

        Budget selectedBudget = new Budget("N/A", 0, 0);


        public MainWindow()
        {
            InitializeComponent();
            budgets = SetBudget(path);
            SetDataGrid();
        }

        private List<Budget> SetBudget(string path)
        {
            Reader reader = new Reader();
            return reader.ReadXML(path);
        }

        private void SetDataGrid()
        {
            gridBudget.Items.Clear();
            foreach (Budget budget in budgets)
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
                gridBudget.Items.Add(budget);
                SetDataGrid();
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
            selectedBudget.Inherit(budget);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, path);
        }

        private void Button_Spend_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            selectedBudget.Spend((float)Convert.ToDouble(TextBox_TransactionAmount.Text));
            foreach (Budget budget in budgets)
            {
                if (Budget.Equals(budget,tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetDataGrid();
                }
            }
        }

        private void Button_Receive_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            selectedBudget.Receive((float)Convert.ToDouble(TextBox_TransactionAmount.Text));
            foreach (Budget budget in budgets)
            {
                if (Budget.Equals(budget,tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetDataGrid();
                }
            }
        }

        private void gridBudget_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SetDataGrid();
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            Budget budget = gridBudget.SelectedItem as Budget;
            budgets.Remove(budget);
            gridBudget.Items.Remove(gridBudget.SelectedItem);
        }
    }
}
