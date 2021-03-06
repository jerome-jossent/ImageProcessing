using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parameter : MonoBehaviour
{
    public TMPro.TMP_Text label_nom;
    public TMPro.TMP_Text label_val;
    public UnityEngine.UI.Slider slider;
    public string description;

    public void _ShowValueFromSlider()
    {
        label_val.text = slider.value.ToString();
    }

    public void _Set(string newLabel, float valmin, float valmax, float valinit, string description)
    {
        label_nom.text = newLabel;
        slider.wholeNumbers = false;
        slider.minValue = valmin;
        slider.maxValue = valmax;
        slider.value = valinit;
        this.description = description;
    }
    public void _Set(string newLabel, int valmin, int valmax, int valinit, string description)
    {
        label_nom.text = newLabel;
        slider.wholeNumbers = true;
        slider.minValue = valmin;
        slider.maxValue = valmax;
        slider.value = valinit;
        this.description = description;
    }
}