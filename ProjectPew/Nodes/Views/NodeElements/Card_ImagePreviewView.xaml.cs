using System.Windows;
using System.Windows.Controls;

namespace ProjectHeracles.Nodes.Views.NodeElements
{
    public partial class Card_ImagePreviewView : UserControl
    {
        public Card_ImagePreviewView()
        {
            InitializeComponent();
        }

        private void BtnSavePreview_Click(object sender, RoutedEventArgs e)
        {
            HeraclesHelper.SaveUsingEncoder(imgHighResolutionPreview, "MyImage");
        }
    }
}
