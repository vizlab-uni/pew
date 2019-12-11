using DynamicData;
using Emgu.CV;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using ProjectHeracles.Nodes;
using ProjectHeracles.Nodes.ViewModels;
using ProjectHeracles.Nodes.ViewModels.Filters;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace ProjectHeracles.Windows
{
    public partial class NodeTest : Window, IViewFor<MainWindowViewModel>
    {
        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();
        
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(MainWindowViewModel), typeof(NodeTest), new PropertyMetadata(null));

        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainWindowViewModel)value;
        }
        #endregion

        public NodeTest()
        {
            InitializeComponent();
            CreateNodeNetwork();

            this.ViewModel = new MainWindowViewModel();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.ListViewModel, v => v.nodeList.ViewModel).DisposeWith(d);
            });
        }

        void CreateNodeNetwork()
        {
            //Create a new ViewModel for the NetworkView
            var network = new NetworkViewModel();
            
            var startNode = new StartNodeModel();           
            network.Nodes.Add(startNode);

            var filterNode = new ColorSpaceFilterNodeModel();
            var filterNode2 = new ColorSpaceFilterNodeModel();
            var blurFilter = new BlurFilterNodeModel();
            var cannyFilter = new CannyFilterNodeModel();


            //network.Nodes.Add(filterNode);
            //network.Nodes.Add(filterNode2);
            //network.Nodes.Add(blurFilter);
            //network.Nodes.Add(cannyFilter);


            var endNode = new NodeViewModel();
            endNode.Name = "End";
            
            //Assign the viewmodel to the view.
            networkView.ViewModel = network;

            //network.Connections.Add(network.ConnectionFactory(filterNode.Inp_Mat, startNode.Out_Mat));
        } 
    }
}
