using System;
using System.Collections.Generic;
using System.Text;

using Visionador.ConvolutionTemplateClass;


namespace Visionador.ConvolutionTemplateClass.ThreeByThreeSymmetricTemplateClass
{
    //  Construction info for the three by three symmetric template.
    public struct threeByThreeSymmetricTemplateCinfo
    {
        public double myCenterWeight;
        public double myNeighborWeight;
    }

    //  Use this for both "heat blurring" and simple Laplacian templates.
    public class threeByThreeSymmetricTemplate : convolutionTemplate
    {
        //  Properly initial
        public threeByThreeSymmetricTemplate(threeByThreeSymmetricTemplateCinfo theCinfo)
        {
            myWidth = 3;
            myHeight = 3;

            myCenterWeight = theCinfo.myCenterWeight;
            myNeighborWeight = theCinfo.myNeighborWeight;

            myTemplate = new double[3, 3];
            myTemplate[1, 1] = theCinfo.myCenterWeight;

            myTemplate[0, 0] = theCinfo.myNeighborWeight;
            myTemplate[0, 1] = theCinfo.myNeighborWeight;
            myTemplate[0, 2] = theCinfo.myNeighborWeight;

            myTemplate[1, 0] = theCinfo.myNeighborWeight;
            myTemplate[1, 2] = theCinfo.myNeighborWeight;

            myTemplate[2, 0] = theCinfo.myNeighborWeight;
            myTemplate[2, 1] = theCinfo.myNeighborWeight;
            myTemplate[2, 2] = theCinfo.myNeighborWeight;
        }

        // The center and neighbor weight of this template.
        public double myCenterWeight;
        public double myNeighborWeight;
    }
}
