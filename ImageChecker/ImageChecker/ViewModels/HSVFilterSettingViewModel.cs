﻿using ImageChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.ViewModels
{
    public class HSVFilterSettingViewModel:NotificationObject
    {
        private double _hueStart;
        public double HueStart
        {
            get
            {
                return this._hueStart;
            }
            set
            {
                SetProperty(ref this._hueStart, value);
            }
        }

        private double _hueEnd;
        public double HueEnd
        {
            get
            {
                return this._hueEnd;
            }
            set
            {
                SetProperty(ref this._hueEnd, value);

            }
        }

       
        private double _saturationStart;
        public double SaturationStart
        {
            get
            {
                return this._saturationStart;
            }
            set
            {
                SetProperty(ref this._saturationStart, value);

            }
        }

        private double _saturationEnd;
        public double SaturationEnd
        {
            get
            {
                return this._saturationEnd;
            }
            set
            {

                SetProperty(ref this._saturationEnd, value);

            }
        }

        private double _valueStart;
        public double ValueStart
        {
            get
            {
                return this._valueStart;
            }
            set
            {
                SetProperty(ref this._valueStart, value);
            }
        }

        private double _valueEnd;
        public double ValueEnd
        {
            get
            {
                return this._valueEnd;
            }
            set
            {
                SetProperty(ref this._valueEnd, value);

            }
        }


        public FeatureFilters<HSVFeature> ToFeatureFilters()
        {
            return new FeatureFilters<HSVFeature>(new List<IFeatureFilter<HSVFeature>>() {
                new HueFeatureFilter(HueStart,HueEnd),
                new SaturationFeatureFileter(SaturationStart,SaturationEnd),
                new ValueFeatureFilter(ValueStart,ValueEnd),
            });
        } 
    
       
    
    }
}
