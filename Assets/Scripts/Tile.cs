using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile : MonoBehaviour
{
    //public GameObject[] _OutputConnectors;
    //public GameObject[] _InputConnector;





    public LinksManager linksManager;

    public enum TileType { None, Tile_Input, Tile_Output, Tile_Process }
    TileType tileType;

    public string tile_name;
    public Color tile_title_color;

    public TMPro.TMP_Text TMP_Text;
    public RawImage rawImage;

    bool moving;
    Vector2 position_0;
    Vector3 mouse_position_0;

    public void Start()
    {
        TMP_Text.text = tile_name;
        rawImage.color = tile_title_color;

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
        }
    }
}
