using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesMenu_Manager : MonoBehaviour
{
    public GameObject pannel1;
    public GameObject pannel2;

    public GameObject prefabBouton;

    List<GameObject> prefabs_in;
    List<GameObject> prefabs_process;
    List<GameObject> prefabs_out;

    public Color color_in;
    public Color color_process;
    public Color color_out;

    void Start()
    {
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

    public void _Menu_Add()
    {
        if (pannel1.activeSelf)
        {
            pannel1.SetActive(false);
            pannel2.SetActive(false);

        }
        else
        {
            pannel1.SetActive(!pannel1.activeSelf);
        }
    }

    public void _Menu_Add_Sources()
    {
        ShowClearPannel2();
        foreach (GameObject prefab in prefabs_in)
        {
            GameObject btn = Instantiate(prefabBouton);
            Image img = btn.GetComponent<Image>();
            img.color = color_in;

            Text txt = btn.GetComponentInChildren<Text>();
            txt.text = prefab.GetComponent<Tile>().name;

            btn.transform.SetParent(pannel2.transform, false);
        }
    }
    public void _Menu_Add_Process()
    {
        ShowClearPannel2();
        foreach (GameObject prefab in prefabs_process)
        {
            GameObject btn = Instantiate(prefabBouton);
            Image img = btn.GetComponent<Image>();
            img.color = color_process;

            Text txt = btn.GetComponentInChildren<Text>();
            txt.text = prefab.GetComponent<Tile>().name;

            btn.transform.SetParent(pannel2.transform, false);
        }
    }
    public void _Menu_Add_Outs()
    {
        ShowClearPannel2();
        foreach (GameObject prefab in prefabs_out)
        {
            GameObject btn = Instantiate(prefabBouton);
            Image img = btn.GetComponent<Image>();
            img.color = color_out;

            Text txt = btn.GetComponentInChildren<Text>();
            txt.text = prefab.GetComponent<Tile>().name;

            btn.transform.SetParent(pannel2.transform, false);
        }
    }

    void ShowClearPannel2()
    {
        pannel2.SetActive(true);
        while (pannel2.transform.childCount > 0)
            DestroyImmediate(pannel2.transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
