﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class vmCalibrationCamera
    {
        public required string Address { get; set; }

        public double X {  get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
