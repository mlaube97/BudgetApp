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
using Microsoft.Win32;

namespace MitchBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Budget> budgets = new List<Budget>();
        List<Transaction> transactions = new List<Transaction>();
        Dictionary<DataGridRow, Budget> row_to_budgets = new Dictionary<DataGridRow, Budget>();
        string path = Global.xmlFile;

        Budget selectedBudget = new Budget();


        public MainWindow()
        {
            InitializeComponent();
            budgets = SetBudget(path);
            SetBudgetGrid();
        }

        private List<Budget> SetBudget(string path)
        {
            Reader reader = new Reader();
            return reader.ReadXML(path);
        }

        private void SetBudgetGrid()
        {
            gridBudget.Items.Clear();
            foreach (Budget budget in budgets)
            {
                gridBudget.Items.Add(budget);
            }
        }
        private void SetTransactionGrid()
        {
            gridTransactions.Items.Clear();
            if (selectedBudget.Transactions != null)
            {
                foreach (Transaction transaction in selectedBudget.Transactions)
                {
                    gridTransactions.Items.Add(transaction);
                }
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
                SetBudgetGrid();
            }
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            buttonRemove.IsEnabled = true;
            DataGridRow row = sender as DataGridRow;
            Budget budget = budgets[row.GetIndex()];
            labelAmount_Transaction_Value.Content = budget.Amount;
            labelName_Transaction_Value.Content = budget.Name;
            labelRemaining_Transaction_Value.Content = budget.Remaining;
            selectedBudget.Inherit(budget);
            SetTransactionGrid();
        }
        #region Load and Save
        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Global.projectPath;
            dialog.Title = "Open Budget File";
            dialog.ShowDialog();
            budgets = SetBudget(dialog.FileName);
            SetBudgetGrid();
            SetTransactionGrid();

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, path);
        }

        private void buttonLoadDefault_Click(object sender, RoutedEventArgs e)
        {
            budgets = SetBudget(Global.defaultFile);
            SetBudgetGrid();
            SetTransactionGrid();
        }
        private void buttonSaveDefault_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, Global.defaultFile);
        }
        #endregion
        #region Button Clicks
        private void Button_Spend_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
            selectedBudget.Spend(value);
            foreach (Budget budget in budgets)
            {
                if (Budget.Equals(budget,tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetBudgetGrid();
                    string date = "N/A";
                    string description = textboxDescription.Text;
                    int type = (int)Transaction.type.Spend;
                    float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                    budget.AddTransaction(new Transaction(date, description, type, val));
                    SetTransactionGrid();
                }
            }
        }

        private void Button_Receive_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
            selectedBudget.Receive(value);
            foreach (Budget budget in budgets)
            {
                if (Budget.Equals(budget,tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetBudgetGrid();
                    string date = "N/A";
                    string description = textboxDescription.Text;
                    int type = (int)Transaction.type.Receive;
                    float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                    budget.AddTransaction(new Transaction(date, description, type, val));
                    SetTransactionGrid();
                }
            }
        }

        private void gridBudget_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SetBudgetGrid();
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (buttonRemove.IsEnabled)
            {
                Budget budget = gridBudget.SelectedItem as Budget;
                budgets.Remove(budget);
                gridBudget.Items.Remove(gridBudget.SelectedItem);
                buttonRemove.IsEnabled = false;
            }
        }

        private void TransactionGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!buttonTransactionRemove.IsEnabled)
                buttonTransactionRemove.IsEnabled = true;
            else
                buttonTransactionRemove.IsEnabled = false;
        }

        private void buttonTransactionRemove_Click(object sender, RoutedEventArgs e)
        {
            if (buttonTransactionRemove.IsEnabled)
            {
                buttonTransactionRemove.IsEnabled = false;
                Transaction transaction = gridTransactions.SelectedItem as Transaction;
                selectedBudget.RemoveTransaction(transaction);
                transactions.Remove(transaction);
                SetTransactionGrid();
            }
        }
        #endregion
    }
}
