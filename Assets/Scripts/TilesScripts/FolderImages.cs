using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FolderImages : Tile
{
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _folderName;
    [Newtonsoft.Json.JsonIgnore]
    public Mat _mat;

    int index;
    int index_max;
    private FileInfo[] fichiers;

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
        _tileInfo.type = TileInfo.TileType.FolderImages;
        typeGeneric = TileTypeGeneric.In;
    }

    public override void _NewInput(object input)
    {
        throw new System.NotImplementedException();
    }

    public override void _NewOutput(object output)
    {
        _mat = new Mat();
        FileInfo fi = (FileInfo)output;
        _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imread(fi.FullName);

        OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_BGR2RGB);

        LinksManager.Instance._NewData(this, _mat);
    }

    public void _PickFolder()
    {
        string folder = PlayerPrefs.GetString("FileImage_folder");

        string defaultname = "";

        string selectedFolder = UnityEditor.EditorUtility.OpenFolderPanel("Select folder of pictures file", folder, defaultname);

        if (selectedFolder != "")
        {
            //Debug.Log(selectedFolder);
            _folderName = selectedFolder;
            TMP_Text_fileName.text = _folderName;
            PlayerPrefs.SetString("FileImage_folder", _folderName);
            DirectoryInfo directoryInfo = new DirectoryInfo(_folderName);
            fichiers = directoryInfo.GetFiles();
            index = 0;
            if (fichiers.Length > 0)
                _NewOutput(fichiers[index]);
        }
    }

    public void _NextFile()
    {
        index++;
        if (index > fichiers.Length - 1)
            index = 0;
        _ReloadFile();
    }
    public void _PreviousFile()
    {
        index--;
        if (index < 0)
            index = fichiers.Length - 1;
        _ReloadFile();
    }

    public void _ReloadFile()
    {
        if (fichiers.Length > index)
            _NewOutput(fichiers[index]);
    }
}
