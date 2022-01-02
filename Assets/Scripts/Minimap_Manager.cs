using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Manager : MonoBehaviour
{
    #region PARAMETERS
    public GameObject canvasWorld;
    public Camera cameraMinimap;
    public Camera cameraMain;

    bool oneTileHasChanged;
    #endregion

    #region SINGLETON
    public static Minimap_Manager Instance { get; internal set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

    #region UNITY METHODS
    void Start()
    {
        oneTileHasChanged = true;
    }

    void Update()
    {
        if (oneTileHasChanged)
        {
            oneTileHasChanged = false;
            Bounds bounds = GetLocalBoundsForObject(canvasWorld);

            cameraMinimap.transform.position = new Vector3(bounds.center.x / 100, bounds.center.y / 100, cameraMinimap.transform.position.z);

            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = bounds.size.x / bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                cameraMinimap.orthographicSize = bounds.size.y / 200;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                cameraMinimap.orthographicSize = bounds.size.y / 200 * differenceInSize;
            }

            Camera_MoveZoom.Instance.UpdateCameraLimits(bounds);
        }
    }
    #endregion

    public void _OneTileHasChanged()
    {
        oneTileHasChanged = true;
    }

    static Bounds GetLocalBoundsForObject(GameObject go)
    {
        var referenceTransform = go.transform;
        var b = new Bounds(Vector2.zero, Vector2.zero);
        RecurseEncapsulate(referenceTransform, ref b);
        return b;

        void RecurseEncapsulate(Transform child, ref Bounds bounds)
        {
            BoxCollider2D boxCollider2D_child = child.GetComponent<BoxCollider2D>();
            if (boxCollider2D_child)
            {
                Bounds lsBounds = boxCollider2D_child.bounds;
                var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents * 100);
                var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents * 100);
                bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMin));
                bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMax));
            }
            foreach (Transform grandChild in child.transform)
            {
                RecurseEncapsulate(grandChild, ref bounds);
            }
        }
    }
}
