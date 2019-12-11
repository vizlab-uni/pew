using ReactiveUI;
using System.Windows;
using ProjectHeracles.Nodes.EditorViewModels;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;
using System;
using System.Windows.Forms;

namespace ProjectHeracles.Nodes.EditorViews
{
    public partial class ImageSourceValueEditorView : IViewFor<ImageSourceValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ImageSourceValueEditorViewModel), typeof(ImageSourceValueEditorView), new PropertyMetadata(null));

        public ImageSourceValueEditorViewModel ViewModel
        {
            get => (ImageSourceValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImageSourceValueEditorViewModel)value;
        }
        #endregion

        public ImageSourceValueEditorView()
        {
            InitializeComponent();


            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Value, v => v.imgNodePreview.Source).DisposeWith(d);
            });

        }
    }
}
