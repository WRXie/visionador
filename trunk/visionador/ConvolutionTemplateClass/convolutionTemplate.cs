using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using System.Diagnostics;

namespace Visionador.ConvolutionTemplateClass
{
    //  Construction info for a basic convolution template.
    public struct convolutionTemplateCinfo
    {
        //  These should be odd numbers.
        public int myWidth;
        public int myHeight;
    }

    //  The most basic type of convolution template.
    public class convolutionTemplate
    {
        public convolutionTemplate(convolutionTemplateCinfo theInfo)
        {
            Debug.Assert( theInfo.myWidth % 2 == 1  );
            Debug.Assert( theInfo.myHeight % 2 == 1 );

            myWidth = theInfo.myWidth;
            myHeight = theInfo.myHeight;
            initializeToValue(0);
        }

        public convolutionTemplate() {}

        //  Pass a filename from which to initialize.
        public convolutionTemplate(string filename)
        {
            initializeFromBitmap(filename, BITMAP_CONVENTION.LUMINOSITY, 0.0f, 1.0f);
        }

        //  Set the entire matrix to value.  Returns false if it is already initialized.
        public bool initializeToValue(double value)
        {
            if (myTemplate == null)
            {
                myTemplate = new double[myWidth, myHeight];
                for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                    {
                        myTemplate[widthCounter, heightCounter] = value;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // An enum denoting which loading scheme to use.  If this is LUMINOSITY, load and save routines will map the interval (min, max)
        // to the interval (0, 255), quantize outgoing values, and use the luminosity channel (average of RBG).  If this is BLUE_RED,
        // load and save routines will map the interval (min, max) to the interval (-255, 255), quantize outgoing values, and use blue
        // for negative numbers and red for positive.
        public enum BITMAP_CONVENTION
        {
            LUMINOSITY = 0,
            BLUE_RED = 1
        };

        //  Initialize from a given bitmap using the brightness.
        public bool initializeFromBitmap(string filename, BITMAP_CONVENTION theConvention, double min, double max)
        {
            Bitmap bitmapToLoad = new Bitmap(filename);

            //  The width and height must be odd.
            myWidth     = ( (bitmapToLoad.Width % 2)    == 1  ? bitmapToLoad.Width  : ( bitmapToLoad.Width  - 1 )   );
            myHeight    = ( (bitmapToLoad.Height % 2)   == 1  ? bitmapToLoad.Height : ( bitmapToLoad.Height - 1 )   );

            myTemplate = new double[myWidth, myHeight];

            if(Convert.ToBoolean(theConvention))
            {
                for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                    {
                        if( bitmapToLoad.GetPixel(widthCounter, heightCounter).R > 0 )
                        {
                            myTemplate[widthCounter, heightCounter] = Convert.ToDouble(bitmapToLoad.GetPixel(widthCounter, heightCounter).R);
                        }
                        else
                        {
                            myTemplate[widthCounter, heightCounter] = -Convert.ToDouble(bitmapToLoad.GetPixel(widthCounter, heightCounter).B);
                        }

                        myTemplate[widthCounter, heightCounter]     += 255.0f;
                        myTemplate[widthCounter, heightCounter]     /= 510.0f;
                        myTemplate[widthCounter, heightCounter]     *= (max - min);
                        myTemplate[widthCounter, heightCounter]     += min;
                    }
                }
            }
            else
            {
                for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                    {
                        myTemplate[widthCounter, heightCounter] = Convert.ToDouble(bitmapToLoad.GetPixel(widthCounter, heightCounter).B +
                                                                                    bitmapToLoad.GetPixel(widthCounter, heightCounter).G +
                                                                                    bitmapToLoad.GetPixel(widthCounter, heightCounter).R)
                                                                                    / 3.0f;
                        myTemplate[widthCounter, heightCounter]     /= 255.0f;
                        myTemplate[widthCounter, heightCounter]     *= (max - min);
                        myTemplate[widthCounter, heightCounter]     += min;
                    }
                }             
            }

            return true;
        }

        //  Save to a bitmap.
        public bool saveToBitmap(string filename, BITMAP_CONVENTION theConvention, double min, double max)
        {
            Bitmap bitmapToSave = new Bitmap(myWidth, myHeight);

            if (Convert.ToBoolean(theConvention))
            {
                for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                    {
                        double luminosity = myTemplate[widthCounter, heightCounter];

                        // If the luminosity is above the halfway point, it should be stored as red.
                        if (luminosity >= ((max + min) / 2.0f))
                        {
                            // Center it.
                            luminosity -= ((max + min) / 2.0f);

                            // Scale it.
                            luminosity /= ((max - min) / 2.0f);
                            luminosity *= 255.0;

                            int luminosityOut = Convert.ToInt32( Math.Min(luminosity, 255.0) );
                            bitmapToSave.SetPixel(widthCounter, heightCounter,
                                                    Color.FromArgb(luminosityOut, 0, 0));
                        }
                        else
                        {
                            // Center it.
                            luminosity = ((max + min) / 2.0f) - luminosity;

                            // Scale it.
                            luminosity /= ((max - min) / 2.0f);
                            luminosity *= 255.0;

                            int luminosityOut = Convert.ToInt32(Math.Min(luminosity, 255.0));
                            bitmapToSave.SetPixel(widthCounter, heightCounter,
                                                    Color.FromArgb(0, 0, luminosityOut));
                        }
                    }
                }
            }
            else
            {
                for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                    {
                        double luminosity = myTemplate[widthCounter, heightCounter];

                        luminosity -= min;
                        luminosity /= (max - min);
                        luminosity *= 255.0f;

                        int luminosityOut = Convert.ToInt32(Math.Min(luminosity, 255.0));
                        
                        bitmapToSave.SetPixel(widthCounter, heightCounter,
                                                Color.FromArgb(luminosityOut,
                                                                    luminosityOut,
                                                                    luminosityOut
                                                               )
                                              );
                    }
                }
            }
            

            bitmapToSave.Save(filename);

            return true;

            //this.savePath = string.Copy(givenName);

            //Bitmap bitmapToSave = new Bitmap(500, 100);
            //Graphics g = Graphics.FromImage(bitmapToSave);

            //myBitmapToShow = myFilmNet.getPlaneAverageMap();
            //g.DrawImage(myBitmapToShow, 0, 0, 100, 100);

            //myBitmapToShow = myHeatNet.getPlaneAverageMap();
            //g.DrawImage(myBitmapToShow, 100, 0, 100, 100);

            //myBitmapToShow = mySphAveNet.getPlaneAverageMap();
            //g.DrawImage(myBitmapToShow, 200, 0, 100, 100);

            //myBitmapToShow = positiveDifference.getPlaneAverageMap();
            //g.DrawImage(myBitmapToShow, 300, 0, 100, 100);

            //myBitmapToShow = negativeDifference.getPlaneAverageMap();
            //g.DrawImage(myBitmapToShow, 400, 0, 100, 100);
        }

        //  The array which actually holds the values of this matrix.
        public double[,] myTemplate;

        //  Overload the brackets so we can treat the template as an array.
        public double this[int x, int y]
        {
            get
            {
                return myTemplate[x, y];
            }

            set
            {
                myTemplate[x, y] = value;
            }
        }

        //  Returns the effective width for the template, as this will be used much.
        public int getEffWidth()
        {
            return (myWidth - 1) / 2;
        }

        //  Returns the effective height for the template, as this will be used much.
        public int getEffHeight()
        {
            return (myHeight - 1) / 2;
        }

        //  The dimensions of the matrix.
        public int myHeight;
        public int myWidth;

        //  An enum for threshold specification.
        public enum THRESHOLD_TYPE
        {
            ABOVE = 0,
            BELOW
        };

        //  Threshold this matrix:  values <theType> <theBound> will be replaced by <theNewValue>.
        public void threshold(double theBound, THRESHOLD_TYPE theType, double theNewValue)
        {
            for (int widthCounter = 0; widthCounter < myWidth; widthCounter++)
            {
                for (int heightCounter = 0; heightCounter < myHeight; heightCounter++)
                {
                    if (((myTemplate[widthCounter, heightCounter] >= theBound) && !Convert.ToBoolean(theType)) ||
                        ((myTemplate[widthCounter, heightCounter] <= theBound) && Convert.ToBoolean(theType)))
                    {
                        myTemplate[widthCounter, heightCounter] = theNewValue;
                    }
                }
            }
        }

        //  Perform a double threshold.  Checks for proper ordering of the bounds.
        public void clamp(double lowerBound, double theNewLowerValue, double upperBound, double theNewUpperValue)
        {
            if (lowerBound > upperBound)
            {
                clamp(upperBound, theNewUpperValue, lowerBound, theNewLowerValue);
            }
            else
            {
                threshold(upperBound, THRESHOLD_TYPE.ABOVE, theNewUpperValue);
                threshold(lowerBound, THRESHOLD_TYPE.BELOW, theNewLowerValue);
            }
        }
    }
}
