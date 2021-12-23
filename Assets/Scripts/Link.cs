using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Tile_Input _point_Start;
    public Tile_Output _point_End;
    LineRenderer lr;
    MeshCollider meshCollider;
    Material material;
    float thickness;

    Vector3[] points;

    void Start()
    {
        points = new Vector3[2];
        lr = gameObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;


        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {
        points[0] = _point_Start._OutputConnector.transform.position;
        points[1] = _point_End._InputConnector.transform.position;
        lr.material = this.material;
        lr.startWidth = this.thickness;
        lr.endWidth = this.thickness;
        lr.SetPositions(points);


        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
    }

    private void OnMouseDown()
    {
        Debug.Log("bo");
    }

    internal void _SetStartEnd(GameObject connector1, GameObject connector2)
    {
        Tile t1 = connector1.transform.parent.GetComponent<Tile>();
        switch (t1._GetType())
        {
            case Tile.TileType.Tile_Input:
                _point_Start = (Tile_Input)t1;
                break;
            case Tile.TileType.Tile_Output:
                _point_End = (Tile_Output)t1;
                break;
            case Tile.TileType.Tile_Process:
                break;
        }

        Tile t2 = connector2.transform.parent.GetComponent<Tile>();
        switch (t2._GetType())
        {
            case Tile.TileType.Tile_Input:
                _point_Start = (Tile_Input)t2;
                break;
            case Tile.TileType.Tile_Output:
                _point_End = (Tile_Output)t2;
                break;
            case Tile.TileType.Tile_Process:
                break;
        }
    }

    internal void _SetMaterial(Material material)
    {
        this.material = material;
    }

    internal void _SetThickness(float thickness)
    {
        this.thickness = thickness;
    }
}