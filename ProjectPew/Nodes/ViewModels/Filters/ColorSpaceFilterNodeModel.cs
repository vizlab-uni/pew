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
    class ColorSpaceFilterNodeModel : FilterNodeModel
    {
        public enum ColorSpace
        {
            RGB,
            RGBA,
            GRAYSCALE,
            HSV
        }

        public ValueNodeInputViewModel<object> Inp_ColorSpace { get; }

        public ColorSpaceFilterNodeModel()
        {
            //Node Name
            this.Name = "Color Space Filter";

            //------------------TYPE OF FILTER------------------
            Inp_ColorSpace = new ValueNodeInputViewModel<object>()
            {
                Name = "Color Space"
            };
            Inp_ColorSpace.Editor = new EnumValueEditorViewModel(typeof(ColorSpace));
            Inp_ColorSpace.Port.IsVisible = false;

            this.Inputs.Add(Inp_ColorSpace);
            //------------------------------------------------------
            
            this.WhenAnyObservable(
                vm => vm.Inp_Mat.Changed, 
                vm => vm.Inp_ColorSpace.Changed).Subscribe(newValue =>
                {
                    ApplyFilter();
                });
        }

        static ColorSpaceFilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<ColorSpaceFilterNodeModel>));
        }
        
        public override Mat ApplyFilter()
        {
            Mat result = null;
            try
            {
                switch (Inp_ColorSpace.Value)
                {
                    //TODO: Actually the RGB and RGBA Converter need to be reviwed
                    case ColorSpace.RGB:
                    case ColorSpace.RGBA:
                        result = Inp_Mat.Value;
                        break;
                    case ColorSpace.GRAYSCALE:
                        result = HeraclesHelper.GetGrayscaleImage(Inp_Mat.Value);
                        break;
                    case ColorSpace.HSV:
                        result = HeraclesHelper.ConvertToHSV(Inp_Mat.Value);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                //TODO: Deal with this error    
                Console.WriteLine("[ColorSpaceFilterNode](ApplyFilter) -> " + e.Message);
            }

            SetPreviewAndOutput(result);
            return result;
        }
    }
}
