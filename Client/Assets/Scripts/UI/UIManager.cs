using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Dictionary<Type, UIBase> _uiPool = new Dictionary<Type, UIBase>();
    Dictionary<Type, List<UIItemBase>> _uiItemPool = new Dictionary<Type, List<UIItemBase>>();

    public T GetUI<T>() where T : UIBase
    {
        if (_uiPool.TryGetValue(typeof(T), out UIBase ui) == false)
        {
            var obj = Resources.Load($"{Config.UI_RESOURCES_PATH}/{typeof(T).Name}");
            ui = (Instantiate(obj, transform) as GameObject).GetComponent<T>();
            _uiPool.Add(typeof(T), ui);
        }

        return ui as T;
    }

    public T GetUIItem<T>(Transform root) where T : UIItemBase
    {
        var list = _uiItemPool.Where(p => p.Key == typeof(T)).Select(p => p.Value).FirstOrDefault();
        if (list == default)
        {
            list = new List<UIItemBase>();
            _uiItemPool.Add(typeof(T), list);
        }

        var item = list.Where(i => i.Using == false).FirstOrDefault() as T;
        if (item == default)
        {
            var obj = Resources.Load<GameObject>($"{Config.UI_ITEM_RESOURCES_PATH}/{typeof(T).Name}");
            item = (Instantiate(obj)).GetComponent<T>();
            list.Add(item);
        }

        item.Using = true;
        item.transform.SetParent(root);

        return (T)item;
    }

    public List<T> GetUIItems<T>(int count, Transform root) where T : UIItemBase
    {
        List<T> list = new List<T>();

        if (_uiItemPool.TryGetValue(typeof(T), out List<UIItemBase> uiList) == false)
        {
            uiList = new List<UIItemBase>();
            _uiItemPool.Add(typeof(T), uiList);
        }

        var obj = Resources.Load<GameObject>($"{Config.UI_ITEM_RESOURCES_PATH}/{typeof(T).Name}");
        while (uiList.Count < count)
        {
            var item = (Instantiate(obj)).GetComponent<T>();
            item.Using = true;
            uiList.Add(item);
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
