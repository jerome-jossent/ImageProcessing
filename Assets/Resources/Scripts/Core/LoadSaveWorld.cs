using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveWorld : MonoBehaviour
{
    #region PARAMETERS
    public LinksManager linksManager;
    public Canvas canvasWorld;

    public string txt;
    string fileJson = @"C:\_Saves\ImageProcessing\save.json";
    #endregion

    public void _Load()
    {
        _ClearAllChilds();
        if (System.IO.File.Exists(fileJson))
        {
            txt = System.IO.File.ReadAllText(fileJson);
            WorldDATA wd = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldDATA>(txt);

            //TILES
            for (int i = 0; i < wd.tilesInfo.Count; i++)
            {
                TileInfo ti = wd.tilesInfo[i];
                string prefname = "Prefabs\\Tiles\\" + ti.type.ToString();
                //Debug.Log(prefname);
                GameObject prefab = Resources.Load(prefname, typeof(GameObject)) as GameObject;
                GameObject go = Instantiate(prefab);
                go.name = ti.name;
                go.transform.SetParent(canvasWorld.transform);
                Tile t = null;

                if (ti.type==TileInfo.TileType.None)
                {
                    Debug.Log("erreur loading " + ti.name);
                }
                else
                {
                    //Chargement du script attendu
                    var SpecificTile = go.GetComponent(ti.type.ToString());
                    Tile tile = (Tile)SpecificTile;
                    tile._Init(wd.tilesInfo[i]);
                    t = tile;
                }

                if (t != null && !linksManager.links.ContainsKey(t))
                    linksManager.links.Add(t, new Dictionary<Tile, Link>());
            }

            //LINKS
            for (int i = 0; i < wd.linksInfo.Count; i++)
            {
                LinkInfo li = wd.linksInfo[i];
                //find GameObject Connectors
                GameObject connector1 = FindChildOrGrandChild(canvasWorld.transform.Find(li.tileInfoNameStart).gameObject, li.connectorNameStart);
                GameObject connector2 = FindChildOrGrandChild(canvasWorld.transform.Find(li.tileInfoNameEnd).gameObject, li.connectorNameEnd);
                //make the link
                linksManager.MakeLink(connector1, connector2);
            }
        }
    }

    void _ClearAllChilds()
    {
        while (canvasWorld.transform.childCount > 0)
            DestroyImmediate(canvasWorld.transform.GetChild(0).gameObject);
        linksManager.links = new Dictionary<Tile, Dictionary<Tile, Link>>();
    }

    GameObject FindChildOrGrandChild(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            if (child.name == name) return child;

            GameObject result = FindChildOrGrandChild(child, name);
            if (result != null) return result;
        }
        return null;
    }

    public void _Save()
    {
        WorldDATA wd = new WorldDATA();

        for (int i = 0; i < canvasWorld.transform.childCount; i++)
        {
            GameObject child = canvasWorld.transform.GetChild(i).gameObject;
            Tile t = child.GetComponent<Tile>();
            TileInfo ti = t._tileInfo;
            wd.tilesInfo.Add(ti);
            ti.UpdateDATA(t);
        }

        foreach (Dictionary<Tile, Link> tile in linksManager.links.Values)
            foreach (Link link in tile.Values)
                wd.linksInfo.Add(link._linkInfo);

        txt = Newtonsoft.Json.JsonConvert.SerializeObject(wd, Newtonsoft.Json.Formatting.Indented);

        //créé le dossier si besoin
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileJson));
        System.IO.File.WriteAllText(fileJson, txt);
    }
}

public class WorldDATA
{
    public List<TileInfo> tilesInfo { get; set; }
    public List<LinkInfo> linksInfo { get; set; }
    public WorldDATA()
    {
        tilesInfo = new List<TileInfo>();
        linksInfo = new List<LinkInfo>();
    }
}