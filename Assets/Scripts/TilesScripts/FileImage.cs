using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileImage : Tile
{
    #region PARAMETERS
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _fileName;
    [Newtonsoft.Json.JsonIgnore]
    public Mat _mat;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.FileImage;

        _fileName = Get("_fileName", varType._string);
        Init_fichier();
    }
    #endregion

    #region UI
    public void _PickAFile()
    {
        GameObject GOFileBrowser = GameObject.Find("FileBrowser");
        Crosstales.FB.FileBrowser fileBrowser = GOFileBrowser.GetComponent<Crosstales.FB.FileBrowser>();
        Crosstales.FB.ExtensionFilter[] ext = new Crosstales.FB.ExtensionFilter[] { new Crosstales.FB.ExtensionFilter { Name = "All", Extensions = new string[] { "*" } } };
        string folder = "";
        if (System.IO.File.Exists(_fileName))
            folder = System.IO.Path.GetDirectoryName(_fileName);
        string file = fileBrowser.OpenSingleFile("Select a picture file", folder, "", ext);

        if (file != null)
        {
            _fileName = file;
            Set("_fileName", _fileName);
            Init_fichier();
        }
    }

    public void _ReloadFile()
    {
        _NewOutput(_fileName);
    }
    #endregion

    void Init_fichier()
    {
        TMP_Text_fileName.text = _fileName;
        if (System.IO.File.Exists(_fileName))
            _NewOutput(_fileName);
    }

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        throw new System.NotImplementedException();
    }

    public override void _NewOutput(object output)
    {
        _mat = new Mat();
        _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imread((string)output);

        OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_BGR2RGB);

        LinksManager.Instance._NewData(this, _mat);
    }
    #endregion
}