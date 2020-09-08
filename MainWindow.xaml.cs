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
        FullBudget budgets = new FullBudget();
        string path = Global.xmlFile;
        Budget selectedBudget = new Budget();
        string activeMonth = "September";
        int activeYear = 2020;

        public MainWindow()
        {
            InitializeComponent();
            MonthComboBox.Text = activeMonth;
            YearComboBox.Text = activeYear.ToString();
            budgets = SetFullBudget(path);
            SetBaseBudgetGrid();
            SetComboBoxes();
        }
        private FullBudget SetFullBudget(string path)
        {
            Reader reader = new Reader();
            return reader.ReadXML(path);
        }
        private void SetBaseBudgetGrid()
        {
            gridBudget.Items.Clear();
            foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
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
        private void SetComboBoxes()
        {
            MonthComboBox.ItemsSource = Global.Months;
            foreach (Year year in budgets.Years)
            {
                YearComboBox.Items.Add(year.Value);
            }

        }
        #region Load and Save
        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Global.projectPath;
            dialog.Title = "Open Budget File";
            dialog.ShowDialog();
            budgets = SetFullBudget(dialog.FileName);
            SetBaseBudgetGrid();
            SetTransactionGrid();

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, path);
        }

        private void buttonLoadDefault_Click(object sender, RoutedEventArgs e)
        {
            budgets = SetFullBudget(Global.defaultFile);
            SetBaseBudgetGrid();
            SetTransactionGrid();
        }
        private void buttonSaveDefault_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, Global.defaultFile);
        }
        #endregion
        #region Button Clicks
        private void buttonCreateBudget_Click(object sender, RoutedEventArgs e)
        {
            bool filled = !String.IsNullOrEmpty(textboxAmount.Text) && !String.IsNullOrEmpty(textboxName.Text) && !String.IsNullOrEmpty(textboxRemaining.Text);

            if (filled == true)
            {
                Budget budget = new Budget(textboxName.Text, (float)Convert.ToDouble(textboxAmount.Text), (float)Convert.ToDouble(textboxRemaining.Text));
                Month month = new Month(activeMonth);
                Year year = new Year(activeYear);
                AddToFullBudget(activeYear, activeMonth, budget);

                SetBaseBudgetGrid();
            }
        }
        private void Button_Spend_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
            selectedBudget.Spend(value);
            foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
            {
                if (Budget.Equals(budget, tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetBaseBudgetGrid();
                    DateTime date = (DateTime)datePicker.SelectedDate;
                    string description = textboxDescription.Text;
                    int type = (int)Transaction.type.Spend;
                    float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                    budget.AddTransaction(new Transaction(date, description, type, val));
                    SetTransactionGrid();
                    break;
                }
            }
        }
        private void Button_Receive_Click(object sender, RoutedEventArgs e)
        {
            Budget tempBudget = selectedBudget.Duplicate();
            float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
            selectedBudget.Receive(value);
            foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
            {
                if (Budget.Equals(budget,tempBudget))
                {
                    budget.Inherit(selectedBudget);
                    SetBaseBudgetGrid();
                    DateTime date = (DateTime)datePicker.SelectedDate;
                    string description = textboxDescription.Text;
                    int type = (int)Transaction.type.Receive;
                    float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                    budget.AddTransaction(new Transaction(date, description, type, val));
                    SetTransactionGrid();
                    break;
                }
            }
        }
        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (buttonRemove.IsEnabled)
            {
                Budget budget = gridBudget.SelectedItem as Budget;
                List<Budget> b = GetBudgetsFromMonth(activeYear, activeMonth);
                b.Remove(budget);
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
                SetTransactionGrid();
            }
        }
        #endregion
        private void DataGridMonthly_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void gridBudget_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SetBaseBudgetGrid();
        }
        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            buttonRemove.IsEnabled = true;
            DataGridRow row = sender as DataGridRow;
            Budget budget = GetBudgetsFromMonth(activeYear, activeMonth)[row.GetIndex()];
            labelAmount_Transaction_Value.Content = budget.Amount;
            labelName_Transaction_Value.Content = budget.Name;
            labelRemaining_Transaction_Value.Content = budget.Remaining;
            selectedBudget.Inherit(budget);
            SetTransactionGrid();
        }
        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            activeMonth = MonthComboBox.Text;
            activeYear = Convert.ToInt32(YearComboBox.Text);
            SetBaseBudgetGrid();
            gridTransactions.Items.Clear();
        }
        private void AddToFullBudget(int year, string month, Budget budget)
        {
            foreach(Year y in budgets.Years)
            {
                if (y.Value == year)
                {
                    foreach(Month m in y.Months)
                    {
                        if (m.Value == month)
                        {
                            m.Budgets.Add(budget);
                        }
                    }
                }
            }
        }
        private List<Budget> GetBudgetsFromMonth(int year, string month)
        {
            List<Budget> b = new List<Budget>();
            foreach (Year y in budgets.Years)
            {
                if (y.Value == year)
                {
                    foreach (Month m in y.Months)
                    {
                        if (m.Value == month)
                        {
                            b = m.Budgets;
                            break;
                        }
                    }
                }
            }
            return b;
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeMonth = MonthComboBox.Text;
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeYear = Convert.ToInt32(YearComboBox.Text);
        }
    }
}
