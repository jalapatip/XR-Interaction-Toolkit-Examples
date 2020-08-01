using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Enum_XROSUI_Color
{
    OnGrab, OnHover, Default
}

public class Controller_UIEffects : MonoBehaviour
{
    private static readonly Color Color_OnHover = new Color(0.929f, 0.094f, 0.278f);
    private static readonly Color Color_OnGrab = new Color(0.019f, 0.733f, 0.827f);

    public Dictionary<string, Color> ColorDictionary = new Dictionary<string, Color>();
    public Dictionary<Enum_XROSUI_Color, Color> ColorDictionary2 = new Dictionary<Enum_XROSUI_Color, Color>();

    public Color[] ColorList;
    // Start is called before the first frame update
    void Start()
    {
        ColorDictionary.Add(Enum_XROSUI_Color.OnHover.ToString(), Color_OnHover);
        ColorDictionary.Add(Enum_XROSUI_Color.OnGrab.ToString(), Color_OnGrab);
        ColorDictionary.Add(Enum_XROSUI_Color.Default.ToString(), Color.white);

        ColorDictionary2.Add(Enum_XROSUI_Color.OnHover, Color_OnHover);
        ColorDictionary2.Add(Enum_XROSUI_Color.OnGrab, Color_OnGrab);
        ColorDictionary2.Add(Enum_XROSUI_Color.Default, Color.white);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public Color GetColor(Enum_XROSUI_Color colorName)
    {
        //Color c = colorDictionary[colorName.ToString()];
        // var c = colorDictionary2[colorName];
        //return ColorDictionary[colorName.ToString()];
        return ColorDictionary2[colorName];
    }
}
