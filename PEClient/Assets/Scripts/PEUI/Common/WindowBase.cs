/************************************************************
    File      : WindowBase
	Author    : Plane
    Version   : 1.0
    Function  : 所有窗口的基类
    Date      : 2016/10/21 16:15:29
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class WindowBase : MonoBehaviour
{
    ///////////////////////////Data Define//////////////////////////////
    public PEWindowEnum curWindowEnum = PEWindowEnum.None;
    public string luaWindowName = "";

    //----------------------------------------------------------------//
    public virtual void InitDic() { }

    public virtual void InitWindow(bool open = true, int state = 0)
    {

    }
}