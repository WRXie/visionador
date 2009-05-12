using System;
using System.Collections.Generic;
using System.Text;

using Visionador.ConvolutionTemplateClass;
using Visionador.PlaneFunctionsClass;

using System.Diagnostics;

namespace Visionador.ConvolutionTemplateClass.GaborTemplateClass
{
    //  Construction info for the Gabor template.
    public struct gaborTemplateCinfo
    {
        public double myFrequency;
        public double myOrientation;
        public double myVariance;

        public gaborTemplate.WAVETYPE myWaveType;

        public int myWidth;
        public int myHeight;

        public double myScaleWidth;
        public double myScaleHeight;
    }

    // A template that takes on the values of a Gabor patch.
    public class gaborTemplate : convolutionTemplate
    {
        public gaborTemplate(gaborTemplateCinfo theCinfo)
        {
            Debug.Assert(theCinfo.myWidth % 2 == 1);
            Debug.Assert(theCinfo.myHeight % 2 == 1);

            myHeight = theCinfo.myHeight;
            myWidth = theCinfo.myWidth;

            myFrequency = theCinfo.myFrequency;
            myOrientation = theCinfo.myOrientation;
            myVariance = theCinfo.myVariance;

            myScaleHeight = theCinfo.myScaleHeight;
            myScaleWidth = theCinfo.myScaleWidth;

            amIcos = theCinfo.myWaveType;

            myTemplate = new double[myWidth, myHeight];

            for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
            {
                for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                {
                    double thisX = ((widthCounter - getEffWidth()) * myScaleWidth);
                    double thisY = ((heightCounter - getEffHeight()) * myScaleHeight);

                    if (Convert.ToBoolean(amIcos))
                    {
                        myTemplate[widthCounter, heightCounter] = planeFunctions.planeCos(myFrequency, myOrientation, thisX, thisY) * planeFunctions.planeGaussianAtOrigin(myVariance, thisX, thisY);
                    }
                    else
                    {
                        myTemplate[widthCounter, heightCounter] = planeFunctions.planeSin(myFrequency, myOrientation, thisX, thisY) * planeFunctions.planeGaussianAtOrigin(myVariance, thisX, thisY);
                    }
                }
            }
        }

        // An enum denoting whether a patch is of sine or cosine.
        public enum WAVETYPE
        {
            SIN = 0,
            COS = 1
        };

        // The parameters of the patch.
        public double myFrequency;
        public double myOrientation;
        public double myVariance;

        // The length that a single pixel represents.
        // The row [0, myWidth] will be mapped onto [-myWidth * myScaleWidth, myWidth * myScaleWidth]
        public double myScaleWidth;
        public double myScaleHeight;

        // Is this patch of sine or cosine?
        public WAVETYPE amIcos;
    }
}
