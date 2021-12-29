using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinksManager : MonoBehaviour
{
    public Canvas canvas;
    RectTransform canvas_rect;

    GameObject gameObject_links;
    const string GameObject_links = "gameObject_links";

    public Dictionary<Tile, Dictionary<Tile, Link>> links;

    GameObject gameObject_connector_current;
    GameObject gameObject_current;
    TileInfo.TileType tileType_current;
    Vector2 connector_current_position;

    public float thickness;
    public Material material;
    LineRenderer lr;
    Vector3[] points;

    public static LinksManager Instance { get; internal set; }

    void Start()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;

        canvas_rect = canvas.GetComponent<RectTransform>();
        if (thickness == 0) thickness = 0.05f;
        points = new Vector3[2];

        lr = gameObject.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;

        links = new Dictionary<Tile, Dictionary<Tile, Link>>();
        gameObject_links = transform.Find(GameObject_links)?.gameObject;

        if (gameObject_links == null)
        {
            gameObject_links = new GameObject(GameObject_links);
            gameObject_links.transform.SetParent(transform);
        }
    }

    public void _Click(GameObject Connector)
    {
        if (gameObject_current == null)
        {
            gameObject_connector_current = Connector;
            Tile t = Connector.GetComponentInParent<Tile>();
            gameObject_current = t.gameObject;
            tileType_current = Tile.GetTileType(gameObject_current);
            connector_current_position = Connector.transform.position;
            points[0] = connector_current_position;
            lr.enabled = true;
            lr.material = material;
            lr.startWidth = thickness;
            lr.endWidth = thickness;
        }
        else
        {
            MakeLink(gameObject_connector_current, Connector);

            //reset
            gameObject_connector_current = null;
            gameObject_current = null;
            tileType_current = TileInfo.TileType.None;
            connector_current_position = Vector2.zero;
            lr.enabled = false;
        }
    }

    void Update()
    {
        if (gameObject_current != null)
        {
            //points[1] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //points[1] = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas_rect, Input.mousePosition, Camera.main, out points[1]);
            points[1].z = 0;
            lr.SetPositions(points);
        }
    }

    public void MakeLink(GameObject connecteur1, GameObject connecteur2)
    {
        //le lien existe déjà ?
        Tile t_source = GetSource(connecteur1, connecteur2).GetComponentInParent<Tile>();
        Tile t_listener = GetListener(connecteur1, connecteur2).GetComponentInParent<Tile>();

        t_source._tileInfo.UpdateDATA(t_source);
        t_listener._tileInfo.UpdateDATA(t_listener);

        if (links.ContainsKey(t_source))
        {
            if (links[t_source].ContainsKey(t_listener))
            {
                Debug.Log("already exists");
                return;
            }
        }
        else
        {
            links.Add(t_source, new Dictionary<Tile, Link>());
        }

        GameObject glk = new GameObject();
        glk.transform.SetParent(gameObject_links.transform);
        Link lk = glk.AddComponent<Link>();

        links[t_source].Add(t_listener, lk);

        lk._SetStartEnd(connecteur1, connecteur2);

        lk._SetMaterial(material);
        lk._SetThickness(thickness);

        PrintDico();
    }



    void PrintDico()
    {
        string msg = "links : " + links.Count;
        foreach (var _links in links)
        {
            Dictionary<Tile, Link> linksV = _links.Value;
            msg += "\n" + _links.Key.name + " : " + linksV.Count;

            foreach (var link in linksV)
            {
                Link linkV = link.Value;
                msg += "\n" + link.Key.name + " : " + linkV.ToString();
            }
        }

        Debug.Log(msg);
    }


    public static GameObject GetListener(GameObject c1, GameObject c2)
    {
        if (c1.tag == "Listener")
            return c1;

        if (c2.tag == "Listener")
            return c2;

        Debug.Log("==GetListener ERREUR==" + c1.name + " " + c2.name);
        throw new System.NotImplementedException();
    }

    public static GameObject GetSource(GameObject c1, GameObject c2)
    {
        if (c1.tag == "Source")
            return c1;

        if (c2.tag == "Source")
            return c2;

        Debug.Log("==GetSource ERREUR==" + c1.name + " " + c2.name);
        throw new System.NotImplementedException();
    }

    void OnValidate()
    {
        if (links != null)
        {
            foreach (Dictionary<Tile, Link> _links in links.Values)
            {
                foreach (Link link in _links.Values)
                {
                    link._SetMaterial(material);
                    link._SetThickness(thickness);
                }
            }
        }
    }

    internal void _NewData(Tile tile_output, object data)
    {
        if (links.ContainsKey(tile_output))
            foreach (Link link in links[tile_output].Values)
                link._point_End._NewInput(data);
    }
}