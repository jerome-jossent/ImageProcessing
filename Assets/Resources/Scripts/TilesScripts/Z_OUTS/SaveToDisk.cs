using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;
using System;
using System.Linq;
using TMPro;
using System.IO;

public class SaveToDisk : Tile
{
    #region PARAMETERS
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _folderName;
    public string _filename;
    public Mat _mat;

    public enum Algo { PNG, JPG }
    public Algo _algo;
    List<string> options;

    [Space(10)]
    public TMPro.TMP_Dropdown _algoFormat;
    public GameObject Param_PNG;
    public UI_Parameter png_compression;
    public int PNG_compression;

    public GameObject Param_JPG;
    public UI_Parameter jpg_quality;
    public int JPG_quality;

    Mat _mat_input;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.SaveToDisk;

        //LOAD
        Enum.TryParse(Get("_algo", varType._int), out _algo);
        _folderName = Get("_folderName", varType._string);
        Init_fichiers();

        PNG_compression = int.Parse(Get("PNG_compression", varType._int));
        JPG_quality = int.Parse(Get("JPG_quality", varType._int));

        FillDropDownWithEnum(_algoFormat, typeof(Algo), _algo);
        png_compression._Set("compression", 0, 9, PNG_compression, "0 no comp - 9 max comp");
        jpg_quality._Set("quality", 0, 100, JPG_quality, "");
    }

    #endregion

    #region SET PARAMETERS
    public void _Set_jpg_quality()
    {
        JPG_quality = (int)jpg_quality.slider.value;
        Set("JPG_quality", JPG_quality.ToString());
        _NewOutput(_mat_input);
    }

    public void _Set_png_compression()
    {
        PNG_compression = (int)png_compression.slider.value;
        Set("PNG_compression", PNG_compression.ToString());
        _NewOutput(_mat_input);
    }
    #endregion

    #region UI
    void FillDropDownWithEnum(TMP_Dropdown dd, Type type, Algo value)
    {   // ex : FillDropDownWithEnum(_algoDD, typeof(Algo));
        options = Enum.GetNames(type).ToList();
        _algoFormat.ClearOptions();
        _algoFormat.AddOptions(options);
        _AlgoChange(options.IndexOf(value.ToString()));
    }

    public void _AlgoChange(int newSelection)
    {
        _algo = (Algo)newSelection;
        switch (_algo)
        {
            case Algo.PNG:
                Param_PNG.SetActive(true);
                Param_JPG.SetActive(false);
                break;
            case Algo.JPG:
                Param_PNG.SetActive(false);
                Param_JPG.SetActive(true);
                break;
        }
        Set("_algo", _algo.ToString());
        _NewOutput(_mat_input);
    }

    public void _PickFolder()
    {
        string folder = PlayerPrefs.GetString("FileImage_folderSave");

        GameObject GOFileBrowser = GameObject.Find("FileBrowser");
        Crosstales.FB.FileBrowser fileBrowser = GOFileBrowser.GetComponent<Crosstales.FB.FileBrowser>();
        Crosstales.FB.ExtensionFilter[] ext = new Crosstales.FB.ExtensionFilter[] { new Crosstales.FB.ExtensionFilter { Name = "All", Extensions = new string[] { "*" } } };
        string selectedFolder = fileBrowser.OpenSingleFolder("Select pictures folder", folder);

        if (selectedFolder != null)
        {
            _folderName = selectedFolder;
            Set("_folderName", _folderName);
            Init_fichiers();
        }
    }
    #endregion

    void Init_fichiers()
    {
        TMP_Text_fileName.text = _folderName;
        if (!Directory.Exists(_folderName))
        {
            //TODO ALARM
        }
    }

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        if (input == null) return;
        _mat_input = (Mat)input;
        MatOfInt param;
        string filename = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss.fff");
        switch (_algo)
        {
            case Algo.PNG:
                param = new MatOfInt(new int[] {
                    OpenCVForUnity.ImgcodecsModule.Imgcodecs.IMWRITE_PNG_COMPRESSION,
                    (int)png_compression.slider.value});
                _filename = _folderName + filename + ".png";
                break;

            case Algo.JPG:
                param = new MatOfInt(new int[] {
                    OpenCVForUnity.ImgcodecsModule.Imgcodecs.IMWRITE_JPEG_QUALITY,
                    (int)jpg_quality.slider.value});
                _filename = _folderName + filename + ".jpg";
                break;

            default:
                param = new MatOfInt(new int[] { });
                break;
        }
        OpenCVForUnity.ImgcodecsModule.Imgcodecs.imwrite(_filename, _mat_input, param);
        LinksManager.Instance._NewData(this, null);
    }

    public override void _NewOutput(object output)
    {
        //TODO ?
    }
    #endregion
}