using ProjectHeracles.Nodes.EditorViewModels;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace ProjectHeracles.Nodes.EditorViews
{
    //TODO: FINISH LATER
    public partial class ButtonValueEditorView : UserControl, IViewFor<ButtonValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ButtonValueEditorViewModel), typeof(ButtonValueEditorView), new PropertyMetadata(null));

        public ButtonValueEditorViewModel ViewModel
        {
            get => (ButtonValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ButtonValueEditorViewModel)value;
        }
        #endregion

        public ButtonValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(
                d => d
                (
                    this.Bind(ViewModel,
                    vm => vm.Value,
                    v => v.Name //TODO: Here it's need to get the event off button click
                )
            ));
        }
    }
}
