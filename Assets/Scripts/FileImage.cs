using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //Normal & Edit Mode
public class FileImage : Tile_Input
{
    public TMPro.TMP_Text TMP_Text_fileName;
    public string _fileName;
    public Mat _mat;

    public override void _NewOutput(object output)
    {
        _mat = new Mat();
        _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imread((string)output);
        linksManager._NewData(this, _mat);
    }

    public void _PickAFile()
    {
        string folder = PlayerPrefs.GetString("FileImage_folder");

        string extensions = "";

        string file = UnityEditor.EditorUtility.OpenFilePanel("Select a picture file", folder, extensions);


        if (file != "")
        {
            Debug.Log(file);
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

    void OnValidate()
    {
        Start();
    }
}