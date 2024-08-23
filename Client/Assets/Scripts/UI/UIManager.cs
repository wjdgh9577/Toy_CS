using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    const string UI_RESOURCES_PATH = "Prefabs/UI";

    Dictionary<Type, UIBase> _uiPool = new Dictionary<Type, UIBase>();

    public T GetUI<T>() where T : UIBase
    {
        if (_uiPool.TryGetValue(typeof(T), out UIBase ui) == false)
        {
            var uiObj = Resources.Load($"{UI_RESOURCES_PATH}/{typeof(T).Name}");
            ui = (Instantiate(uiObj, transform) as GameObject).GetComponent<T>();
            _uiPool.Add(typeof(T), ui);
        }

        return ui as T;
    }

    public void RemoveUI<T>() where T : UIBase
    {
        if (_uiPool.Remove(typeof(T), out UIBase ui))
            Destroy(ui);
    }
}
