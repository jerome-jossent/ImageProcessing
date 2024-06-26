using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile : MonoBehaviour
{
    public enum varType { _string, _int, _float, _double }
    public enum TileTypeGeneric { None, In, Process, Out }

    #region PARAMETERS
    [JsonConverter(typeof(StringEnumConverter))]
    public TileTypeGeneric typeGeneric;

    public TileInfo _tileInfo;

    public GameObject[] _OutputConnectors;
    public GameObject[] _InputConnector;

    public abstract void _NewInput(object input);
    public abstract void _NewOutput(object output);

    public TMPro.TMP_Text titre_TMP_Text;
    public RawImage titre_fond;
    public Image tuile_fond;

    bool moving;
    Vector2 position_0;
    Vector3 mouse_position_0;
    BoxCollider2D boxCollider2D;

    RectTransform rectTransform;
    bool activated = true;
    #endregion

    #region STATIC METHODS
    internal static TileInfo.TileType GetTileType(GameObject connector)
    {
        Tile t = connector.GetComponent<Tile>();
        return t._tileInfo.type;
    }
    #endregion

    #region UNITY METHODS
    public void Start()
    {
        moving = false;
        if (rectTransform == null)
            rectTransform = gameObject.GetComponent<RectTransform>();

        GameObject goFond = transform.Find("Fond").gameObject;
        tuile_fond = goFond.GetComponent<Image>();
        tuile_fond.color = WorldManager.Instance.tuile_fond_Couleur_Init;

        GameObject goContenant = goFond.transform.Find("Contenant").gameObject;
        GameObject goTitre = goContenant.transform.Find("Titre").gameObject;
        GameObject goTitreFond = goTitre.transform.Find("Fond").gameObject;
        titre_fond = goTitreFond.GetComponent<RawImage>();
        GameObject goNom = goTitreFond.transform.Find("Nom").gameObject;
        titre_TMP_Text = goNom.GetComponent<TMPro.TMP_Text>();

        titre_TMP_Text.text = gameObject.name; //nom attribu� dans Menu_Manager.Add()

        tuile_fond.color = WorldManager.Instance.tuile_fond_Couleur_Init;

        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.offset = new Vector2(0, 0);
        boxCollider2D.size = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void OnMouseDown()
    {
        if (WorldManager.Instance._removing)
        {

        }
        else
        {
            moving = true;
            position_0 = transform.position;
            mouse_position_0 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnMouseUp()
    {
        if (WorldManager.Instance._removing)
        {
            //destroy links
            LinksManager.Instance.DestroyAllLinksWith(this);

            //destroy tile
            Destroy(gameObject);
            Minimap_Manager.Instance._OneTileHasChanged();
        }
        moving = false;
    }

    public void OnMouseEnter()
    {
        if (WorldManager.Instance._removing)
            tuile_fond.color = WorldManager.Instance.tuile_fond_Couleur_Delete;
        else
            tuile_fond.color = WorldManager.Instance.tuile_fond_Couleur_Selected;
    }

    public void OnMouseExit()
    {
        tuile_fond.color = WorldManager.Instance.tuile_fond_Couleur_Init;
    }

    //public void OnMouseOver()
    //{
        
    //}

    public void Update()
    {
        if (activated)
        {
            ActiveConnectorsLink();
            activated = false;
        }

        if (moving)
        {
            Vector2 deplacement = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouse_position_0;
            transform.position = position_0 + deplacement;
            //save position to object
            if (_tileInfo == null)
                _tileInfo = new TileInfo(this);

            _tileInfo.local_position = transform.localPosition;
            _tileInfo.size = rectTransform.sizeDelta;

            Minimap_Manager.Instance._OneTileHasChanged();
        }
    }
    #endregion
    
    public void _SetMovingOff()//pour que les sliders ne soient pas g�n�s
    {
        moving = false;
    }
    
    public void _Init(TileInfo tileInfo)
    {
        Start();
        _tileInfo = tileInfo;
        titre_TMP_Text.text = _tileInfo.name;
        titre_fond.color = _tileInfo.title_color.GetColor();
        rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = _tileInfo.size;

        transform.localScale = Vector3.one;
        transform.localPosition = _tileInfo.local_position;
    }

    void ActiveConnectorsLink()
    {
        //Debug.Log("ActiveConnectorsLink() " + name);
        //Find GO Connectors
        GameObject GOConnectors = transform.Find("Fond/Connectors").gameObject;
        //In Childs "Outs" & "Ins" make an event in the Button component with linksManager._Click( with itself as GameObject)

        GameObject GOConnectors_Outs = GOConnectors.transform.Find("Outs").gameObject;
        for (int i = 0; i < GOConnectors_Outs.transform.childCount; i++)
        {
            GameObject GOOut = GOConnectors_Outs.transform.GetChild(i).gameObject;
            Button button = GOOut.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            button.onClick = new Button.ButtonClickedEvent();
            // /!\ Non visible dans l'inspecteur !!!!!!!!!
            button.onClick.AddListener(delegate () { LinksManager.Instance._Click(GOOut); });
        }

        GameObject GOConnectors_Ins = GOConnectors.transform.Find("Ins").gameObject;
        for (int i = 0; i < GOConnectors_Ins.transform.childCount; i++)
        {
            GameObject GOIn = GOConnectors_Ins.transform.GetChild(i).gameObject;
            Button button = GOIn.GetComponent<Button>();
            //button.onClick.RemoveAllListeners();
            button.onClick = new Button.ButtonClickedEvent();
            // /!\ Non visible dans l'inspecteur !!!!!!!!!
            button.onClick.AddListener(delegate () { LinksManager.Instance._Click(GOIn); });
        }
    }

    public string Get(string clef, varType varType)
    {
        if (_tileInfo.others.ContainsKey(clef))
            return _tileInfo.others[clef];

        switch (varType)
        {
            case varType._string:
                return "";
            case varType._int:
            case varType._float:
            case varType._double:
            default:
                return "0";
        }
    }
    
    public void Set(string clef, string value)
    {
        if (!_tileInfo.others.ContainsKey(clef))
            _tileInfo.others.Add(clef, value);
        else
            _tileInfo.others[clef] = value;
    }
}

public class TileInfo
{
    public enum TileType
    {
        None,
        FileImage,
        ImageViewer,
        FolderImages,
        MQTTImage,
        ToGray,
        EdgesDetection,
        SaveToDisk,
        ImageThresholding,
        Resize,
    }
    #region PARAMETERS
    [JsonConverter(typeof(StringEnumConverter))]
    public TileType type { get; set; }
    public string name { get; set; }
    public SerializableColor title_color { get; set; }
    public Vector2 local_position { get; set; }
    public Vector2 size { get; set; }

    public Dictionary<string, string> others { get; set; }

    [JsonIgnoreAttribute]
    Tile tile;
    #endregion

    #region CONSTRUCTORS
    public TileInfo()
    {
        if (others == null)
            others = new Dictionary<string, string>();
    }

    public TileInfo(Tile tile)
    {
        this.tile = tile;
        if (others == null)
            others = new Dictionary<string, string>();
    }
    #endregion

    public void UpdateDATA(Tile tile)
    {
        if (this.tile == null)
            this.tile = tile;
        name = tile.titre_TMP_Text.text;
        title_color = new SerializableColor(tile.titre_fond.color);
    }
}

#region PARAMETERS
#endregion

#region SINGLETON
#endregion

#region UNITY METHODS
#endregion

#region SET PARAMETERS
#endregion

#region UI
#endregion

#region INPUT_OUTPUT
#endregion