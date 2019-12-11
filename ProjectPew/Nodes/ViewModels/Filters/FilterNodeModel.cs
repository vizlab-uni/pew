using System;
using System.Reactive.Linq;
using System.Windows.Media;
using DynamicData;
using Emgu.CV;
using NodeNetwork;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using ProjectHeracles.Nodes.EditorViewModels;
using ProjectHeracles.Nodes.Views;
using ReactiveUI;

namespace ProjectHeracles.Nodes.ViewModels.Filters
{
    public abstract class FilterNodeModel : NodeViewModel
    {
        ImageSource _PreviewSource;
        public ImageSource PreviewSource
        {
            get => _PreviewSource;
            set => this.RaiseAndSetIfChanged(ref _PreviewSource, value);
        }

        public ValueNodeInputViewModel<Mat> Inp_Mat { get; }
        public ValueNodeOutputViewModel<Mat> Out_Mat { get; }

        public FilterNodeModel()
        {
            Inp_Mat = new ValueNodeInputViewModel<Mat>()
            {
                Name = "In_Image"
            };
            this.Inputs.Add(Inp_Mat);

            Out_Mat = new ValueNodeOutputViewModel<Mat>()
            {
                Name = "Out_Image"
                
            };
            
            PreviewSource = HeraclesHelper.ToBitmapSource(Properties.Resources.NoneImage);
            Out_Mat.Value = null;

            this.Outputs.Add(Out_Mat);
        }

        static FilterNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FilterNodeView(), typeof(IViewFor<FilterNodeModel>));
        }

        public abstract Mat ApplyFilter();

        public void SetPreviewAndOutput(Mat image)
        {
            if (image != null)
            {
                try
                {
                    PreviewSource = HeraclesHelper.ToBitmapSource(image);
                    Out_Mat.Value = Observable.Return(image);
                }
                catch (Exception e)
                {
                    PreviewSource = HeraclesHelper.ToBitmapSource(Properties.Resources.NoneImage);
                    Out_Mat.Value = null;

                    Console.WriteLine("[FilterNodeModel](SetPreviewAndOutput) -> " + e.Message);
                }
            }
        }
    }
}
