using ImageChecker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ImageChecker.ViewModels
{
    public class MainViewModel : NotificationObject
    {

        private DelegateCommand _newCommand;
        [XmlIgnore]
        public DelegateCommand NewCommand
        {
            get
            {
                return this._newCommand ?? (this._newCommand = new DelegateCommand(
                    _ =>
                    {
                        InitializeSettings();
                    }


                    ));
            }
        }
        private DelegateCommand _openFileCommand;
        [XmlIgnore]

        public DelegateCommand OpenFileCommand
        {
            get
            {
                return this._openFileCommand ?? (this._openFileCommand = new DelegateCommand(
                    _ =>
                    {
                        this.OpenDialogCallback = OnOpenDialogCallback;
                    }


                    ));
            }
        }

        private Action<bool, string> _openDialogCallback;
        [XmlIgnore]

        public Action<bool, string> OpenDialogCallback
        {
            get { return this._openDialogCallback; }
            private set { SetProperty(ref this._openDialogCallback, value); }
        }
        private void OnOpenDialogCallback(bool isOk, string filePath)
        {
            this.OpenDialogCallback = null;
            if (isOk) {
                OpenSettingFile(filePath);
            }
        }

        private DelegateCommand _saveFileCommand;
        [XmlIgnore]

        public DelegateCommand SaveFileCommand
        {
            get
            {
                return this._saveFileCommand ?? (this._saveFileCommand = new DelegateCommand(
                    _ =>
                    {
                        if (_filepath != null) SaveSetttingsToFile();
                        else
                        this.SaveAsDialogCallback = OnSaveAsDialogCallback;
                    }


                    ));
            }
        }


        private DelegateCommand _saveAsFileCommand;
        [XmlIgnore]

        public DelegateCommand SaveAsFileCommand
        {
            get
            {
                return this._saveAsFileCommand ?? (this._saveAsFileCommand = new DelegateCommand(
                    _ =>
                    {
                        this.SaveAsDialogCallback = OnSaveAsDialogCallback;
                    }


                    ));
            }
        }

        private Action<bool, string> _saveAsDialogCallback;
        [XmlIgnore]

        public Action<bool, string> SaveAsDialogCallback
        {
            get { return this._saveAsDialogCallback; }
            private set { SetProperty(ref this._saveAsDialogCallback, value); }
        }
        private void OnSaveAsDialogCallback(bool isOk, string filePath)
        {
            this.SaveAsDialogCallback = null;
            
            if (isOk) {
                _filepath = filePath;
                SaveSetttingsToFile(); 
            }
        }


        private DelegateCommand _exitCommand;


        [XmlIgnore]

        public DelegateCommand ExitCommand
        {
            get
            {
                return this._exitCommand ?? (this._exitCommand =
                    new DelegateCommand(
                        _ =>
                        {
                            OnExit();
                        }));
            }

        }

       
        private bool OnExit()
        {
            App.Current.Shutdown();
            return true;
        }

        private string _imageFilePath;
        [XmlElement(ElementName = "ImageFilePath")]

        public string ImageFilePath
        {
            set
            {
                SetProperty(ref this._imageFilePath, value);
                AnalyzeCommand.RaiseCanExecuteChanged();
            }
            get { return this._imageFilePath; }
        }
        private DelegateCommand _dropCommand;
        [XmlIgnore]

        public DelegateCommand DropCommand
        {
            get
            {
                return _dropCommand = _dropCommand ?? new DelegateCommand(FileDrop);
            }
        }

        private void FileDrop(object parameter)
        {
            var filepath = ((string[])parameter)[0];
            ImageFilePath = filepath;
           

        }


        private ClusteringSettingViewModel _clusteringSetting;
        [XmlElement(ElementName = "ClusteringSetting")]

        public ClusteringSettingViewModel ClusteringSetting
        {
            get => _clusteringSetting;
            set
            {
                _clusteringSetting = value;
                RaisePropertyChanged();
            }
        }


        private bool isBusy;
        private async void ExecuteCommandTask()
        {
            isBusy = true;
            AnalyzeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            await Task.Run(() => AnalyzeImage());
            AnalyzeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            isBusy = false;
            AnalyzeCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }
        private void AnalyzeImage()
        {

            Bitmap bitmap = new Bitmap(ImageFilePath);

            var imageProcessor = ClusteringSetting.GetProcessor(); 
            (var dst,var centers)=imageProcessor.BeginProcess(bitmap);
            Pallets = new List<HSVFeature>(centers.Select(color=>HSVFeature.RGBtoHSV(color)));

            DstBitmap = dst;
           

        }

        private DelegateCommand _analyzeCommand;
        [XmlIgnore]

        public DelegateCommand AnalyzeCommand
        {
            get
            {
                return _analyzeCommand = _analyzeCommand ?? new DelegateCommand(
                    _ => {

                        ExecuteCommandTask();
                    },

                    _ =>
                    {
                        if (!isBusy & ImageFilePath != null) return true;
                        return false;

                    }
                    );
            }
        }

        private List<HSVFeature> _pallets;
        [XmlIgnore]

        public List<HSVFeature> Pallets
        {

            get
            {
                return this._pallets;
            }
            set
            {
                SetProperty(ref this._pallets, value);
                this._pallets = value;
                
            }

        }
       
        private BitmapSource _dstBitmapSource;
        [XmlIgnore]

        public BitmapSource DstBitmapSource
        {
            get
            {
                return this._dstBitmapSource;
            }
            set
            {
                this._dstBitmapSource = value;
                RaisePropertyChanged();
            }
        }
        private Bitmap _dstBitmap;
        [XmlIgnore]

        public Bitmap DstBitmap
        {
            get
            {
                return this._dstBitmap;
            }
            set
            {
                this._dstBitmap = value;
                using (var ms = new System.IO.MemoryStream())
                {
                    _dstBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);

                    DstBitmapSource =
                       System.Windows.Media.Imaging.BitmapFrame.Create(
                           ms,
                           System.Windows.Media.Imaging.BitmapCreateOptions.None,
                           System.Windows.Media.Imaging.BitmapCacheOption.OnLoad
                       );

                }
            }
        }

        private DelegateCommand _saveCommand;
        [XmlIgnore]

        public DelegateCommand SaveCommand
        {
            get
            {
                return _saveCommand = _saveCommand ?? new DelegateCommand(
                    _ =>
                    {

                        this.SaveImageDialogCallback = OnSaveImageDialogCallback;
                    },
                    _ =>
                    {
                        if ( DstBitmapSource != null && !isBusy) return true;
                        else return false;
                    }

                    ); 
            }
        }

       

        private Action<bool, string> _saveImageDialogCallback;
        [XmlIgnore]


        public Action<bool, string> SaveImageDialogCallback
        {
            get { return this._saveImageDialogCallback; }
            private set { SetProperty(ref this._saveImageDialogCallback, value); }
        }
        private void OnSaveImageDialogCallback(bool isOk, string filePath)
        {
            this.SaveAsDialogCallback = null;

            if (isOk)
            {
                DstBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

       

        private void InitializeSettings()
        {


            _filepath = null;
           
            
            isBusy = false;
       
        }

        private void OpenSettingFile(string filepath)
        {
            InitializeSettings();
            _filepath = filepath;

            var sel = new XmlSerializer(typeof(MainViewModel));

            var fileStream = new FileStream(_filepath, FileMode.Open);
            var vm = (MainViewModel)sel.Deserialize(fileStream);
            ImageFilePath = vm.ImageFilePath;
            ClusteringSetting = vm.ClusteringSetting;
        }

        private string _filepath;
        private void SaveSetttingsToFile()
        {
            StreamWriter writer = new StreamWriter(_filepath);
            var sel = new XmlSerializer(typeof(MainViewModel));
            sel.Serialize(writer,this);
            writer.Close();


        }
        public MainViewModel()
        {
            InitializeSettings();
            ClusteringSetting = new ClusteringSettingViewModel();
          
         
        }

    }
}
