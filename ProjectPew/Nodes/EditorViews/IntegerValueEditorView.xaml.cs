using System;
using ReactiveUI;
using System.Windows;
using ProjectHeracles.Nodes.EditorViewModels;

namespace ProjectHeracles.Nodes.EditorViews
{
    public partial class IntegerValueEditorView : IViewFor<IntegerValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IntegerValueEditorViewModel), typeof(IntegerValueEditorView), new PropertyMetadata(null));

        public IntegerValueEditorViewModel ViewModel
        {
            get => (IntegerValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IntegerValueEditorViewModel)value;
        }
        #endregion

        public IntegerValueEditorView()
        {
            InitializeComponent();

            this. WhenActivated(
                d => d
                (
                    this.Bind(ViewModel,
                    vm => vm.Value,
                    v => v.intField.Value,
                    v => ViewModelToViewConverterFunc(v),
                    vm => ViewToViewModelConverterFunc(vm)
                )
            ));
        }

        private double ViewModelToViewConverterFunc(int? value)
        {
            double val = 0;
            try
            {
                val = Convert.ToDouble(value);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                throw;
            }
            return val;
        }

        private int ViewToViewModelConverterFunc(double? value)
        {
            int val = 0;
            try
            {
                val = Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                throw;
            }
            return val;
        }
    }
}
