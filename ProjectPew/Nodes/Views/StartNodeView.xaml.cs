using ProjectHeracles.Nodes.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectHeracles.Nodes.Views
{
    public partial class StartNodeView : IViewFor<StartNodeModel>
    {
        #region ViewModel

        StartNodeModel view;

        public StartNodeModel ViewModel
        {
            get => view; set => view = value;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel; set => ViewModel = (StartNodeModel)value;
        }

        #endregion
        
        public StartNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.StartNodeControl.ViewModel).DisposeWith(d);
            });
        }
    }
}