using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FolderImages : Tile
{
    #region PARAMETERS
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _folderName;
    [Newtonsoft.Json.JsonIgnore]
    public Mat _mat;

    int index;
    int index_max;
    FileInfo[] fichiers;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.FolderImages;
        typeGeneric = TileTypeGeneric.In;

        _folderName = Get("_folderName", varType._string);
        Init_fichiers();
    }
    #endregion

    #region UI
    public void _PickFolder()
    {                
        GameObject GOFileBrowser = GameObject.Find("FileBrowser");
        Crosstales.FB.FileBrowser fileBrowser = GOFileBrowser.GetComponent<Crosstales.FB.FileBrowser>();
        Crosstales.FB.ExtensionFilter[] ext = new Crosstales.FB.ExtensionFilter[] { new Crosstales.FB.ExtensionFilter { Name = "All", Extensions = new string[] { "*" } } };
        string selectedFolder = fileBrowser.OpenSingleFolder("Select pictures folder", _folderName);

        if (selectedFolder != null)
        {
            _folderName = selectedFolder;
            Set("_folderName", _folderName);
            Init_fichiers();
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
        if (fichiers != null && fichiers.Length > index)
            _NewOutput(fichiers[index]);
    }
    #endregion

    void Init_fichiers()
    {
        TMP_Text_fileName.text = _folderName;

        if (Directory.Exists(_folderName))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_folderName);
            fichiers = directoryInfo.GetFiles();
            index = 0;
            if (fichiers.Length > 0)
                _NewOutput(fichiers[index]);
        }
    }

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        throw new System.NotImplementedException();
    }

    public override void _NewOutput(object output)
    {
        _mat = new Mat();
        FileInfo fi = (FileInfo)output;

        //Debug.Log(fi.FullName);

        //TODO PopUp !?
        //attention pose problème avec des noms de fichiers avec des accents
        _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imread(fi.FullName);

        OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_BGR2RGB);
        if (_mat.empty())
        {
            //TODO PopUp !?
            Debug.Log("IMAGE VIDE !? : " + fi.FullName);
        }

        LinksManager.Instance._NewData(this, _mat);
    }
    #endregion
}