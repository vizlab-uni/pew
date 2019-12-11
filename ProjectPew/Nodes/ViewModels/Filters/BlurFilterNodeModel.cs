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
    class BlurFilterNodeModel : FilterNodeModel
    {
        public ValueNodeInputViewModel<double?> Inp_Kernel { get; }
        public ValueNodeInputViewModel<object> Inp_BorderType { get; }

        public BlurFilterNodeModel()
        {
            //Node Name
            this.Name = "Blur Filter";

            //------------------KERNEL SIZE------------------
            Inp_Kernel = new ValueNodeInputViewModel<double?>()
            {
                Name = "Kernel Size"
            };
            Inp_Kernel.Editor = new SliderValueEditorViewModel(3, 3, 30);
            Inp_Kernel.Port.IsVisible = false;
            this.Inputs.Add(Inp_Kernel);
            //------------------------------------------------------

            //------------------BORDER TYPE------------------
            Inp_BorderType = new ValueNodeInputViewModel<object>()
            {
                Name = "Border Type"
            };
            Inp_BorderType.Editor = new EnumValueEditorViewModel(typeof(Emgu.CV.CvEnum.BorderType));
            Inp_BorderType.Port.IsVisible = false;
            this.Inputs.Add(Inp_BorderType);
            //------------------------------------------------------

            this.WhenAnyObservable(
                vm => vm.Inp_Mat.Changed,
                vm => vm.Inp_Kernel.Changed,
                vm => vm.Inp_BorderType.Changed).Subscribe(newValue =>
                {
                    ApplyFilter();
                });

            //------------------------------------------------------
        }

        static BlurFilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<BlurFilterNodeModel>));
        }

        public override Mat ApplyFilter()
        {
            Mat result = new Mat();
            try
            {
                CvInvoke.Blur
                    (
                        Inp_Mat.Value,
                        result,
                        new System.Drawing.Size((int)Inp_Kernel.Value, (int)Inp_Kernel.Value),
                        new System.Drawing.Point(-1, -1), (Emgu.CV.CvEnum.BorderType)Inp_BorderType.Value
                    );
                }
            catch (Exception e)
            {
                //TODO: Deal with this error    
                Console.WriteLine("[BlurFilterNodeModel](ApplyFilter) -> " + e.Message);
            }

            SetPreviewAndOutput(result);
            return result;
        }
    }
}
