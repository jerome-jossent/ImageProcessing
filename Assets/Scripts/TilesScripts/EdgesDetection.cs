using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using System;
using System.Linq;
using TMPro;

public class EdgesDetection : Tile
{
    #region PARAMETERS
    public Mat _edges;

    public enum Algo { Sobel, Canny }
    public enum Algo_sobel { x, y, xy }
    public Algo _algo;
    List<string> options;
    [Space(10)]
    public Algo_sobel _algo_Sobel;
    public int _sobel_ddepth;
    [Space(10)]
    public int _canny_lower_threshold;
    public int _canny_upper_threshold;

    public TMPro.TMP_Dropdown _algoDD;
    public GameObject Param_Sobel;
    public UI_Parameter sobel_ddepth;

    public GameObject Param_Canny;
    public UI_Parameter canny_lower_threshold;
    public UI_Parameter canny_upper_threshold;

    Mat _mat_input;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.EdgesDetection;

        _sobel_ddepth = int.Parse(Get("_sobel_ddepth", varType._int));
        _canny_lower_threshold = int.Parse(Get("_canny_lower_threshold", varType._int));
        _canny_upper_threshold = int.Parse(Get("_canny_upper_threshold", varType._int));

        FillDropDownWithEnum(_algoDD, typeof(Algo));
        sobel_ddepth._Set("ddepth", -1, 5, _sobel_ddepth,
            "ddepth - Une variable entière représentant la profondeur de l’image (-1)\n" +
            "dx - Une variable entière représentant la dérivée x. (0 ou 1)\n" +
            "dy - Une variable entière représentant la dérivée y. (0 ou 1)");
        canny_lower_threshold._Set("seuil bas", 0, 255, _canny_lower_threshold, "");
        canny_upper_threshold._Set("seuil haut", 0, 255, _canny_upper_threshold, "");
    }

    #endregion

    #region SET PARAMETERS
    public void _Set_sobel_ddepth()
    {
        _sobel_ddepth = (int)sobel_ddepth.slider.value;
        Set("_sobel_ddepth", _sobel_ddepth.ToString());
        _NewOutput(_mat_input);
    }
    public void _Set_canny_lower_threshold()
    {
        _canny_lower_threshold = (int)canny_lower_threshold.slider.value;
        Set("_canny_lower_threshold", _canny_lower_threshold.ToString());
        _NewOutput(_mat_input);
    }
    public void _Set_canny_upper_threshold()
    {
        _canny_upper_threshold = (int)canny_upper_threshold.slider.value;
        Set("_canny_upper_threshold", _canny_upper_threshold.ToString());
        _NewOutput(_mat_input);
    }
    #endregion

    #region UI
    void FillDropDownWithEnum(TMP_Dropdown dd, Type type)
    {   // ex : FillDropDownWithEnum(_algoDD, typeof(Algo));
        options = Enum.GetNames(type).ToList();
        _algoDD.ClearOptions();
        _algoDD.AddOptions(options);
        _AlgoChange(_algoDD.value);
    }

    public void _AlgoChange(int newSelection)
    {
        _algo = (Algo)newSelection;
        switch (_algo)
        {
            case Algo.Sobel:
                Param_Sobel.SetActive(true);
                Param_Canny.SetActive(false);
                break;
            case Algo.Canny:
                Param_Sobel.SetActive(false);
                Param_Canny.SetActive(true);
                break;
        }
        Set("_algo", _algo.ToString());
        _NewOutput(_mat_input);
    }
    #endregion

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        _NewOutput(input);
    }

    public override void _NewOutput(object output)
    {
        if (output == null) return;

        _mat_input = (Mat)output;
        _edges = new Mat();

        switch (_algo)
        {
            case Algo.Sobel:
                int _sobel_dx = 0, _sobel_dy = 0;
                switch (_algo_Sobel)
                {
                    case Algo_sobel.x: _sobel_dx = 1; _sobel_dy = 0; break;
                    case Algo_sobel.y: _sobel_dx = 0; _sobel_dy = 1; break;
                    case Algo_sobel.xy: _sobel_dx = 1; _sobel_dy = 1; break;
                }
                OpenCVForUnity.ImgprocModule.Imgproc.Sobel(_mat_input, _edges, _sobel_ddepth, _sobel_dx, _sobel_dy);
                break;

            case Algo.Canny:
                OpenCVForUnity.ImgprocModule.Imgproc.Canny(_mat_input, _edges, _canny_lower_threshold, _canny_upper_threshold);
                break;
        }
        LinksManager.Instance._NewData(this, _edges);
    }
#endregion
}