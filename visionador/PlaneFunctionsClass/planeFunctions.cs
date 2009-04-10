using System;
using System.Collections.Generic;
using System.Text;

namespace Visionador.PlaneFunctionsClass
{
    // Some useful fuctions in the plane.
    public class planeFunctions
    {
        public static double planeCos(double frequency, double orientation, double x, double y)
        {
            return Math.Cos(2.0f * Math.PI * frequency * (x * Math.Cos(orientation) + y * Math.Sin(orientation)));
        }

        public static double planeSin(double frequency, double orientation, double x, double y)
        {
            return Math.Sin(2.0f * Math.PI * frequency * (x * Math.Cos(orientation) + y * Math.Sin(orientation)));
        }

        public static double planeGaussianAtOrigin(double variance, double x, double y)
        {
            return ((1.0f / (variance * Math.Sqrt(2.0f * Math.PI))) * Math.Exp(-(x * x + y * y) / (2.0f * variance * variance)));
        }
    }
}
