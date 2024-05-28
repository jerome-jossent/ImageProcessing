using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;

public class Resize : Tile
{
    public Mat _mat;


    public float _factor;


    public UI_Parameter factor;

    public UI_Parameter factorX;
    public UI_Parameter factorY;

    public UI_Parameter newWidth;
    public UI_Parameter newHeight;

    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.Resize;

        _factor = float.Parse(Get("_factor", varType._float));
        factor._Set("Facteur", 0.1f, 10f, _factor, "");

    }

    public override void _NewInput(object input)
    {
        _NewOutput(input);
    }

    public override void _NewOutput(object output)
    {
        Mat _mat_input = (Mat)output;
        _mat = new Mat();


        OpenCVForUnity.ImgprocModule.Imgproc.resize(_mat_input, _mat, null, factor.slider.value);

        LinksManager.Instance._NewData(this, _mat);
    }
}
