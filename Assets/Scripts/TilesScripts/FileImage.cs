using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode] //Normal & Edit Mode
public class FileImage : Tile
{
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _fileName;
    [Newtonsoft.Json.JsonIgnore]
    public Mat _mat;

    public new void Start()
    {
        base.Start();
        //TileInfo ti = new TileInfo()
        //{
        //    name = "FileImage 1",
        //    title_color = new SerializableColor(0.2f, 0.5f, 0.9f),
        //    type = TileInfo.TileType.FileImage,
        //    local_position = new Vector2(-420, 140),
        //    size = new Vector2(300, 215)
        //};
        //_Init(ti);

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.FileImage;
    }

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

    public void _PickAFile()
    {
        string folder = PlayerPrefs.GetString("FileImage_folder");


        GameObject GOFileBrowser = GameObject.Find("FileBrowser");
        Crosstales.FB.FileBrowser fileBrowser = GOFileBrowser.GetComponent<Crosstales.FB.FileBrowser>();
        Crosstales.FB.ExtensionFilter[] ext = new Crosstales.FB.ExtensionFilter[] { new Crosstales.FB.ExtensionFilter { Name = "All", Extensions = new string[] { "*" } } };
        string file = fileBrowser.OpenSingleFile("Select a picture file", folder, "", ext);
        //        string extensions = "";
        //string file = UnityEditor.EditorUtility.OpenFilePanel("Select a picture file", folder, extensions);


        if (file != "")
        {
            //Debug.Log(file);
            _fileName = file;
            TMP_Text_fileName.text = _fileName;
            folder = System.IO.Path.GetDirectoryName(_fileName);
            PlayerPrefs.SetString("FileImage_folder", folder);

            _NewOutput(_fileName);
        }
    }

    public void _ReloadFile()
    {
        _NewOutput(_fileName);
    }

    //void OnValidate()
    //{
    //    Start();
    //}
}