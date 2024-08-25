using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    const string UI_RESOURCES_PATH = "Prefabs/UI";
    const string UI_ITEM_RESOURCES_PATH = "Prefabs/UI/UIItem";

    Dictionary<Type, UIBase> _uiPool = new Dictionary<Type, UIBase>();
    Dictionary<Type, List<UIItemBase>> _uiItemPool = new Dictionary<Type, List<UIItemBase>>();

    public T GetUI<T>() where T : UIBase
    {
        if (_uiPool.TryGetValue(typeof(T), out UIBase ui) == false)
        {
            var obj = Resources.Load($"{UI_RESOURCES_PATH}/{typeof(T).Name}");
            ui = (Instantiate(obj, transform) as GameObject).GetComponent<T>();
            _uiPool.Add(typeof(T), ui);
        }

        return ui as T;
    }

    public List<T> GetUIItems<T>(int count, Transform root) where T : UIItemBase
    {
        List<T> list = new List<T>();

        if (_uiItemPool.TryGetValue(typeof(T), out List<UIItemBase> uiList) == false)
        {
            uiList = new List<UIItemBase>();
            _uiItemPool.Add(typeof(T), uiList);
        }

        var obj = Resources.Load<GameObject>($"{UI_ITEM_RESOURCES_PATH}/{typeof(T).Name}");
        while (uiList.Count < count)
        {
            uiList.Add((Instantiate(obj)).GetComponent<T>());
        }

        for (int i = 0; i < count; i++)
        {
            uiList[i].transform.SetParent(root);
            list.Add(uiList[i] as T);
        }

        return list;
    }

    public void RemoveUI<T>() where T : UIBase
    {
        if (_uiPool.Remove(typeof(T), out UIBase ui))
            Destroy(ui);
    }
}
