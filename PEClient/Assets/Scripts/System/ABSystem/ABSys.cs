/************************************************************
    File      : ABSys
	Author    : Plane
    Version   : 1.0
    Function  : AssetBundle管理系统
    Date      : 2016/10/20 15:7:12
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class ABSys : ISystem 
{
    ///////////////////////////Data Define//////////////////////////////
    public static ABSys Instance = null;
    //----------------------------------------------------------------//

    ///////////////////////////MainFunctions////////////////////////////
    public override void Init()
    {
        Instance = this;
    }
    //----------------------------------------------------------------//
	
}