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
        List<Transaction> MonthTransactions = new List<Transaction>();
        

        public MainWindow()
        {
            InitializeComponent();
            MonthComboBox.SelectedItem = activeMonth;
            YearComboBox.SelectedItem = activeYear;
            budgets = GetFullBudget(path);
            SetBaseBudgetGrid();
            SetComboBoxes();
            InitMonthlyTransactions();
            labelBudgetValue.Content = path;

        }
        private FullBudget GetFullBudget(string path)
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
        private void SetMonthlyGrid()
        {
            DataGridMonthly.Items.Clear();
            foreach (Transaction t in MonthTransactions)
            {
                DataGridMonthly.Items.Add(t);
            }
        }
        private void InitMonthlyTransactions()
        {
            foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
            {
                foreach (Transaction transaction in budget.Transactions)
                {
                    MonthTransactions.Add(transaction);
                }
            }
            SetMonthlyGrid();
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
            budgets = GetFullBudget(dialog.FileName);
            SetBaseBudgetGrid();
            SetTransactionGrid();
            labelBudgetValue.Content = dialog.FileName;

        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, path);
        }
        private void buttonLoadDefault_Click(object sender, RoutedEventArgs e)
        {
            budgets = GetFullBudget(Global.defaultFile);
            SetBaseBudgetGrid();
            SetTransactionGrid();
        }
        private void buttonSaveDefault_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            reader.WriteXML(budgets, Global.defaultFile);
        }
        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Global.projectPath;
            dialog.Title = "Save Budget As";
            dialog.ShowDialog();
            string p = dialog.FileName;
            reader.WriteXML(budgets, p);
        }
        #endregion
        #region Button Clicks
        private void buttonCreateBudget_Click(object sender, RoutedEventArgs e)
        {
            bool filled = !String.IsNullOrEmpty(textboxAmount.Text) && !String.IsNullOrEmpty(textboxName.Text) && !String.IsNullOrEmpty(textboxRemaining.Text);

            if (filled == true)
            {
                try
                {
                    Budget budget = new Budget(textboxName.Text, (float)Convert.ToDouble(textboxAmount.Text), (float)Convert.ToDouble(textboxRemaining.Text));
                    Month month = new Month(activeMonth);
                    Year year = new Year(activeYear);
                    AddToFullBudget(activeYear, activeMonth, budget);
                    SetBaseBudgetGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please check all fields are filled correctly." + ex.Message, "Invalid Entry");
                }

            }
        }
        private void Button_Spend_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAddTransactionValidity())
            {
                Budget tempBudget = selectedBudget.Duplicate();
                float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                selectedBudget.Spend(value);
                foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
                {
                    if (Budget.isEqual(budget, tempBudget))
                    {
                        budget.Inherit(selectedBudget);
                        SetBaseBudgetGrid();
                        DateTime date = (DateTime)datePicker.SelectedDate;
                        string description = textboxDescription.Text;
                        string type = "Spend";
                        float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                        Transaction t = new Transaction(date, description, type, val);
                        budget.AddTransaction(t);
                        SetTransactionGrid();
                        MonthTransactions.Add(t);
                        SetMonthlyGrid();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Check the following:\n- You have selected a date\n- The price does not contain any numbers", "Invalid Entry");
            }
        }
        private void Button_Receive_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAddTransactionValidity())
            {
                Budget tempBudget = selectedBudget.Duplicate();
                float value = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                selectedBudget.Receive(value);
                foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
                {
                    if (Budget.isEqual(budget, tempBudget))
                    {
                        budget.Inherit(selectedBudget);
                        SetBaseBudgetGrid();
                        DateTime date = (DateTime)datePicker.SelectedDate;
                        string description = textboxDescription.Text;
                        string type = "Receive";
                        float val = (float)Convert.ToDouble(TextBox_TransactionAmount.Text);
                        Transaction t = new Transaction(date, description, type, val);
                        budget.AddTransaction(t);
                        SetTransactionGrid();
                        MonthTransactions.Add(t);
                        SetMonthlyGrid();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Check the following:\n- You have selected a date\n- The price does not contain any numbers", "Invalid Entry");
            }
        }
        private void buttonRemove_Click(object sender, RoutedEventArgs e) //budgetGrid remove button
        {
            if (buttonRemove.IsEnabled)
            {
                Budget budget = gridBudget.SelectedItem as Budget;
                List<Budget> b = GetBudgetsFromMonth(activeYear, activeMonth);
                b.Remove(budget);
                gridBudget.Items.Remove(gridBudget.SelectedItem);
                SetMonthlyGrid();
                buttonRemove.IsEnabled = false;
            }
        }
        private void TransactionGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!buttonTransactionRemove.IsEnabled)
                buttonTransactionRemove.IsEnabled = true;
        }
        private void buttonTransactionRemove_Click(object sender, RoutedEventArgs e)
        {
            if (buttonTransactionRemove.IsEnabled)
            {
                buttonTransactionRemove.IsEnabled = false;
                Transaction transaction = gridTransactions.SelectedItem as Transaction;
                Budget budgetGrid = new Budget();
                foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
                {
                    if (Budget.isEqual(selectedBudget, budget))
                    {
                        if (transaction.Type == "Spend")
                        {
                            budget.Receive(transaction.Amount);
                            selectedBudget = budget;
                        }
                        if (transaction.Type == "Receive")
                        {
                            budget.Spend(transaction.Amount);
                            selectedBudget = budget;
                        }
                    }
                }
                selectedBudget.RemoveTransaction(transaction);
                SetBaseBudgetGrid();
                SetTransactionGrid();
                MonthTransactions.Remove(transaction);
                SetMonthlyGrid();
            }
        }
        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            SetBaseBudgetGrid();
            gridTransactions.Items.Clear();
            MonthTransactions.Clear();
            foreach (Budget budget in GetBudgetsFromMonth(activeYear, activeMonth))
            {
                foreach (Transaction transaction in budget.Transactions)
                {
                    MonthTransactions.Add(transaction);
                }
            }
            SetMonthlyGrid();

        }
        #endregion
        private bool CheckAddTransactionValidity()
        {
            bool one = datePicker.SelectedDate.HasValue;
            int value;
            bool two = int.TryParse(TextBox_TransactionAmount.Text, out value);
            bool okay = (one == true) && (two == true);
            return okay;
            //TODO
        }
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
        private void AddToFullBudget(int year, string month, Budget budget)
        {
            //month + year already exists
            foreach(Year y in budgets.Years)
            {
                if (y.Value == year)
                {
                    foreach(Month m in y.Months)
                    {
                        if (m.Value == month)
                        {
                            m.Budgets.Add(budget);
                            return;
                        }
                    }
                }
            }
            //month + year doesn't exist
            foreach(Year y in budgets.Years)
            {
                if (y.Value == year)
                {
                    Month m = new Month(month, budget);
                    y.Months.Add(m);
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
            activeMonth = MonthComboBox.SelectedItem.ToString();
        }
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeYear = Convert.ToInt32(YearComboBox.SelectedItem);
        }

        private void gridBudget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IList<DataGridCellInfo> t = gridBudget.SelectedCells;
            //gridBudget.BeginEdit();
            
        }
        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            gridBudget.Items.Clear();
            gridTransactions.Items.Clear();
            DataGridMonthly.Items.Clear();
            budgets = new FullBudget();
        }
        private void textboxRemaining_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void textboxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void textboxName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void gridBudget_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        private void gridTransactions_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        private void DataGridMonthly_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
