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

namespace MitchBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Input_Budget> budgets = new List<Input_Budget>();
        Dictionary<DataGridRow, Input_Budget> row_to_budgets = new Dictionary<DataGridRow, Input_Budget>();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";


        public MainWindow()
        {
            InitializeComponent();
            System.IO.FileStream file = System.IO.File.Create(path);
        }

        private void buttonCreateBudget_Click(object sender, RoutedEventArgs e)
        {
            bool filled = !String.IsNullOrEmpty(textboxAmount.Text) && !String.IsNullOrEmpty(textboxName.Text) && !String.IsNullOrEmpty(textboxRemaining.Text);
            
            if ( filled == true)
            {
                Input_Budget budget = new Input_Budget(textboxName.Text, (float)Convert.ToDouble(textboxAmount.Text), (float)Convert.ToDouble(textboxRemaining.Text));
                budgets.Add(budget);
                XmlSerializer x = new XmlSerializer(budget.GetType());
                x.Serialize(Console.Out, budget);
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
            Input_Budget budget = budgets[row.GetIndex()];
            labelAmount_Transaction_Value.Content = budget.Amount;
            labelName_Transaction_Value.Content = budget.Name;
            labelRemaining_Transaction_Value.Content = budget.Remaining;
        }
    }
}
