﻿//
// This file is part of Biological Signals Acquisition Framework (BSA-F).
// Copyright © Adriano Repetti 2016.
//
// BSA-F is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BSA-F is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with BSA-F. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Numerics;

namespace Bsa.Dsp
{
    static class Mathx
    {
        public static double Sqr(double x)
        {
            return x * x;
        }

        public static Complex Expj(double x)
        {
            return new Complex(Math.Cos(x), Math.Sin(x)); 
        }

        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(1 + x * x));
        }

        public static double Clip(double minimum, double maximum, double value)
        {
            return Math.Max(minimum, Math.Min(maximum, value));
        }
    }
}
