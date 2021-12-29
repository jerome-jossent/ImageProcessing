using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile : MonoBehaviour
{
    public enum TileTypeGeneric { None, In, Process, Out }

    [JsonConverter(typeof(StringEnumConverter))]
    public TileTypeGeneric typeGeneric;

    public TileInfo _tileInfo;

    public GameObject[] _OutputConnectors;
    public GameObject[] _InputConnector;

    public abstract void _NewInput(object input);
    public abstract void _NewOutput(object output);

    public LinksManager linksManager;

    public TMPro.TMP_Text TMP_Text;
    public RawImage rawImage;
    bool moving;
    Vector2 position_0;
    Vector3 mouse_position_0;
    BoxCollider2D boxCollider2D;

    RectTransform rectTransform;
    internal static TileInfo.TileType GetTileType(GameObject connector)
    {
        Tile t = connector.GetComponent<Tile>();
        return t._tileInfo.type;
    }

    public void Start()
    {
        moving = false;
        if (rectTransform == null)
            rectTransform = gameObject.GetComponent<RectTransform>();

        if (linksManager == null)        
            linksManager = LinksManager.Instance;        
    }

    public void _Init(TileInfo tileInfo)
    {
        _tileInfo = tileInfo;
        TMP_Text.text = _tileInfo.name;
        rawImage.color = _tileInfo.title_color.GetColor();
        rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = _tileInfo.size;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.offset = new Vector2(0, 0);
        boxCollider2D.size = gameObject.GetComponent<RectTransform>().sizeDelta;

        transform.localScale = Vector3.one;
        transform.localPosition = _tileInfo.local_position;
    }

    void ActiveConnectorsLink()
    {
        //Debug.Log("ActiveConnectorsLink() " + name);
        //Find GO Connectors
        GameObject GOConnectors = transform.Find("Fond/Connectors").gameObject;
        //In Childs "Outs" & "Ins" make an event in the Button component with linksManager._Click( with itself as GameObject)

        GameObject GOConnectors_Outs = GOConnectors.transform.Find("Outs").gameObject;
        for (int i = 0; i < GOConnectors_Outs.transform.childCount; i++)
        {
            GameObject GOOut = GOConnectors_Outs.transform.GetChild(i).gameObject;
            Button button = GOOut.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            button.onClick = new Button.ButtonClickedEvent();
            // /!\ Non visible dans l'inspecteur !!!!!!!!!
            button.onClick.AddListener(delegate () { linksManager._Click(GOOut); });
        }

        GameObject GOConnectors_Ins = GOConnectors.transform.Find("Ins").gameObject;
        for (int i = 0; i < GOConnectors_Ins.transform.childCount; i++)
        {
            GameObject GOIn = GOConnectors_Ins.transform.GetChild(i).gameObject;
            Button button = GOIn.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            button.onClick = new Button.ButtonClickedEvent();
            // /!\ Non visible dans l'inspecteur !!!!!!!!!
            button.onClick.AddListener(delegate () { linksManager._Click(GOIn); });
        }
    }

    public void OnMouseDown()
    {
        moving = true;
        position_0 = transform.position;
        mouse_position_0 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnMouseUp()
    {
        moving = false;
    }

    bool activated = true;
    public void Update()
    {
        if (activated)
        {
            ActiveConnectorsLink();
            activated = false;
        }
        if (moving)
        {
            Vector2 deplacement = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouse_position_0;
            transform.position = position_0 + deplacement;
            //save position to object
            if (_tileInfo == null)
                _tileInfo = new TileInfo(this);

            _tileInfo.local_position = transform.localPosition;
            _tileInfo.size = rectTransform.sizeDelta;
        }
    }
}

public class TileInfo
{
    public enum TileType { None, FileImage, ImageViewer, FolderImages }

    [JsonConverter(typeof(StringEnumConverter))]
    public TileType type { get; set; }
    public string name { get; set; }
    public SerializableColor title_color { get; set; }
    public Vector2 local_position { get; set; }
    public Vector2 size { get; set; }

    [JsonIgnoreAttribute]
    Tile tile;

    public TileInfo()
    {
    }

    public TileInfo(Tile tile)
    {
        this.tile = tile;
    }

    public void UpdateDATA(Tile tile)
    {
        if (this.tile == null)
            this.tile = tile;
        name = tile.TMP_Text.text;
        title_color = new SerializableColor(tile.rawImage.color);
    }
}