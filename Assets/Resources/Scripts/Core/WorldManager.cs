using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    #region PARAMETERS
    public Color color_in;
    public Color color_process;
    public Color color_out;

    public Color tuile_fond_Couleur_Init;
    public Color tuile_fond_Couleur_Selected;
    public Color tuile_fond_Couleur_Delete;
    internal bool _removing = false;

    public GameObject ImageViewerMax;
    RawImage ImageViewerMaxRawImage;
    RectTransform rectTransform;
    public bool isShowingImageInFullScreen;
    #endregion

    #region SINGLETON
    public static WorldManager Instance { get; internal set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

    #region UNITY METHODS
    public void Start()
    {
        ImageViewerMaxRawImage = ImageViewerMax.GetComponent<RawImage>();
        ImageViewerMax.SetActive(false);
        rectTransform = ImageViewerMax.GetComponent<RectTransform>();
    }
    public void Update()
    {
        if (isShowingImageInFullScreen && Input.GetMouseButtonDown(0))
            _ImageViewerMax_Hide();
    }
    #endregion

    #region Affichage image fullscreen
    public void _ImageViewerMax_Show(Texture2D texture2D)
    {
        isShowingImageInFullScreen = true;
        ImageViewerMaxRawImage.texture = texture2D;

        //AutoSizeMax
        float sc_w = Screen.width;
        int sc_h = Screen.height;
        float sc_r = sc_w / sc_h;
        float im_r = (float)texture2D.width / texture2D.height;

        float w, h;
        if (im_r > sc_r) //largeur image > largeur écran
        {
            w = sc_w;
            h = w / im_r;
        }
        else
        {
            h = sc_h;
            w = h * im_r;
        }
        rectTransform.sizeDelta = new Vector2(w, h);

        ImageViewerMax.SetActive(true);
    }
    public void _ImageViewerMax_Hide()
    {
        isShowingImageInFullScreen = false;
        ImageViewerMax.SetActive(false);
    }
    #endregion
}