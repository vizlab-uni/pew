using ProjectHeracles.Nodes.ViewModels.Filters;
using ReactiveUI;
using System.Windows;
using System.Windows.Media;

namespace ProjectHeracles.Nodes.Views.NodeElements
{
    public partial class FilterNodeTrailingView : IViewFor<FilterNodeModel>
    {

        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(FilterNodeModel), typeof(FilterNodeTrailingView), new PropertyMetadata(null));

        public FilterNodeModel ViewModel
        {
            get => (FilterNodeModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FilterNodeModel)value;
        }
        #endregion

        public FilterNodeTrailingView()
        {
            InitializeComponent();
            //Set preview Colors
            crdPreview.openPreviewButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x64, 0xc8, 0x64));
            crdPreview.previewCard.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x86, 0xef, 0x86));
            crdPreview.cardTitleBar.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xc4, 0xef, 0xc4));

            this.Bind(ViewModel, vm => vm.PreviewSource, v => v.crdPreview.imgPreview.Source);
            this.Bind(ViewModel, vm => vm.PreviewSource, v => v.crdPreview.imgHighResolutionPreview.Source);
        }
    }
}
