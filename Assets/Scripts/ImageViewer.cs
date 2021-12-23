using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] //Normal & Edit Mode
public class ImageViewer : Tile
{
    [Newtonsoft.Json.JsonIgnore]
    public RawImage image;
    [Newtonsoft.Json.JsonIgnore]
    public object input;

    public override void _NewInput(object input)
    {
        if (input == null)
        {
            Debug.Log(name + " : input is null");
            return;
        }

        string typ = input.GetType().ToString();
        Debug.Log(typ);
        Texture2D texture2D;
        switch (typ)
        {
            case "UnityEngine.Texture2D":
                texture2D = (Texture2D)input;
                image.texture = texture2D;
                break;

            case "OpenCVForUnity.CoreModule.Mat":
                Mat imgMat = (Mat)input; ;
                texture2D = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);
                Utils.setDebugMode(true);
                Utils.matToTexture2D(imgMat, texture2D);
                Utils.setDebugMode(false);
                image.texture = texture2D;
                break;
        }
    }

    public override void _NewOutput(object output)
    {
        throw new System.NotImplementedException();
    }

    public void _Test()
    {
        Utils.setDebugMode(true);
        Texture2D imgTexture = Resources.Load("face") as Texture2D;

        Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);

        Utils.texture2DToMat(imgTexture, imgMat);
        Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);

        Utils.matToTexture2D(imgMat, texture);
        Utils.setDebugMode(false);

        //_NewInput(texture);
        _NewInput(imgMat);
    }

    void OnValidate()
    {
        Start();
    }
}
