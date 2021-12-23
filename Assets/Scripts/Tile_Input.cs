using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile_Input : Tile
{
    public GameObject _OutputConnector;
    public abstract void _NewOutput(object output);

    public new void Start()
    {
        base.Start();
        _SetType(TileType.Tile_Input);
        _OutputConnector = transform.Find("Connecteur Out").gameObject;
    }
}
