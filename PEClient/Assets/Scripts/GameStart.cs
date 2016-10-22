/************************************************************
    File      : GameStart
	Author    : Plane
    Version   : 1.0
    Function  : Nothing
    Date      : 2016/10/19 10:24:11
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class GameStart : MonoBehaviour
{
    ///////////////////////////Data Define//////////////////////////////

    //----------------------------------------------------------------//

    ///////////////////////////MainFunctions////////////////////////////
    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        //todo 多平台宏定义
        StartWithEditor();
    }
    //----------------------------------------------------------------//

    ///////////////////////////ToolMethonds/////////////////////////////    
    private void StartWithEditor()
    {
        gameObject.AddComponent("GameRoot");
        gameObject.AddComponent("PEWindowMgr");
    }
    private void StartWithAndroid()
    {

    }
    private void StartWithiOS()
    {

    }
    //----------------------------------------------------------------//
}