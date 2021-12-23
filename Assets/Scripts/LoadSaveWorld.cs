using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveWorld : MonoBehaviour
{
    public LinksManager linksManager;
    public Canvas canvasWorld;

    public string txt;

    public bool save, load;
    private void Update()
    {
        if (save)
        {
            save = false;
            _Save();
        }
        if (load)
        {
            load = false;
            _Load();
        }
    }

    public void _Load()
    {

    }

    public void _Save()
    {
        txt = "";
        for (int i = 0; i < canvasWorld.transform.childCount; i++)
        {
            GameObject child = canvasWorld.transform.GetChild(i).gameObject;
            Tile tile = child.GetComponent<Tile>();
            //string data = JsonUtility.ToJson(tile, true);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(tile, Newtonsoft.Json.Formatting.Indented);
            txt += data + "\n";
        }



    }

}
