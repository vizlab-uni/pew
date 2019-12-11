using ProjectHeracles.Nodes.ViewModels;
using ReactiveUI;
using System.Windows;

namespace ProjectHeracles.Nodes.Views.NodeElements
{

    public partial class StartNodeTrailingView : IViewFor<StartNodeModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(StartNodeModel), typeof(StartNodeTrailingView), new PropertyMetadata(null));

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

        public StartNodeTrailingView()
        {
            InitializeComponent();
            
            this.Bind(ViewModel, vm => vm.PreviewSource, v => v.crdPreview.imgPreview.Source);
            this.Bind(ViewModel, vm => vm.PreviewSource, v => v.crdPreview.imgHighResolutionPreview.Source);
        }
    }
}