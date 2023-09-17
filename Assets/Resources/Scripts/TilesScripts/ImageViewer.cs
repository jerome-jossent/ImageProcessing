using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : Tile
{
    #region PARAMETERS
    [Newtonsoft.Json.JsonIgnore]
    public RawImage image;
    [Newtonsoft.Json.JsonIgnore]
    public object input;
    Texture2D texture2D;
    RectTransform imageRectTransform;

    public float rotationAngle, rotationAngle_prec;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();
        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.ImageViewer;
        typeGeneric = TileTypeGeneric.Out;
        imageRectTransform = image.GetComponent<RectTransform>();
    }
    public new void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            WorldManager.Instance._ImageViewerMax_Show(texture2D);
        }
    }
    #endregion

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        if (input == null)
        {
            Debug.Log(name + " : input is null");
            return;
        }

        string typ = input.GetType().ToString();
        switch (typ)
        {
            case "UnityEngine.Texture2D":
                texture2D = (Texture2D)input;
                image.texture = texture2D;
                break;

            case "OpenCVForUnity.CoreModule.Mat":
                Mat imgMat = (Mat)input;

//                texture2D = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);
                texture2D = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.BGRA32, false);

                if (imgMat.empty())
                {
                    Debug.Log("IMAGE VIDE !?");
                    image.texture = null;
                }
                else
                {
                    Utils.setDebugMode(true);
                    Mat copy = imgMat.clone();
                    //Core.flip(copy, copy, 0);
                    Utils.matToTexture2D(copy, texture2D);
                    Utils.setDebugMode(false);

                    //AutoSizeMax
                    float sc_w = 280;
                    int sc_h = 250;
                    float sc_r = sc_w / sc_h;
                    float im_r = (float)texture2D.width / texture2D.height;

                    float w, h;
                    if (im_r > sc_r) //largeur image > largeur écran
                    {
                        w = sc_w;
                        h = w / im_r;
                    }
                    else
                    {
                        h = sc_h;
                        w = h * im_r;
                    }
                    imageRectTransform.sizeDelta = new Vector2(w, h);

                    image.texture = texture2D;
                }
                break;
        }
    }

    public override void _NewOutput(object output)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public void _Test()
    {
        Utils.setDebugMode(true);
        Texture2D imgTexture = Resources.Load("face") as Texture2D;

        Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);

        Utils.texture2DToMat(imgTexture, imgMat);
        Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);

        Utils.matToTexture2D(imgMat, texture);
        Utils.setDebugMode(false);

        _NewInput(imgMat);
    }
}