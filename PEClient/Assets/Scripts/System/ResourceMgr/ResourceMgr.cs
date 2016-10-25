/************************************************************
    File      : ResourceMgr
	Author    : Plane
    Version   : 1.0
    Function  : 资源加载类
    Date      : 2016/10/19 15:40:55
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : MonoBehaviour 
{
	///////////////////////////Data Define//////////////////////////////
    public static Dictionary<ResKey,ResItem> ressDic = new Dictionary<ResKey,ResItem>();

	//----------------------------------------------------------------//
    
    ///////////////////////////MainFunctions////////////////////////////
    public static Object GetInstantiateOB(string resName, ResType resType, ResCacheType cacheType = ResCacheType.Never)
    {
        ResKey reskey = new ResKey(resType, resName);
        ResItem resItem;
        if (!ressDic.ContainsKey(reskey))
        {
            Object ob = LoadObj(resType, resName);
            if (ob != null)
            {
                resItem = new ResItem(ob, cacheType);
                ressDic.Add(reskey, resItem);
            }
            else
            {
                resItem = null;
            }                
        }
        else
        {
            resItem = ressDic[reskey];
        }
        
        if (!ressDic.ContainsKey(reskey))
        {
            Debug.LogError("Load Res:" + reskey.name + " Error!");
            return null;
        }
        return Instantiate(resItem.obj);
    }
    private static Object LoadObj(ResType resType, string resName)
    {
        //游戏包中Resources目录下的路径
        string pathRes = GetResPathByType(resType) + resName;
        //比对更新文件字典中是否存在，取得最新版本资源的路径
        string pathAB = "";//TODO
        pathAB = pathRes;//TODEL
        if(pathRes.Equals(pathAB))
        {
            //从游戏的Resources目录中加载
            return Resources.Load(pathRes);
        }
        else
        {
            //从更新下来的AB文件中读取并实例化
            //TODO
        }
        return null;
    }
    //----------------------------------------------------------------//
    
    ///////////////////////////ToolMethonds/////////////////////////////
    private static string GetResPathByType(ResType rt)
    {
        string path = "";
        switch(rt)
        {
            case ResType.UICommomType:
                path = "PEUI/UICommon/";
                break;
            case ResType.UIMainCityType:
                path = "PEUI/UIMainCity";
                break;
            default:
                Debug.Log("Can not find Res Load Path!");
                break;

        }
        return path;
    }
    //----------------------------------------------------------------//
}

public class ResKey
{
    public ResType type;
    public string name;
    public ResKey(ResType type,string name)
    {
        this.type = type;
        this.name = name;
    }
}
public class ResItem
{
    public Object obj;
    public ResCacheType cacheType;
    public ResItem(Object obj, ResCacheType cacheType)
    {
        this.obj = obj;
        this.cacheType = cacheType;
    }
}
public enum ResCacheType
{
    Never,   //不缓存
    Always,  //缓存
    None
}
public enum ResType
{
    UICommomType,
    UIItemType,
    UIMainCityType,
    UIFightType,

    WeaponType,
    SkillFXType,

    UISoundType,
    BGSoundType,
    SkillSoundType

}