using System;
using System.Collections.Generic;
using System.Text;

using Visionador.ConvolutionTemplateClass;

namespace Visionador.GaborMultiplexerClass
{
    // The construction information for the Gabor multiplexer.
    public struct gaborMultiplexerCinfo
    {
        public convolutionTemplate mySignalSource;
        public convolutionTemplate myOutput;
    }

    // The Gabor multiplexer.
    public class gaborMultiplexer
    {
        public gaborMultiplexer(gaborMultiplexerCinfo theInfo)
        {
            mySignalSource = theInfo.mySignalSource;
            myOutput = theInfo.myOutput;
        }

        //  Return false if the dimensions don't add up.
        //  consider adding a center value instead of defaulting to 0.
        //  The upperCeiling and lowerFloor are the max and min expected signal values, respectively,
        //  which should straddle 0.
        public bool multiplex(double upperCeiling, double lowerFloor)
        {

            if ((mySignalSource.myWidth == myOutput.myWidth) &&
                (mySignalSource.myHeight == myOutput.myHeight))
            {
                for (int widthCounter = 0; widthCounter < myOutput.myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myOutput.myHeight; heightCounter++)
                    {
                        if (mySignalSource[widthCounter, heightCounter] > 0.0f)
                        {
                            //myOutput[widthCounter, heightCounter] = upperCeiling - mySignalSource[widthCounter, heightCounter];
                            myOutput[widthCounter, heightCounter] = upperCeiling - Math.Min( mySignalSource[widthCounter, heightCounter], upperCeiling );
                        }
                        else if (mySignalSource[widthCounter, heightCounter] < 0.0f)
                        {
                            //myOutput[widthCounter, heightCounter] = -mySignalSource[widthCounter, heightCounter] + lowerFloor;
                            myOutput[widthCounter, heightCounter] = -Math.Max( mySignalSource[widthCounter, heightCounter], lowerFloor ) + lowerFloor;
                        }
                        else
                        {
                            myOutput[widthCounter, heightCounter] = 0.0f;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public convolutionTemplate mySignalSource;
        public convolutionTemplate myOutput;
    }
}
