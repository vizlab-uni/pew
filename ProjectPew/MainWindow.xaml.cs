using ProjectHeracles.Windows;
using System.Windows;

namespace ProjectHeracles
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

        private void btnLucas_Click(object sender, RoutedEventArgs e)
        {
            LucasTest l_window = new LucasTest();
            l_window.Show();
        }

        private void btnPedro_Click(object sender, RoutedEventArgs e)
        {
            PedroTest p_window = new PedroTest();
            p_window.Show();
        }

        private void BtnTestPython_Click(object sender, RoutedEventArgs e)
        {
            RunPythonTest rptWindow = new RunPythonTest();
            rptWindow.Show();
        }

        private void BtnTestNodeNetwork_Click(object sender, RoutedEventArgs e)
        {
            NodeTest nt = new NodeTest();
            nt.Show();
        }
    }
}
