/************************************************************
    File      : LoginState
	Author    : Plane
    Version   : 1.0
    Function  : 游戏登录状态
    Date      : 2016/10/21 11:48:53
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using System.Collections;
using UnityEngine;

public class LoginState : IGameState 
{
    ///////////////////////////MainFunctions////////////////////////////
    
    public override void Enter()
    {
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        //设置标志变量，确保ABSys中的AB映射字典已经构建完成，可以开始加载UI等资源后才能去打开登录窗口
        //TODO
        bool isDone = true;
        while (!isDone)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //加载等待，错误，tips提示等通用窗口。
        //TODO

        mProgressValue = 0;
        //显示登录界面，预加载登录完成界面
        PEWindowMgr.Instance.SetWindowState(PEWindowEnum.Login_window, ResType.UICommomType);
        //缓存登录成功界面
        //PEWindowMgr.Instance.InitWindowCache(PEWindowEnum.Logined_window, ResType.UICommomType);
        mProgressValue = 1;
    }
    //----------------------------------------------------------------//

    ///////////////////////////ToolMethonds/////////////////////////////    
    public override GameStateType GetStateType()
    {
        return GameStateType.LoginState;
    }
    //----------------------------------------------------------------//
}