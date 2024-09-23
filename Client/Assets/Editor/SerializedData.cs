using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public interface IData
{

}

public class SerializedData<T> where T : IData
{
    public List<T> datas = new List<T>();
}

#endif
