using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOKOBAN_ASSESSMENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EZbtn_Click(object sender, RoutedEventArgs e)
        {
            EASYMODE eASYMODE = new EASYMODE("Easy Mode");
            eASYMODE.Show();
            this.Close();
        }

        private void MEDbtn_Click(object sender, RoutedEventArgs e)
        {
            MEDMODE mEDMODE = new MEDMODE("Medium Stage");
            mEDMODE.Show();
            this.Close();
        }

        private void HRDbtn_Click(object sender, RoutedEventArgs e)
        {
            HARDMODE hARDMODE = new HARDMODE("Hard Stage");
            hARDMODE.Show();
            this.Close();
        }

        private void QUITbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}