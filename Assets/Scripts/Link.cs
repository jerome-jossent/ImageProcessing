using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    #region PARAMETERS
    public LinkInfo _linkInfo;

    public Tile _point_Start;
    public Tile _point_End;
    public GameObject _connecteur_Start;
    public GameObject _connecteur_End;
    LineRenderer lr;
    MeshCollider meshCollider;
    Material material;
    float thickness;

    Vector3[] points;
    #endregion

    #region UNITY METHODS
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
        Debug.Log("TODO");
    }
    #endregion

    #region SET PARAMETERS
    internal void _SetStartEnd(GameObject connector1, GameObject connector2)
    {
        _connecteur_Start = LinksManager.GetSource(connector1, connector2);
        _connecteur_End = LinksManager.GetListener(connector1, connector2);
        _point_Start = _connecteur_Start.GetComponentInParent<Tile>();
        _point_End = _connecteur_End.GetComponentInParent<Tile>();

        _linkInfo = new LinkInfo(_connecteur_Start, _connecteur_End);
    }

    internal void _SetMaterial(Material material)
    {
        this.material = material;
    }

    internal void _SetThickness(float thickness)
    {
        this.thickness = thickness;
    }
    #endregion

    public new string ToString()
    {
        string txt = "Link between " +
            _point_Start.name + "[" + _connecteur_Start.name + "] & " +
            _point_End.name + "[" + _connecteur_End.name + "]";
        return txt;
    }
}

public class LinkInfo
{
    public string tileInfoNameStart { get; set; }
    public string connectorNameStart { get; set; }
    public string tileInfoNameEnd { get; set; }
    public string connectorNameEnd { get; set; }

    public LinkInfo()
    { }

    public LinkInfo(GameObject connecteur_Start, GameObject connecteur_End)
    {
        tileInfoNameStart = connecteur_Start.GetComponentInParent<Tile>()._tileInfo.name;
        tileInfoNameEnd = connecteur_End.GetComponentInParent<Tile>()._tileInfo.name;
        connectorNameStart = connecteur_Start.name;
        connectorNameEnd = connecteur_End.name;
    }
}