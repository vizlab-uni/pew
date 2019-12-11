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
    class SobelFilterNodeModel : FilterNodeModel
    {
        public ValueNodeInputViewModel<double?> Inp_XOrder { get; }
        public ValueNodeInputViewModel<double?> Inp_YOrder { get; }
        public ValueNodeInputViewModel<double?> Inp_Kernel { get; }
        public ValueNodeInputViewModel<double?> Inp_Scale { get; }
        public ValueNodeInputViewModel<double?> Inp_Delta { get; }
        public ValueNodeInputViewModel<object> Inp_DepthType { get; }
        public ValueNodeInputViewModel<object> Inp_BorderType { get; }

        public SobelFilterNodeModel()
        {
            //Node Name
            this.Name = "Sobel Filter";

            //------------------X ORDER------------------
            Inp_XOrder = new ValueNodeInputViewModel<double?>()
            {
                Name = "X Order"
            };
            Inp_XOrder.Editor = new SliderValueEditorViewModel(1, 0, 1);
            Inp_XOrder.Port.IsVisible = false;
            this.Inputs.Add(Inp_XOrder);
            //------------------------------------------------------

            //------------------Y ORDER------------------
            Inp_YOrder = new ValueNodeInputViewModel<double?>()
            {
                Name = "Y Order"
            };
            Inp_YOrder.Editor = new SliderValueEditorViewModel(0, 0, 1);
            Inp_YOrder.Port.IsVisible = false;
            this.Inputs.Add(Inp_YOrder);
            //------------------------------------------------------

            //------------------KERNEL SIZE------------------
            Inp_Kernel = new ValueNodeInputViewModel<double?>()
            {
                Name = "Kernel Size"
            };
            Inp_Kernel.Editor = new SliderValueEditorViewModel(3, 3, 15);
            Inp_Kernel.Port.IsVisible = false;
            this.Inputs.Add(Inp_Kernel);
            //------------------------------------------------------

            //------------------SCALE------------------
            Inp_Scale = new ValueNodeInputViewModel<double?>()
            {
                Name = "Scale"
            };
            Inp_Scale.Editor = new SliderValueEditorViewModel(1, 1, 8);
            Inp_Scale.Port.IsVisible = false;
            this.Inputs.Add(Inp_Scale);
            //------------------------------------------------------

            //------------------DELTA------------------
            Inp_Delta = new ValueNodeInputViewModel<double?>()
            {
                Name = "Delta"
            };
            Inp_Delta.Editor = new SliderValueEditorViewModel(0, 0, 5);
            Inp_Delta.Port.IsVisible = false;
            this.Inputs.Add(Inp_Delta);
            //------------------------------------------------------

            //------------------DEPTH TYPE------------------
            Inp_DepthType = new ValueNodeInputViewModel<object>()
            {
                Name = "Depth Type"
            };
            Inp_DepthType.Editor = new EnumValueEditorViewModel(typeof(Emgu.CV.CvEnum.DepthType));
            Inp_DepthType.Port.IsVisible = false;
            this.Inputs.Add(Inp_DepthType);
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
                vm => vm.Inp_XOrder.Changed,
                vm => vm.Inp_YOrder.Changed,
                vm => vm.Inp_Scale.Changed,
                vm => vm.Inp_Delta.Changed,
                vm => vm.Inp_DepthType.Changed,
                vm => vm.Inp_BorderType.Changed).Subscribe(newValue =>
                {
                    ApplyFilter();
                });

            //------------------------------------------------------
        }

        static SobelFilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<SobelFilterNodeModel>));
        }

        public override Mat ApplyFilter()
        {
            Mat result = new Mat();
            try
            {
                CvInvoke.Sobel
                    (
                        Inp_Mat.Value,
                        result,
                        Emgu.CV.CvEnum.DepthType.Default,
                        (int)Inp_XOrder.Value,
                        (int)Inp_YOrder.Value,
                        (int)Inp_Kernel.Value,
                        (int)Inp_Scale.Value,
                        (int)Inp_Delta.Value,
                        (Emgu.CV.CvEnum.BorderType)Inp_BorderType.Value
                    );
            }
            catch (Exception e)
            {
                //TODO: Deal with this error    
                Console.WriteLine("[SobelFilterNodeModel](ApplyFilter) -> " + e.Message);
            }

            SetPreviewAndOutput(result);
            return result;
        }
    }
}
