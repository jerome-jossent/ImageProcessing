using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_MoveZoom : MonoBehaviour
{
    bool moving;
    Vector3 mouse_position_0;

    void Update()
    {
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

            float Xp = 10;
            float Xm = -10;
            float Yp = 5;
            float Ym = -5;

            if (transform.position.x > Xp) transform.position = new Vector3(Xp, transform.position.y, transform.position.z);
            if (transform.position.x < Xm) transform.position = new Vector3(Xm, transform.position.y, transform.position.z);
            if (transform.position.y > Yp) transform.position = new Vector3(transform.position.x, Yp, transform.position.z);
            if (transform.position.y < Ym) transform.position = new Vector3(transform.position.x, Ym, transform.position.z);
        }

        // zoom
        float zoom = Input.mouseScrollDelta.y;
        if (zoom != 0f)
        {
            float Zp = 100;
            float Zm = 1;
            Camera.main.orthographicSize += Input.mouseScrollDelta.y;
            if (Camera.main.orthographicSize > Zp) Camera.main.orthographicSize = Zp;
            if (Camera.main.orthographicSize < Zm) Camera.main.orthographicSize = Zm;
        }
    }
}