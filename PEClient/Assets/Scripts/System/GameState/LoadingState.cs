/************************************************************
    File      : LoadingState
	Author    : Plane
    Version   : 1.0
    Function  : 游戏加载状态，通过其完成各个状态的切换
    Date      : 2016/10/19 17:24:4
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using System.Collections;
using UnityEngine;

public class LoadingState : IGameState 
{
    public static bool mLoadSceneDone = false;
    private string mSceneName = "";
    
    ///////////////////////////MainFunctions////////////////////////////
    public override void Enter()
    {
        GameStateType dstState = GameRoot.Instance.GetDstGameState();
        switch(dstState)
        {
            case GameStateType.LoginState:
                mSceneName = "Login";
                break;
            case GameStateType.MainCityState:
                mSceneName = "MainCity";

                break;
            default:
                break;
        }

        StartCoroutine(AsynLoadScene());
    }

    private IEnumerator AsynLoadScene()
    {
        //对LoginState进行相关处理，加载LoginState时不需要Loading界面
        //TODO

        //当目标不是LoginState并且Loading窗口没有显示出来的情况下让程序一直挂在这里等待
        while (GameRoot.Instance.GetDstGameState() != GameStateType.LoginState)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        AsyncOperation mAsync = Application.LoadLevelAsync(mSceneName);

        //当开始进入新的场景时要完成上一个场景的垃圾回收处理
        GameRoot.Instance.ClearLastLeaveState();
        
        //更新Loading界面的加载进度条,整个加载过程程序一直挂在这里
        while(!mAsync.isDone)
        {
            //更新加载数值
            mProgressValue = mAsync.progress;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //检测GameState结点是否已经创建了，在启动进入游戏后这里将会运行一次。
        if (!GameRoot.isStateGBCreateDone)
        {
            //Create GameStates
            GameRoot.Instance.CreateAllGameStates();
            //程序挂起，等待所有GameState创建并挂载完成
            while(!GameRoot.isStateGBCreateDone)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Debug.Log("Create StateGB Done!");            
        }
        mLoadSceneDone = true;//新场景加载完成
        mProgressValue = 0;//进度重置为0
        GameRoot.Instance.SetSceneLoadDoneFlag();
    }
    //----------------------------------------------------------------//

    ///////////////////////////ToolMethonds/////////////////////////////    
    public override GameStateType GetStateType()
    {
        return GameStateType.LoadingState;
    }
    //----------------------------------------------------------------//
}