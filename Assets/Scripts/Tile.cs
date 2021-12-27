using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile : MonoBehaviour
{
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

    internal static TileInfo.TileType GetTileType(GameObject connector)
    {
        Tile t = connector.GetComponent<Tile>();
        return t._tileInfo.type;
    }

    public void Start()
    {
        moving = false;
    }

    public void _Init(TileInfo tileInfo)
    {
        _tileInfo = tileInfo;
        TMP_Text.text = _tileInfo.name;
        rawImage.color = _tileInfo.title_color.GetColor();

        gameObject.GetComponent<RectTransform>().sizeDelta = _tileInfo.size;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.offset = new Vector2(0, 0);
        boxCollider2D.size = gameObject.GetComponent<RectTransform>().sizeDelta;

        transform.localScale = Vector3.one;
        transform.localPosition = _tileInfo.local_position;
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

    public void Update()
    {
        if (moving)
        {
            Vector2 deplacement = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouse_position_0;
            transform.position = position_0 + deplacement;
            if(_tileInfo==null)
                _tileInfo = new TileInfo(this);
            _tileInfo.local_position = transform.localPosition;
        }
    }
}

public class TileInfo
{
    public enum TileType { None, FileImage, ImageViewer }

    [JsonConverter(typeof(StringEnumConverter))]
    public TileType type { get; set; }
    public string name { get; set; }
    public SerializableColor title_color { get; set; }
    public Vector2 local_position { get; set; }
    public Vector2 size { get; set; }

    [JsonIgnoreAttribute]
    Tile tile;

    public TileInfo(Tile tile)
    {
        this.tile = tile;
    }

    public void UpdateDATA()
    {
        name = tile.TMP_Text.text;
        title_color = new SerializableColor(tile.rawImage.color);
    }
}