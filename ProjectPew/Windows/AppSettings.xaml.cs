using Microsoft.Win32;
using System.Windows;

namespace ProjectHeracles.Windows
{
    /// <summary>
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : Window
    {
        public AppSettings()
        {
            InitializeComponent();
        }

        private void BtnSetPythonPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Properties.Settings.Default.PythonPath = openFileDialog.FileName;
            }
        }

        private void BtnSetPythonScriptsFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Properties.Settings.Default.PythonScriptsFolder = openFileDialog.FileName;
            }
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnUpdatePip_Click(object sender, RoutedEventArgs e)
        {
            string s = Properties.Resources.pipUpdate;
            //TODO: Discover why this script don't work properly
            HeraclesHelper.RunPromptCommand(s);
        }
    }
}
