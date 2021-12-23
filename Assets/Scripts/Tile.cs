using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile : MonoBehaviour
{
    [Newtonsoft.Json.JsonIgnore]
    public GameObject[] _OutputConnectors;
    [Newtonsoft.Json.JsonIgnore]
    public GameObject[] _InputConnector;

    public abstract void _NewInput(object input);
    public abstract void _NewOutput(object output);

    [Newtonsoft.Json.JsonIgnore]
    public LinksManager linksManager;

    public enum TileType { None, FileImage, ImageViewer }

    TileType tileType;

    public string tile_name;

    [Newtonsoft.Json.JsonIgnore] // :(
    public Color tile_title_color;

    [Newtonsoft.Json.JsonIgnore]
    public TMPro.TMP_Text TMP_Text;

    [Newtonsoft.Json.JsonIgnore]
    public RawImage rawImage;

    [Newtonsoft.Json.JsonIgnore] 
    bool moving;

    [Newtonsoft.Json.JsonIgnore] 
    Vector2 position_0;

    [Newtonsoft.Json.JsonIgnore] 
    Vector3 mouse_position_0;

    Vector3 tile_Position;

    public void Start()
    {
        TMP_Text.text = tile_name;
        rawImage.color = tile_title_color;
        tile_Position = transform.position;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = gameObject.GetComponent<RectTransform>().sizeDelta;
        boxCollider2D.offset = new Vector2(0, 0);

        moving = false;
    }

    public TileType _GetType()
    {
        return tileType;
    }
    public void _SetType(TileType tileType)
    {
        this.tileType = tileType;
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

    internal static TileType GetTileType(GameObject connector)
    {
        Tile t1 = connector.GetComponent<Tile>();
        return t1._GetType();
    }

    public void Update()
    {
        if (moving)
        {
            Vector2 deplacement = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouse_position_0;
            transform.position = position_0 + deplacement;
            tile_Position = transform.position;
        }
    }
}
