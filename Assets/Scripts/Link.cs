using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Tile _point_Start;
    public Tile _point_End;
    public GameObject _connecteur_Start;
    public GameObject _connecteur_End;
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
        points[0] = _connecteur_Start.transform.position;
        points[1] = _connecteur_End.transform.position;
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
        _connecteur_Start = LinksManager.GetSource(connector1, connector2);
        _connecteur_End = LinksManager.GetListener(connector1, connector2);
        _point_Start = _connecteur_Start.transform.parent.GetComponent<Tile>();
        _point_End = _connecteur_End.transform.parent.GetComponent<Tile>();
    }

    internal void _SetMaterial(Material material)
    {
        this.material = material;
    }

    internal void _SetThickness(float thickness)
    {
        this.thickness = thickness;
    }

    public new string ToString()
    {
        string txt ="Link between " +
            _point_Start.name + "[" + _connecteur_Start.name + "] & " +
            _point_End.name + "[" + _connecteur_End.name + "]";
        return txt;
    }
}