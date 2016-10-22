/************************************************************
    File      : IGameState
	Author    : Plane
    Version   : 1.0
    Function  : ��Ϸ״̬����
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
        //�����л���ɺ�״̬�����˸ı䣬��ԭ���������������Դ���л��մ���
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