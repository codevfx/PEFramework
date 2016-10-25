/************************************************************
    File      : LoadingState
	Author    : Plane
    Version   : 1.0
    Function  : ��Ϸ����״̬��ͨ������ɸ���״̬���л�
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
        //��LoginState������ش�������LoginStateʱ����ҪLoading����
        //TODO

        //��Ŀ�겻��LoginState����Loading����û����ʾ������������ó���һֱ��������ȴ�
        while (GameRoot.Instance.GetDstGameState() != GameStateType.LoginState)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        AsyncOperation mAsync = Application.LoadLevelAsync(mSceneName);

        //����ʼ�����µĳ���ʱҪ�����һ���������������մ���
        GameRoot.Instance.ClearLastLeaveState();
        
        //����Loading����ļ��ؽ�����,�������ع��̳���һֱ��������
        while(!mAsync.isDone)
        {
            //���¼�����ֵ
            mProgressValue = mAsync.progress;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //���GameState����Ƿ��Ѿ������ˣ�������������Ϸ�����ｫ������һ�Ρ�
        if (!GameRoot.isStateGBCreateDone)
        {
            //Create GameStates
            GameRoot.Instance.CreateAllGameStates();
            //������𣬵ȴ�����GameState�������������
            while(!GameRoot.isStateGBCreateDone)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Debug.Log("Create StateGB Done!");            
        }
        mLoadSceneDone = true;//�³����������
        mProgressValue = 0;//��������Ϊ0
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