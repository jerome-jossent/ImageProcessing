using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;

public class ToGray : Tile
{
    public Mat _mat;

    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.ToGray;
    }

    public override void _NewInput(object input)
    {
        _NewOutput(input);
    }

    public override void _NewOutput(object output)
    {
        Mat _mat_input = (Mat)output;
        _mat = new Mat();
        int channels = _mat_input.channels();
        switch (channels)
        {
            case 1:
                _mat=_mat_input.clone();
                break;
            case 3:
                OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat_input, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_RGB2GRAY);
                break;
            case 4:
                OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat_input, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_RGBA2GRAY);
                break;
        }
        LinksManager.Instance._NewData(this, _mat);
    }
}