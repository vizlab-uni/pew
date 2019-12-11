using System;
using System.Reactive.Linq;
using DynamicData;
using Emgu.CV;
using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViewModels;
using ProjectHeracles.Nodes.Views;
using ReactiveUI;


namespace ProjectHeracles.Nodes.ViewModels.Filters
{
    class CannyFilterNodeModel : FilterNodeModel
    {
        public ValueNodeInputViewModel<double?> Inp_Threshold_1 { get; }
        public ValueNodeInputViewModel<double?> Inp_Threshold_2 { get; }

        public CannyFilterNodeModel()
        {
            //Node Name
            this.Name = "Canny Filter";

            //------------------FIRST THRESHOLD------------------
            Inp_Threshold_1 = new ValueNodeInputViewModel<double?>()
            {
                Name = "Threshold"
            };
            Inp_Threshold_1.Editor = new SliderValueEditorViewModel(1, 0, 10);
            Inp_Threshold_1.Port.IsVisible = false;
            this.Inputs.Add(Inp_Threshold_1);
            //------------------------------------------------------


            //------------------SECOND THRESHOLD------------------
            Inp_Threshold_2 = new ValueNodeInputViewModel<double?>()
            {
                Name = "Second Threshold"
            };
            Inp_Threshold_2.Editor = new SliderValueEditorViewModel(3,0,10);
            Inp_Threshold_2.Port.IsVisible = false;
            this.Inputs.Add(Inp_Threshold_2);
            //------------------------------------------------------

            this.WhenAnyObservable(
                vm => vm.Inp_Mat.Changed,
                vm => vm.Inp_Threshold_1.Changed,
                vm => vm.Inp_Threshold_2.Changed).Subscribe(newValue =>
                {
                    ApplyFilter();
                });

            //------------------------------------------------------
        }

        static CannyFilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<CannyFilterNodeModel>));
        }
        
        public override Mat ApplyFilter()
        {
            Mat result = new Mat();
            try
            {
                CvInvoke.Canny(Inp_Mat.Value, result, (double)Inp_Threshold_1.Value, (double)Inp_Threshold_2.Value);
            }
            catch (Exception e)
            {
                //TODO: Deal with this error    
                Console.WriteLine("[CannyFilterNodeModel](ApplyFilter) -> " + e.Message);
            }

            SetPreviewAndOutput(result);
            return result;
        }
    }
}
