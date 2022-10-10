using ImageChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ImageChecker.ViewModels
{
    public class ClusteringSettingViewModel:NotificationObject
    {
        private int _clusterNum;
        [XmlElement(ElementName="NumberOfClusters")]
        public int ClusterNum
        {
            get
            {
                return this._clusterNum;
            }
            set
            {
                SetProperty(ref this._clusterNum, value);
            }
        }
        private int _loopUpperLimit;
        [XmlElement(ElementName = "LoopUpperLimit")]

        public int LoopUpperLimit
        {
            get
            {
                return this._loopUpperLimit;
            }
            set
            {
                SetProperty(ref this._loopUpperLimit, value);
            }
        }
        private int _seed;
        [XmlElement(ElementName = "Seed")]

        public int Seed
        {
            get
            {
                return this._seed;
            }
            set
            {
                SetProperty(ref this._seed, value);
            }
        }
        private HSVFilterSettingViewModel _HSVSetting;
        [XmlElement(ElementName ="HSVSettings")]
        public HSVFilterSettingViewModel HSVSetting
        {
            get => _HSVSetting;
            set
            {
                _HSVSetting = value;
                RaisePropertyChanged();
            }
        }

        public KMeansHSVProcessor GetProcessor()
        {
            return new KMeansHSVProcessor(HSVSetting.ToFeatureFilters(),ClusterNum,LoopUpperLimit,Seed);
        }


        public ClusteringSettingViewModel()
        {
            HSVSetting = new HSVFilterSettingViewModel();
            HSVSetting.HueStart = 0.0;
            HSVSetting.HueEnd = 361.0;
            HSVSetting.SaturationStart = 0.0;
            HSVSetting.SaturationEnd = 101.0;
            HSVSetting.ValueStart = 0.0;
            HSVSetting.ValueEnd = 101.0;

            ClusterNum = 3;
            LoopUpperLimit = 10;
            Seed = 0;
        }

        public void LoadSetting(XElement settings)
        {

            var ColorRegion = settings.Element("ColorRegion");
            HSVSetting.HueStart = double.Parse(ColorRegion.Element("Hue").Attribute("Start").Value);
            HSVSetting.HueEnd = double.Parse(ColorRegion.Element("Hue").Attribute("End").Value);
            HSVSetting.SaturationStart = double.Parse(ColorRegion.Element("Saturation").Attribute("Start").Value);
            HSVSetting.SaturationEnd = double.Parse(ColorRegion.Element("Saturation").Attribute("End").Value);
            HSVSetting.ValueStart = double.Parse(ColorRegion.Element("Value").Attribute("Start").Value);
            HSVSetting.ValueEnd = double.Parse(ColorRegion.Element("Value").Attribute("End").Value);

            var ClusteringSettings = settings.Element("ClusteringSettings");
            Seed = int.Parse(ClusteringSettings.Attribute("Seed").Value);
            ClusterNum = int.Parse(ClusteringSettings.Attribute("NumberOfClusters").Value);
            LoopUpperLimit = int.Parse(ClusteringSettings.Attribute("LoopUpperLimit").Value);
        }

        public XDocument ToElement()
        {
            return  new XDocument(
               new XElement("Settings",


               new XElement("ColorRegion",
                   new XElement("Hue", new XAttribute("Start", HSVSetting.HueStart), new XAttribute("End",HSVSetting.HueEnd)),
                   new XElement("Saturation", new XAttribute("Start", HSVSetting.SaturationStart), new XAttribute("End", HSVSetting.ValueEnd)),
                   new XElement("Value", new XAttribute("Start", HSVSetting.ValueStart), new XAttribute("End", HSVSetting.ValueEnd))
                   ),

               new XElement("ClusteringSettings",
                   new XAttribute("Seed", Seed),
                   new XAttribute("NumberOfClusters", ClusterNum),
                   new XAttribute("LoopUpperLimit", LoopUpperLimit)
                   )
               )
           );
        } 
    }
}
