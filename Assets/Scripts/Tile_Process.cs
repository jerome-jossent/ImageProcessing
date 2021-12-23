using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile_Process : Tile
{
    public GameObject _InputConnector;
    public GameObject _OutputConnector;
    public abstract void _NewInput(object input);
    public abstract void _NewOutput(object output);

    public new void Start()
    {
        base.Start();
        _SetType(TileType.Tile_Process);
        _InputConnector = transform.Find("Connecteur In").gameObject;
        _OutputConnector = transform.Find("Connecteur Out").gameObject;
    }
}
