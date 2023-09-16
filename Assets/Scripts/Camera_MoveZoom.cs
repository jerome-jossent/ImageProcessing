using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera_MoveZoom : MonoBehaviour
{
    #region PARAMETERS
    bool moving;
    Vector3 mouse_position_0;
    public bool zoomInWhenWheelUp;

    public float zoomMin, zoomMax;
    public float xMax, xMin, yMax, yMin;
    #endregion

    #region SINGLETON
    public static Camera_MoveZoom Instance { get; internal set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

    TMP_Dropdown[] dropdowns;


    #region UNITY METHODS
    void Start()
    {
        if (zoomMin == 0) zoomMin = 0.1f;
        if (zoomMax == 0) zoomMax = 100f;

        if (xMax == 0) xMax = 10f;
        if (xMin == 0) xMin = -10f;
        if (yMax == 0) yMax = 5f;
        if (yMin == 0) yMin = -5f;

        dropdowns = GameObject.FindObjectsOfType<TMP_Dropdown>();
    }

    void Update()
    {
        if (WorldManager.Instance.isShowingImageInFullScreen)
            return;

        // click droit vient d'être appuyé
        if (Input.GetMouseButtonDown(1))
        {
            mouse_position_0 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            moving = false;
        }

        // pan
        if (moving)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 deplacement = mouse_position_0 - mouse_position;
            transform.position += deplacement;

            if (transform.position.x > xMax) transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
            if (transform.position.x < xMin) transform.position = new Vector3(xMin, transform.position.y, transform.position.z);
            if (transform.position.y > yMax) transform.position = new Vector3(transform.position.x, yMax, transform.position.z);
            if (transform.position.y < yMin) transform.position = new Vector3(transform.position.x, yMin, transform.position.z);
        }

        // zoom
        //Tester si un menu déroulant est ouvert ?
        bool almost_one_dropdown_is_open = false;
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            if (dropdown.IsExpanded)
            {
                almost_one_dropdown_is_open = true;
                break;
            }
        }

        if (!almost_one_dropdown_is_open)
        {
            float zoom = (zoomInWhenWheelUp) ? -Input.mouseScrollDelta.y : Input.mouseScrollDelta.y;
            if (zoom != 0f)
            {
                Camera.main.orthographicSize += zoom;
                if (Camera.main.orthographicSize > zoomMax) Camera.main.orthographicSize = zoomMax;
                if (Camera.main.orthographicSize < zoomMin) Camera.main.orthographicSize = zoomMin;
            }
        }
    }
    #endregion

    internal void UpdateCameraLimits(Bounds bounds)
    {
        xMax = bounds.center.x / 100 + bounds.size.x / 200;
        xMin = bounds.center.x / 100 - bounds.size.x / 200;
        yMax = bounds.center.y / 100 + bounds.size.y / 200;
        yMin = bounds.center.y / 100 - bounds.size.y / 200;
    }
}