using ProjectHeracles.Nodes.ViewModels;
using ProjectHeracles.Nodes.ViewModels.Filters;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ProjectHeracles.Nodes.Views
{
    public partial class FilterNodeView : IViewFor<FilterNodeModel>
    {
        #region ViewModel

        FilterNodeModel view;

        public FilterNodeModel ViewModel
        {
            get => view; set => view = value;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel; set => ViewModel = (FilterNodeModel)value;
        }

        #endregion

        public FilterNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            });
        }
    }
}
