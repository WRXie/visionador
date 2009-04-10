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


namespace Visionador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string defaultInput = "C:\\Users\\Jarvis\\Documents\\Visual Studio 2005\\Projects\\visionador\\visionador\\testInput.bmp";
            string defaultOutput = "C:\\Users\\Jarvis\\Documents\\Visual Studio 2005\\Projects\\visionador\\visionador\\testOutput.bmp";

            convolutionTemplate source = new convolutionTemplate(defaultInput);
            source.initializeFromBitmap(defaultInput, convolutionTemplate.BITMAP_CONVENTION.LUMINOSITY, 0, 2.0f);
            source.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.LUMINOSITY, 0.0f, 2.0f);

            threeByThreeSymmetricTemplateCinfo templateInfo;
            templateInfo.myCenterWeight = 1.0f;
            templateInfo.myNeighborWeight = -1.0f / 8.0f;
            threeByThreeSymmetricTemplate laplaceTemplate = new threeByThreeSymmetricTemplate(templateInfo);

            simpleConvolverCinfo convolverInfo;
            convolverInfo.myInput = source;
            convolverInfo.myOutput = null;
            convolverInfo.myTemplate = laplaceTemplate;
            simpleConvolver convolver = new simpleConvolver(convolverInfo);

            
            convolver.iterateConvolve(1);


            templateInfo.myCenterWeight = 1.0f;// 0.50f;
            templateInfo.myNeighborWeight = 0.0f;//1.0f / 16.0f;
            threeByThreeSymmetricTemplate heatTemplate = new threeByThreeSymmetricTemplate(templateInfo);

            convolutionTemplateCinfo differentInfo;
            differentInfo.myWidth = 3;
            differentInfo.myHeight = 3;
            convolutionTemplate differentHeatTemplate = new convolutionTemplate( differentInfo );

            const double crossValue = 1.0f / 30.0f;
            const double centerValue = 1.0f;

            differentHeatTemplate[0, 0] = 0;            differentHeatTemplate[1, 0] = crossValue;   differentHeatTemplate[2, 0] = 0;
            differentHeatTemplate[0, 1] = crossValue;   differentHeatTemplate[1, 1] = centerValue;  differentHeatTemplate[2, 1] = crossValue;
            differentHeatTemplate[0, 2] = 0;            differentHeatTemplate[1, 2] = crossValue;   differentHeatTemplate[2, 2] = 0;

            convolver.myInput = convolver.myOutput;
            convolver.myTemplate = differentHeatTemplate;

            convolver.iterateConvolve(1);

            //convolver.myOutput.saveToBitmap(defaultOutput, convolutionTemplate.BITMAP_CONVENTION.BLUE_RED, -1.0f, 1.0f);

            gaborTemplateCinfo gaborInfo;
            gaborInfo.myWidth = 199;
            gaborInfo.myHeight = 199;
            gaborInfo.myFrequency = 1.0f;
            gaborInfo.myOrientation = 0.0f;
            gaborInfo.myScaleWidth = 100.0f;
            gaborInfo.myScaleHeight = 100.0f;
            gaborInfo.myVariance = 1.0f;
            gaborInfo.myWaveType = gaborTemplate.WAVETYPE.COS;

            gaborTemplate testGaborTemplate = new gaborTemplate( gaborInfo );
            //testGaborTemplate.saveToBitmap(defaultOutput);


        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}