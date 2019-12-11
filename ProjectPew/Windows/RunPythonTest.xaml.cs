using DynamicData;
using Microsoft.Win32;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjectHeracles.Windows
{
    /// <summary>
    /// Interaction logic for RunPythonTest.xaml
    /// </summary>
    public partial class RunPythonTest : Window
    {
        public RunPythonTest()
        {
            InitializeComponent();
        }

        private string scriptPath;

        private void BtnLoadScript_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                scriptPath = openFileDialog.FileName;

                btnRun.IsEnabled = !string.IsNullOrEmpty(scriptPath);

                txbCode.Text = string.Empty;

                string[] lines = File.ReadAllLines(scriptPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    txbCode.Text += lines[i] + "\r\n";
                }
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Discover why this script don't work properly
            HeraclesHelper.RunPromptCommand(Properties.Resources.pipreqs);

            pnlProcessing.Visibility = Visibility.Visible;

            //TODO: Made this paths be set by ui
            string args = "C:/Users/prossa/Documents/_Projects/ProjectHeracles/PYTHONSCRIPTS/NumberDetector/four.jpg"
                + " C:/Users/prossa/Documents/_Projects/ProjectHeracles/PYTHONSCRIPTS/NumberDetector/model.h5"
                + " C:/Users/prossa/Documents/_Projects/ProjectHeracles/PYTHONSCRIPTS/NumberDetector/weights.h5";

            txbCodeOutput.Text = string.Empty;
            HeraclesHelper.RunPythonScript(scriptPath, args, PythonScriptDataOut, PythonScriptError, ScriptEnd);
        }

        private void PythonScriptDataOut(string data)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() =>
                {
                    txbCodeOutput.Text += data + "\r\n";
                }));
        }

        private void PythonScriptError(string data)
        {
            Dispatcher.BeginInvoke(
               new ThreadStart(() =>
               {
                   txbCodeOutput.Text += "ERROR: " + data + "\r\n";
               }));
        }

        private void ScriptEnd()
        {
            Dispatcher.BeginInvoke(
                  new ThreadStart(() =>
                  {
                      pnlProcessing.Visibility = Visibility.Hidden;
                  }));
        }
    }
}
