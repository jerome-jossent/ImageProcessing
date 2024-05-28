using OpenCVForUnity.CoreModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCV_TESTJJ : MonoBehaviour
{

    public bool runtest = false;

    public Mat mat;

    void Update()
    {
        if (runtest)
        {
            runtest = false;

            mat = Read(@"D:\DATA\PNG\vr a 2.png");
            mat = Read(@"D:\DATA\images_out\teszzt.jpg");

            Save(mat, @"D:\DATA\images_out\");
        }
    }

    private void Save(Mat mat, string destinationFolder)
    {
        string savename = destinationFolder + "test";
        MatOfInt param;

        //param = new MatOfInt(new int[] {
        //            OpenCVForUnity.ImgcodecsModule.Imgcodecs.IMWRITE_PNG_COMPRESSION,
        //            5});
        //savename +=".png";

        param = new MatOfInt(new int[] {
                    OpenCVForUnity.ImgcodecsModule.Imgcodecs.IMWRITE_JPEG_QUALITY,
                    50});
        savename += ".jpg";

        OpenCVForUnity.ImgcodecsModule.Imgcodecs.imwrite(savename, mat, param);
    }

    Mat Read(string filename)
    {
        Mat _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imread(filename);

        //OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat, _mat, 
        //    OpenCVForUnity.ImgprocModule.Imgproc.COLOR_BGR2RGB);
        return _mat;
    }

}
