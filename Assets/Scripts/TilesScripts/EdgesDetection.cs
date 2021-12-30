using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVForUnity.CoreModule;

public class EdgesDetection : Tile
{
    public Mat _edges;

    public enum Algo { Sobel, Canny }
    public enum Algo_sobel { x, y, xy }
    public Algo _algo;
    [Space(10)]
    public Algo_sobel _algo_Sobel;
    public int _sobel_ddepth;
    [Space(10)]
    public double _canny_lower_threshold;
    public double _canny_upper_threshold;

    public void _Set_sobel_ddepth(int value)
    {
        _sobel_ddepth = value;
    }
    public void _Set_canny_lower_threshold(int value)
    {
        _canny_lower_threshold = value;
    }
    public void _Set_canny_upper_threshold(int value)
    {
        _canny_upper_threshold = value;
    }

    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.EdgesDetection;
    }

    public override void _NewInput(object input)
    {
        _NewOutput(input);
    }

    public override void _NewOutput(object output)
    {
        Mat _mat_input = (Mat)output;
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
}
