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
    class LaplacianFilterNodeModel : FilterNodeModel
    {
        public ValueNodeInputViewModel<double?> Inp_Kernel { get; }
        public ValueNodeInputViewModel<double?> Inp_Scale { get; }
        public ValueNodeInputViewModel<double?> Inp_Delta { get; }
        public ValueNodeInputViewModel<object> Inp_DepthType { get; }
        public ValueNodeInputViewModel<object> Inp_BorderType { get; }

        public LaplacianFilterNodeModel()
        {
            //Node Name
            this.Name = "Laplacian Filter";

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
                vm => vm.Inp_Scale.Changed,
                vm => vm.Inp_Delta.Changed,
                vm => vm.Inp_DepthType.Changed,
                vm => vm.Inp_BorderType.Changed).Subscribe(newValue =>
                {
                    ApplyFilter();
                });
            
        //------------------------------------------------------
    }

    static LaplacianFilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<LaplacianFilterNodeModel>));
        }

        public override Mat ApplyFilter()
        {
            Mat result = new Mat();
            try
            {
                CvInvoke.Laplacian
                    (
                        Inp_Mat.Value,
                        result, 
                        Emgu.CV.CvEnum.DepthType.Default,
                        (int)Inp_Kernel.Value,
                        (int)Inp_Scale.Value,
                        (int)Inp_Delta.Value, 
                        (Emgu.CV.CvEnum.BorderType)Inp_BorderType.Value
                    );
            }
            catch (Exception e)
            {
                //TODO: Deal with this error    
                Console.WriteLine("[LaplacianFilterNodeModel](ApplyFilter) -> " + e.Message);
            }

            SetPreviewAndOutput(result);
            return result;
        }
    }
}
