/************************************************************
    File      : PETools
	Author    : Plane
    Version   : 1.0
    Function  : ���ܹ�����
    Date      : 2016/10/20 16:57:44
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class PEUITools 
{

    public static Transform GetTrans(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform childTrans = parent.GetChild(i);
            if (childTrans.name.Equals(name))
            {
                return childTrans;
            }
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform childTrans = parent.GetChild(i);
            Transform getTrans = GetTrans(childTrans, name);
            if (getTrans == null)
            {
                continue;
            }
            else
            {
                return getTrans;
            }
        }
        return null;
    }
    public static T GetOrAddWindowHandle<T>(GameObject gb) where T : WindowBase
    {
        T t = gb.GetComponent<T>();
        if (t == null) t = gb.AddComponent<T>();
        return t;
    }
}