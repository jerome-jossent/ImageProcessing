using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    public GameObject canvasWorld;

    public GameObject Menu_Boutons;

    public GameObject pannel1;
    public GameObject pannel2;

    public GameObject prefabBouton;

    List<GameObject> prefabs_in;
    List<GameObject> prefabs_process;
    List<GameObject> prefabs_out;

    void Start()
    {
        Menu_Boutons.SetActive(false);
        pannel1.SetActive(false);
        pannel2.SetActive(false);
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        prefabs_in = new List<GameObject>();
        prefabs_process = new List<GameObject>();
        prefabs_out = new List<GameObject>();

        string prefabFolder = "Prefabs\\Tiles\\";
        GameObject[] prefabs = Resources.LoadAll<GameObject>(prefabFolder);

        foreach (GameObject prefab in prefabs)
        {
            Tile t = prefab.GetComponent<Tile>();
            switch (t.typeGeneric)
            {
                case Tile.TileTypeGeneric.In:
                    prefabs_in.Add(prefab);
                    break;
                case Tile.TileTypeGeneric.Process:
                    prefabs_process.Add(prefab);
                    break;
                case Tile.TileTypeGeneric.Out:
                    prefabs_out.Add(prefab);
                    break;
            }
        }
    }

    public void _Menu_SHOW()
    {
        if (Menu_Boutons.activeSelf)
        {
            //Menu OFF
            pannel1.SetActive(false);
            pannel2.SetActive(false);
            _Menu_RemoveOff();
            Menu_Boutons.SetActive(false);
        }
        else
        {
            Menu_Boutons.SetActive(true);
        }
    }

    public void _Menu_Add() //Display/Hide
    {
        if (pannel1.activeSelf)
        {
            pannel1.SetActive(false);
            pannel2.SetActive(false);
            _Menu_RemoveOff();
        }
        else
        {
            pannel1.SetActive(true);
        }
    }

    public void _Menu_Remove(bool value)
    {
        WorldManager.Instance._removing = value;
    }
    public void _Menu_RemoveOff()
    {
        WorldManager.Instance._removing = false;
    }

    public void _Menu_Add_Sources() //Display
    {
        Menu_Add_Panel2(prefabs_in, WorldManager.Instance.color_in);
    }
    public void _Menu_Add_Process() //Display
    {
        Menu_Add_Panel2(prefabs_process, WorldManager.Instance.color_process);
    }
    public void _Menu_Add_Outs() //Display
    {
        Menu_Add_Panel2(prefabs_out, WorldManager.Instance.color_out);
    }

    void Menu_Add_Panel2(List<GameObject> prefabs_category, Color colorCategory)
    {
        ShowClearPannel2();
        foreach (GameObject prefab in prefabs_category)
        {
            GameObject btn = Instantiate(prefabBouton);
            Image img = btn.GetComponent<Image>();
            img.color = colorCategory;

            Text txt = btn.GetComponentInChildren<Text>();
            txt.text = prefab.GetComponent<Tile>().name;

            btn.transform.SetParent(pannel2.transform, false);
            Button button = btn.GetComponent<Button>();
            button.onClick.AddListener(delegate () { _Add(prefab); });
        }
    }

    void ShowClearPannel2()
    {
        pannel2.SetActive(true);
        while (pannel2.transform.childCount > 0)
            DestroyImmediate(pannel2.transform.GetChild(0).gameObject);
    }

    public void _Add(GameObject prefab)
    {
        Debug.Log(prefab.name);

        GameObject go = Instantiate(prefab, Vector2.zero, Quaternion.identity, canvasWorld.transform);
        string name = GetNewName(prefab.name);
        go.name = name;
    }

    string GetNewName(string racine)
    {
        bool OK = false;
        int i = 1;
        string rep = racine + " " + i;
        while (!OK)
        {
           Transform tr = canvasWorld.transform.Find(rep);
           OK = (tr == null);
            if (tr != null)
                rep = racine + " " + ++i;
        }
        return rep;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
