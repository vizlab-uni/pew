using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DynamicData;
using Emgu.CV;
using Emgu.CV.Structure;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using ProjectHeracles.Nodes.EditorViewModels;
using ProjectHeracles.Nodes.Views;
using ReactiveUI;

namespace ProjectHeracles.Nodes.ViewModels
{
    //TODO: NEED TO BE FINISHED (DON"T WORK PROPERLY YET)
    //Need to deal when select File or Batch. If batch, needs to pass an array of mats
    public class StartNodeModel : NodeViewModel
    {
        public enum StartType
        {
            File,
            Batch
        }

        private string _TxtFilePath;
        public string TxtFilePath
        {
            get => _TxtFilePath;
            set => this.RaiseAndSetIfChanged(ref _TxtFilePath, value);
        }

        ImageSource _PreviewSource;
        public ImageSource PreviewSource
        {
            get => _PreviewSource;
            set => this.RaiseAndSetIfChanged(ref _PreviewSource, value);
        }

        public ValueNodeInputViewModel<object> Inp_Type { get; }
        public ValueNodeOutputViewModel<Mat> Out_Mat { get; }
        public ValueNodeInputViewModel<double?> Inp_Slider { get; }

        public StartNodeModel()
        {
            //Node Name
            this.Name = "Start";

            //------------------TYPE OF START NODE------------------
            //Inp_Type = new ValueNodeInputViewModel<object>()
            //{
            //    Name = "Type"
            //};
            //Inp_Type.Editor = new EnumValueEditorViewModel(typeof(StartType));
            //Inp_Type.Port.IsVisible = false;
            //------------------------------------------------------

            Inp_Slider = new ValueNodeInputViewModel<double?>()
            {
                Name = "Slider"
            };
            Inp_Slider.Editor = new SliderValueEditorViewModel(0,0,10);
            Inp_Slider.Port.IsVisible = false;

            //------------------OUTPUT MAT CV------------------
            Out_Mat = new ValueNodeOutputViewModel<Mat>()
            {
                Name = "Out_Image"
            };
            //------------------------------------------------------

            //Subscribe changes on in_path to try load an image on Out_Mat in any change
            this.WhenAnyValue(vm => vm.TxtFilePath).Subscribe(newValue =>
            {
                Out_Mat.Value = Observable.Return(LoadImage(newValue));
            });

            //Add inputs and outputs to node
            //this.Inputs.Add(Inp_Type);
            this.Outputs.Add(Out_Mat);
        }
        
        private Mat LoadImage(string path)
        {
            Mat srcImage = null;
            BitmapSource bmpSource;
            try
            {
                srcImage = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.AnyColor);
                bmpSource = HeraclesHelper.ToBitmapSource(srcImage);
            }
            catch (Exception e)
            {
                //If could not load image, load an placeholder
                bmpSource = HeraclesHelper.ToBitmapSource(Properties.Resources.NoneImage);
                //TODO: Deal with this error      
                Console.WriteLine(e.Message);
            }
            PreviewSource = bmpSource;
            return srcImage;
        }
        
        static StartNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new StartNodeView(), typeof(IViewFor<StartNodeModel>));
        }
    }
}
