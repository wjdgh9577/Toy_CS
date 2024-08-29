using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemBase : MonoBehaviour
{
    public bool Using { get; set; }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
