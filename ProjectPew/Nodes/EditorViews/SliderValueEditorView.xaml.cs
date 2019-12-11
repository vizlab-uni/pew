using ReactiveUI;
using System.Windows;
using ProjectHeracles.Nodes.EditorViewModels;
using System.Reactive.Disposables;
using System;

namespace ProjectHeracles.Nodes.EditorViews
{
    public partial class SliderValueEditorView : IViewFor<SliderValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(SliderValueEditorViewModel), typeof(SliderValueEditorView), new PropertyMetadata(null));

        public SliderValueEditorViewModel ViewModel
        {
            get => (SliderValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SliderValueEditorViewModel)value;
        }
        #endregion

        public SliderValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d => {

                this.Bind(
                    ViewModel, 
                    vm => vm.Value, v => v.txtCurrentValue.Text, 
                    ViewModelToViewConverterFunc, ViewToViewModelConverterFunc).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.MinValue, v => v.sliderElement.Minimum).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.MaxValue, v => v.sliderElement.Maximum).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Value, v => v.sliderElement.Value).DisposeWith(d);
            });
        }

        private string ViewModelToViewConverterFunc(double? value)
        {
            return value.ToString();
        }

        private double? ViewToViewModelConverterFunc(string value)
        {
            double val = 0;
            try
            {
                val = Convert.ToDouble(value);
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR: " + e.Message);
            }
            return val;
        }
    }
}
