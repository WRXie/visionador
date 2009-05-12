using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Visionador.ConvolutionTemplateClass;
using Visionador.ConvolutionTemplateClass.ThreeByThreeSymmetricTemplateClass;
using Visionador.ConvolutionTemplateClass.GaborTemplateClass;

using Visionador.SimpleConvolverClass;

using Visionador.GaborMultiplexerClass;

namespace Visionador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string defaultInput = "C:\\Users\\Jarvis\\Documents\\Visual Studio 2005\\Projects\\visionador\\visionador\\testInput.bmp";
            string defaultOutput = "C:\\Users\\Jarvis\\Documents\\Visual Studio 2005\\Projects\\visionador\\visionador\\testOutput.bmp";
            string gaborOutput = "C:\\Users\\Jarvis\\Documents\\Visual Studio 2005\\Projects\\visionador\\visionador\\gaborOutput.bmp";

            convolutionTemplate source = new convolutionTemplate(defaultInput);
            source.initializeFromBitmap(defaultInput, convolutionTemplate.BITMAP_CONVENTION.LUMINOSITY, 0, 2.0f);
            //source.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.LUMINOSITY, 0.0f, 2.0f);

            threeByThreeSymmetricTemplateCinfo templateInfo;
            templateInfo.myCenterWeight = 1.0f;// 0.50f;
            templateInfo.myNeighborWeight = 0.0f;//1.0f / 16.0f;
            threeByThreeSymmetricTemplate heatTemplate = new threeByThreeSymmetricTemplate(templateInfo);


            convolutionTemplateCinfo differentInfo;
            differentInfo.myWidth = 3;
            differentInfo.myHeight = 3;
            convolutionTemplate differentHeatTemplate = new convolutionTemplate(differentInfo);
            {
                const double crossValue = 0.1f;//1.0f / 30.0f;
                const double centerValue = 0.50f;

                differentHeatTemplate[0, 0] = 0; differentHeatTemplate[1, 0] = crossValue; differentHeatTemplate[2, 0] = 0;
                differentHeatTemplate[0, 1] = crossValue; differentHeatTemplate[1, 1] = centerValue; differentHeatTemplate[2, 1] = crossValue;
                differentHeatTemplate[0, 2] = 0; differentHeatTemplate[1, 2] = crossValue; differentHeatTemplate[2, 2] = 0;
            }

            simpleConvolverCinfo convolverInfo;
            convolverInfo.myInput = source;
            convolverInfo.myOutput = null;
            convolverInfo.myTemplate = differentHeatTemplate;
            simpleConvolver convolver = new simpleConvolver(convolverInfo);


            convolver.iterateConvolve(3);

            

            templateInfo.myCenterWeight = 1.0f;
            templateInfo.myNeighborWeight = -1.0f / 8.0f;
            threeByThreeSymmetricTemplate laplaceTemplate = new threeByThreeSymmetricTemplate(templateInfo);

            convolver.swapAndSuit(laplaceTemplate);

            convolver.iterateConvolve(1);

            //convolver.myOutput.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -1.0f, 1.0f);
            //convolver.myOutput.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -.50f, .50f);
            //convolver.myOutput.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -.250f, .250f);

            gaborMultiplexerCinfo gmInfo;
            gmInfo.mySignalSource = convolver.myOutput;
            gmInfo.myOutput = convolver.myOutput;
            gaborMultiplexer aMultiplexer = new gaborMultiplexer(gmInfo);

            aMultiplexer.multiplex(0.250f, -0.250f);

            convolver.myOutput.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -0.5f, 0.5f);

            gaborTemplateCinfo gaborInfo;
            gaborInfo.myWidth = 399;
            gaborInfo.myHeight = 399;
            gaborInfo.myFrequency = 1.0f;
            gaborInfo.myOrientation = Math.PI / 2.0f;
            gaborInfo.myScaleWidth = 1.0 / 200.0f;
            gaborInfo.myScaleHeight = 1.0 / 200.0f;
            gaborInfo.myVariance = 1.0f;
            gaborInfo.myWaveType = gaborTemplate.WAVETYPE.COS;

            gaborTemplate testGaborTemplate = new gaborTemplate(gaborInfo);
            testGaborTemplate.saveToBitmap(gaborOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -1.0f, 1.0f);


        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}