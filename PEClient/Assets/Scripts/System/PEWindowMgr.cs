/************************************************************
    File      : PEWindowMgr
	Author    : Plane
    Version   : 1.0
    Function  : 窗口管理
    Date      : 2016/10/19 14:54:35
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWindow
{     
    public PEWindowEnum windowEnum;
    public string windowName = "";
    public PWindow(PEWindowEnum windowEnum, string luaName)
    {
        this.windowEnum = windowEnum;
        if (luaName == "")
        {
            windowName = windowEnum.ToString();
        }
        else
        {
            windowName = luaName;
        }
    }

    //覆写HashCode加快查询速度
    public override int GetHashCode()
    {
        return (int)windowEnum + windowName.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        else
        {
            PWindow pw = obj as PWindow;
            return Equals(pw);
        }
    }
    public bool Equals(PWindow pwindow)
    {
        if (pwindow == null)
        {
            return false;
        }
        else
        {
            return pwindow.windowEnum == windowEnum && pwindow.windowName == windowName;
        }
    }
}

public class PEWindowMgr : MonoBehaviour
{
    ///////////////////////////Data Define//////////////////////////////
    private static PEWindowMgr instance = null;
    public static PEWindowMgr Instance { get { return instance; } }
    private Transform uiRootTrans = null;
    private Transform windowRootTrans = null;
    private Transform cameraRootTrans = null;
    public bool isInitDone = false;
    class PWindowType
    {
        public WindowBase windowBase;
        public ResType resType;
        public ResCacheType cacheType;
        public PWindowType(WindowBase windowBase, ResType resType, ResCacheType cacheType)
        {
            this.windowBase = windowBase;
            this.resType = resType;
            this.cacheType = cacheType;
        }
    }
    private Dictionary<PWindow, PWindowType> windowDic = new Dictionary<PWindow, PWindowType>();
    //----------------------------------------------------------------//

    ///////////////////////////MainFunctions////////////////////////////
    private void Awake()
    {
        instance = this;
        //当有图集进行更新时，通过这里首先去加载
        //TODO
        StartCoroutine(InitWindowMgr());
    }

    IEnumerator InitWindowMgr()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        var uiroot = GameObject.Find("UIRoot");
        if (uiroot != null) Destroy(uiroot);
        GameObject gb = null;               
        gb = (GameObject)ResourceMgr.GetInstantiateOB("UIRoot", ResType.UICommomType, ResCacheType.Always);
        gb.name = "UIRoot";
        uiRootTrans = gb.transform;
        uiRootTrans.parent = transform;
        windowRootTrans = PEUITools.GetTrans(uiRootTrans, "windowRoot");
        cameraRootTrans = PEUITools.GetTrans(uiRootTrans, "cameraRoot");        
        isInitDone = true;
    }
    //----------------------------------------------------------------//

    ///////////////////////////WindowControl////////////////////////////

    //控制UI窗口的状态 open:窗口是否打开，state:状态控制参数，两者都传递到WindowBase的InitWindow()中去。
    public void SetWindowState(PEWindowEnum windowEnum, ResType resType, string luaName = "", bool open = true, int state = 0)
    {
        PWindow pwindow = new PWindow(windowEnum, luaName);
        if (!windowDic.ContainsKey(pwindow))
        {
            InitWindowCache(windowEnum, resType, luaName);
        }        
        var wb = windowDic[pwindow].windowBase;
        wb.curWindowEnum = windowEnum;
        wb.luaWindowName = luaName;
        wb.InitWindow(open, state);        
    }
    //预加载窗口物体进缓存
    public void InitWindowCache(PEWindowEnum windowEnum, ResType resType, string luaName = "", ResCacheType cacheType = ResCacheType.Never)
    {        
        PWindow pwindow = new PWindow(windowEnum, luaName);
        string windowName = pwindow.windowName;
        GameObject gb = null;
        if (!windowDic.ContainsKey(pwindow))
        {
            gb = (GameObject)ResourceMgr.GetInstantiateOB(windowName, resType, cacheType);
            gb.name = windowName;
            gb.transform.parent = windowRootTrans;
            gb.transform.localPosition = Vector3.zero;
            gb.transform.localScale = Vector3.one;
            NGUITools.SetActive(gb, false);
            WindowBase windowBase = gb.GetComponent<WindowBase>();
            if (windowBase == null)
            {
                windowBase = GetOrAddWindowHandle(gb, windowEnum, luaName);
                if (windowBase == null)
                {
                    Debug.LogError("Can not Get " + windowName + " Handle Scripts");
                    return;
                }
            }
            windowDic.Add(pwindow, new PWindowType(windowBase, resType, cacheType));
            windowBase.InitDic();
        }
    }
    //----------------------------------------------------------------//

    ///////////////////////////ToolMethonds/////////////////////////////
    //获取或添加窗口的根控制脚本
    private WindowBase GetOrAddWindowHandle(GameObject gb, PEWindowEnum windowEnum, string luaName)
    {
        WindowBase windowBase = null;
        switch (windowEnum)
        {
            case PEWindowEnum.Login_window:
                windowBase = PEUITools.GetOrAddWindowHandle<HandleLoginWindow>(gb);
                break;
            case PEWindowEnum.Logined_window:
                windowBase = PEUITools.GetOrAddWindowHandle<HandleLoginedWindow>(gb);
                break;
            default:
                break;
        }
        return windowBase;
    }
    //----------------------------------------------------------------//
}