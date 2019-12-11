using ProjectHeracles.Nodes.ViewModels;
using ReactiveUI;
using System.Windows;
using Microsoft.Win32;

namespace ProjectHeracles.Nodes.Views.NodeElements
{
    public partial class StartNodeLeadingView : IViewFor<StartNodeModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(StartNodeModel), typeof(StartNodeLeadingView), new PropertyMetadata(null));

        public StartNodeModel ViewModel
        {
            get => (StartNodeModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StartNodeModel)value;
        }
        #endregion
        
        public StartNodeLeadingView()
        {
            InitializeComponent();
        }

        private void BtnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Set viewModel string var with fullpath
                ViewModel.TxtFilePath = openFileDialog.FileName;
                //Set TextBox just with name of the file
                txtFilePath.Text = openFileDialog.SafeFileName;
            }
        }
    }
}
