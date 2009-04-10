using System;
using System.Collections.Generic;
using System.Text;

using Visionador.ConvolutionTemplateClass;

namespace Visionador.GaborMultiplexerClass
{
    // The construction information for the Gabor multiplexer.
    public struct gaborMultiplexerCinfo
    {
        public convolutionTemplate myPositiveSignalSource;
        public convolutionTemplate myNegativeSignalSource;
        public convolutionTemplate myOutput;
    }

    // The Gabor multiplexer.
    public class gaborMultiplexer
    {
        public gaborMultiplexer(gaborMultiplexerCinfo theInfo)
        {
            myPositiveSignalSource = theInfo.myPositiveSignalSource;
            myNegativeSignalSource = theInfo.myNegativeSignalSource;
            myOutput = theInfo.myOutput;
        }

        //  Return false if the dimensions don't add up.
        //  consider adding a center value instead of defaulting to 0.
        public bool multiplex(double upperCeiling, double lowerFloor)
        {

            if ((myPositiveSignalSource.myWidth == myNegativeSignalSource.myWidth) &&
                (myPositiveSignalSource.myWidth == myOutput.myWidth) &&
                (myPositiveSignalSource.myHeight == myNegativeSignalSource.myHeight) &&
                (myPositiveSignalSource.myHeight == myOutput.myHeight))
            {
                for (int widthCounter = 0; widthCounter < myOutput.myWidth; widthCounter++)
                {
                    for (int heightCounter = 0; heightCounter < myOutput.myHeight; heightCounter++)
                    {
                        if (myPositiveSignalSource[widthCounter, heightCounter] > 0.0f)
                        {
                            myOutput[widthCounter, heightCounter] = upperCeiling - myPositiveSignalSource[widthCounter, heightCounter];
                        }
                        else if (myNegativeSignalSource[widthCounter, heightCounter] > 0.0f)
                        {
                            myOutput[widthCounter, heightCounter] = myNegativeSignalSource[widthCounter, heightCounter] - lowerFloor;
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

        public convolutionTemplate myPositiveSignalSource;
        public convolutionTemplate myNegativeSignalSource;
        public convolutionTemplate myOutput;
    }
}
