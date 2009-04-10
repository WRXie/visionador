using System;
using System.Collections.Generic;
using System.Text;

using Visionador.ConvolutionTemplateClass;

using System.Diagnostics;

namespace Visionador.SimpleConvolverClass
{
    // The construction info for the simple convolver.
    public struct simpleConvolverCinfo
    {
        public convolutionTemplate myTemplate;
        public convolutionTemplate myInput;
        public convolutionTemplate myOutput;
    }

    // The simple convolver.  Calling convolve() will convolve the input with 
    // the template and store the result in the output.
    public class simpleConvolver
    {
        public simpleConvolver() { }

        public simpleConvolver(simpleConvolverCinfo theInfo)
        {
            myInput = theInfo.myInput;
            myOutput = theInfo.myOutput;
            myTemplate = theInfo.myTemplate;
        }

        public convolutionTemplate myTemplate;
        public convolutionTemplate myInput;
        public convolutionTemplate myOutput;

        //  Return false if the dimensions don't add up.
        public bool convolve()
        {
            int thisEffWidth = myInput.getEffHeight() - myTemplate.getEffHeight();
            int thisEffHeight = myInput.getEffWidth() - myTemplate.getEffWidth();

            Debug.Assert(thisEffHeight >= 0);
            Debug.Assert(thisEffWidth  >= 0);

            if ((myOutput.getEffHeight() >= thisEffHeight) &&
                (myOutput.getEffWidth() >= thisEffWidth))
            {
                for (int outputWidthCounter = 0; outputWidthCounter < 1 + 2 * thisEffWidth; outputWidthCounter++)
                {
                    for (int outputHeightCounter = 0; outputHeightCounter < 1 + 2 * thisEffHeight; outputHeightCounter++)
                    {
                        //  center + (current offset)
                        int thisOutputX = myOutput.getEffWidth() + (outputWidthCounter - thisEffWidth);
                        int thisOutputY = myOutput.getEffHeight() + (outputHeightCounter - thisEffHeight);

                        double thisOutputValue = 0;

                        for (int templateWidthCounter = 0; templateWidthCounter < 1 + 2 * myTemplate.getEffWidth(); templateWidthCounter++)
                        {
                            for (int templateHeightCounter = 0; templateHeightCounter < 1 + 2 * myTemplate.getEffHeight(); templateHeightCounter++)
                            {
                                int thisTemplateX = templateWidthCounter;
                                int thisTemplateY = templateHeightCounter;

                                // (half the amount by which myInput is larger than myOutput) + (current position on myOutput) + (current template offset)
                                int thisInputX = ( myInput.getEffWidth()  - myOutput.getEffWidth()   ) + thisOutputX + (templateWidthCounter - myTemplate.getEffWidth());
                                int thisInputY = ( myInput.getEffHeight() - myOutput.getEffHeight()  ) + thisOutputY + (templateHeightCounter - myTemplate.getEffHeight());

                                thisOutputValue += myInput[thisInputX, thisInputY] * myTemplate[thisTemplateX, thisTemplateY];
                            }
                        }

                        myOutput[thisOutputX, thisOutputY] = thisOutputValue;
                    }
                }


                return true;
            }
            else
            {
                return false;
            }
        }

        // Convolve n times if possible, return the number of successful convolutions.
        public int iterateConvolve( int n )
        {
            int successes = 0;

            // Keep references to myInput and myOutput.
            convolutionTemplate outputHolder = myOutput;
            convolutionTemplate inputHolder = myInput;

            for (successes = 0; successes < n; successes++)
            {
                convolutionTemplate intermediateTemplate = createSuitableOutput(myInput, myTemplate);

                if ( intermediateTemplate != null )
                {
                    myOutput = intermediateTemplate;
                    convolve();
                    myInput         = intermediateTemplate;
                    outputHolder    = intermediateTemplate;
                }
                else
                {
                    break;
                }
            }

            myInput = inputHolder;
            myOutput = outputHolder;

            return successes;
        }

        // Creates a convolution template suitable for the output of a convolution of theInput with theTemplate.
        public static convolutionTemplate createSuitableOutput(convolutionTemplate theInput, convolutionTemplate theTemplate)
        {
            int thisEffWidth = theInput.getEffHeight() - theTemplate.getEffHeight();
            int thisEffHeight = theInput.getEffWidth() - theTemplate.getEffWidth();

            if ((thisEffHeight >= 0) && (thisEffWidth >= 0))
            {
                convolutionTemplateCinfo theOutputInfo;
                theOutputInfo.myHeight = (2 * thisEffHeight) + 1;
                theOutputInfo.myWidth = (2 * thisEffWidth) + 1;

                convolutionTemplate theOutput = new convolutionTemplate(theOutputInfo);

                return theOutput;
            }
            else
            {
                return null;
            }
        }

        // Create and set myOutput to a convolutionTemplate suitable for myInput and myTemplate.
        public bool makeMyOutputSuitable()
        {
            myOutput = createSuitableOutput( myInput, myOutput );
            
            if (myOutput != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
