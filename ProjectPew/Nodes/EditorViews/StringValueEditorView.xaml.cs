﻿using ReactiveUI;
using System.Windows;
using ProjectHeracles.Nodes.EditorViewModels;
using System.Reactive.Disposables;

namespace ProjectHeracles.Nodes.EditorViews
{
    public partial class StringValueEditorView : IViewFor<StringValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(StringValueEditorViewModel), typeof(StringValueEditorView), new PropertyMetadata(null));

        public StringValueEditorViewModel ViewModel
        {
            get => (StringValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StringValueEditorViewModel)value;
        }
        #endregion

        public StringValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.Bind(ViewModel, vm => vm.Value, v => v.TextBox.Text).DisposeWith(d);
            });
        }
    }
}
