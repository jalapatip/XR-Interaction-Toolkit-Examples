using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller_SystemMenu : MonoBehaviour
{
    //These should all be assigned using inspector from Hierarchy as part of the Prefab
    [TooltipAttribute("Assign using inspector from Hierarchy")]
    public GameObject Menu_None;
    public GameObject Menu_General;
    public GameObject Menu_Screenshot;
    public GameObject Menu_Setting;
    public GameObject Menu_Audio;
    public GameObject Menu_Visual;
    public GameObject Menu_User;
    public GameObject Menu_Credit;

    private IDictionary<XROSMenuTypes, GameObject> _menuDictionary = new Dictionary<XROSMenuTypes, GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        _menuDictionary.Add(XROSMenuTypes.Menu_None, Menu_None);
        _menuDictionary.Add(XROSMenuTypes.Menu_General, Menu_General);
        _menuDictionary.Add(XROSMenuTypes.Menu_Screenshot, Menu_Screenshot);
        _menuDictionary.Add(XROSMenuTypes.Menu_Setting, Menu_Setting);
        _menuDictionary.Add(XROSMenuTypes.Menu_Audio, Menu_Audio);
        _menuDictionary.Add(XROSMenuTypes.Menu_Visual, Menu_Visual);
        _menuDictionary.Add(XROSMenuTypes.Menu_User, Menu_User);
        _menuDictionary.Add(XROSMenuTypes.Menu_Credit, Menu_Credit);
    }

    public void OpenMenu(XROSMenuTypes menuTypes)
    {
        foreach (var item in _menuDictionary)
        {
            if (item.Value)
            {
                item.Value.SetActive(item.Key == menuTypes);
            }
            else
            {
                Dev.LogWarning(item.Key + " does not exist", Dev.LogCategory.UI);
            }
        }
    }
}