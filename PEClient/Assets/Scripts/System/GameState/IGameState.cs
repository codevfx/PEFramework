/************************************************************
    File      : IGameState
	Author    : Plane
    Version   : 1.0
    Function  : 游戏状态父类
    Date      : 2016/10/19 16:58:17
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class IGameState : MonoBehaviour
{
    ///////////////////////////Data Define//////////////////////////////
    protected static float mProgressValue = 0f;
    public static float ProgressValue
    {
        get { return mProgressValue; }
    }
    //----------------------------------------------------------------//
    public virtual void Init() { }
    public virtual void Enter() { }
    public virtual void Leave()
    {
        //场景切换完成后，状态发生了改变，对原来场景里的垃圾资源进行回收处理
        //TODO
        System.GC.Collect();
    }

    public virtual GameStateType GetStateType()
    {
        return GameStateType.None;
    }
    public virtual string GetName()
    {
        return GetType().Name;
    }
}

public enum GameStateType
{
    None,
    LoadingState,
    LoginState,
    MainCityState
}