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

namespace MitchBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Budget> budgets = new List<Budget>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonCreateBudget_Click(object sender, RoutedEventArgs e)
        {
            bool filled = !String.IsNullOrEmpty(textboxAmount.Text) && !String.IsNullOrEmpty(textboxName.Text) && !String.IsNullOrEmpty(textboxRemaining.Text);
            
            if ( filled == true)
            {
                Budget budget = new Budget(textboxName.Text, (float)Convert.ToDouble(textboxAmount.Text), (float)Convert.ToDouble(textboxRemaining.Text));
                budgets.Add(budget);
                gridBudget.Items.Add(budget);
            }
        }
    }
}
