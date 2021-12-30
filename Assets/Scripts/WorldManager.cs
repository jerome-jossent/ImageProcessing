using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Color color_in;
    public Color color_process;
    public Color color_out;

    public Color tuile_fond_Couleur_Init;
    public Color tuile_fond_Couleur_Selected;
    public Color tuile_fond_Couleur_Delete;
    internal bool _removing = false;

    public static WorldManager Instance { get; internal set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

}
