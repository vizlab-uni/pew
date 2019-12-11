using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using ProjectHeracles.Nodes.ViewModels;
using ProjectHeracles.Nodes.ViewModels.Filters;
using ReactiveUI;

namespace ProjectHeracles.Nodes
{
    public class MainWindowViewModel : ReactiveObject
    {
        static MainWindowViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
        }

        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();
        

        public MainWindowViewModel()
        {
            ListViewModel.AddNodeType(() => new StartNodeModel());
            ListViewModel.AddNodeType(() => new BlurFilterNodeModel());
            ListViewModel.AddNodeType(() => new CannyFilterNodeModel());
            ListViewModel.AddNodeType(() => new ColorSpaceFilterNodeModel());
            ListViewModel.AddNodeType(() => new LaplacianFilterNodeModel());
            ListViewModel.AddNodeType(() => new SobelFilterNodeModel());
        }
    }
}

