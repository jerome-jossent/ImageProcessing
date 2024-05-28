using OpenCVForUnity.CoreModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class MQTTImage_IN : Tile
{
    #region PARAMETERS
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_InputField TMP_InputField_topic;
    
    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text_messagenumbers;

    public string topic;
    [Newtonsoft.Json.JsonIgnore]
    public Mat _mat;

    [Newtonsoft.Json.JsonIgnore]
    public MQTT_JJ_Subscribe mqtt_jj_subs;
    #endregion

    #region UNITY METHODS
    public new void Start()
    {
        base.Start();

        if (_tileInfo == null)
            _tileInfo = new TileInfo(this);
        _tileInfo.type = TileInfo.TileType.MQTTImage;
        typeGeneric = TileTypeGeneric.In;

        mqtt_jj_subs.mqtt = MQTT_JJ._mqtt_client;

        _Subscribe();
    }

    #endregion

    #region UI

    public void _Subscribe()
    {
        topic = TMP_InputField_topic.text;
        mqtt_jj_subs._Subscribe(topic);
    }
    #endregion

    #region INPUT_OUTPUT
    public override void _NewInput(object input)
    {
        throw new System.NotImplementedException();
    }

    public override void _NewOutput(object output)
    {
        TMP_Text_messagenumbers.text = mqtt_jj_subs.nbr_received_messages.ToString();

        byte[] bytes = output as byte[];
        Mat b = new MatOfByte(bytes);

        _mat = OpenCVForUnity.ImgcodecsModule.Imgcodecs.imdecode(b, OpenCVForUnity.ImgcodecsModule.Imgcodecs.IMREAD_UNCHANGED);
        OpenCVForUnity.ImgprocModule.Imgproc.cvtColor(_mat, _mat, OpenCVForUnity.ImgprocModule.Imgproc.COLOR_BGR2RGB);

        LinksManager.Instance._NewData(this, _mat);
    }
    #endregion
}
