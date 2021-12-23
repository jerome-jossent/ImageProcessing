using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile_Output : Tile
{
    public GameObject _InputConnector;
    public abstract void _NewInput(object input);

    public new void Start()
    {
        base.Start();
        _SetType(TileType.Tile_Output);
        _InputConnector = transform.Find("Connecteur In").gameObject;
    }
}
