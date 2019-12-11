using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Media;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    public class ImageSourceValueEditorViewModel : ValueEditorViewModel<ImageSource>
    {
        static ImageSourceValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ImageSourceValueEditorView(), typeof(IViewFor<ImageSourceValueEditorViewModel>));
        }

        private ImageSource _currentImageSource;
        public ImageSource CurrentImageSource
        {
            get => _currentImageSource;
            set => this.RaiseAndSetIfChanged(ref _currentImageSource, value);
        }

        public ImageSourceValueEditorViewModel()
        {
            //Value = HeraclesHelper.ToBitmapSource(Properties.Resources.logo_white);
            
            this.WhenAnyValue(vm => vm.CurrentImageSource)
                //.Select(v => v)
                .BindTo(this, vm => vm.Value);
            

        }

        public ImageSourceValueEditorViewModel(System.Windows.Media.Imaging.BitmapSource img)
        {
            Value = img;
        }
    }
}
