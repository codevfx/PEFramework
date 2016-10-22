/************************************************************
    File      : GameRoot
	Author    : Plane
    Version   : 1.0
    Function  : ��Ϸȫ�ֹ��������
    Date      : 2016/10/19 15:56:9
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GameRoot : MonoBehaviour 
{
    ///////////////////////////Data Define//////////////////////////////    
    private static GameRoot instance = null;
    public static GameRoot Instance
    {
        get { return instance; }
    }
    GameObject StateGB = null;
    GameObject SystemGB = null;
    private List<ISystem> systemList = new List<ISystem>();
    private Dictionary<Type, ISystem> systemMap = new Dictionary<Type, ISystem>();

    private List<IGameState> gameStateList = new List<IGameState>();//�洢���е�gameState
    public static bool isStateGBCreateDone = false;
    private bool mLoadSceneDone = false;
    //----------------------------------------------------------------//

    ///////////////////////////MainFunctions////////////////////////////
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        //Create GameState Objs
        StateGB = new GameObject();
        StateGB.name = "StateRoot";
        StateGB.transform.parent = this.transform;
        StateGB.transform.localPosition = Vector3.zero;
        //Create GameSystem Objs
        SystemGB = new GameObject();
        SystemGB.name = "SystemRoot";
        SystemGB.transform.parent = this.transform;
        SystemGB.transform.localPosition = Vector3.zero;

        //��Ϸ����ʱ��Ҫͨ��Loading״̬����LoginState�����������Ȱ�Loading״̬��������
        IGameState ls = CreateGameState<LoadingState>(StateGB.transform);
        ls.Init();
        gameStateList.Add(ls);

        StartCoroutine(SwitchToLoginState());
    }
    void Update()
    {
        //�³���������ɺ������л���Ӧ����Ϸ״̬
        if (mLoadSceneDone)
        {
            ChangeGameStateToTarget(dstGameState);
            mLoadSceneDone = false;
        }

        //ͳһ�������ϵͳ��Update����
        for (int i = 0; i < systemList.Count; i++)
        {
            systemList[i].SysUpdate();
        }
    }

    private IEnumerator SwitchToLoginState()
    {
        //�ȴ�PEWindowMgr��ɳ�ʼ����ſ�ʼ��������ϵͳ������LoginState
        while(!PEWindowMgr.Instance.isInitDone)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        InitGameCoreSystems();
        ChangeGameStateToTarget(GameStateType.LoginState);
    }
    private void InitGameCoreSystems()
    {
        AddGameSys<ResourceSys>(SystemGB.transform);
        AddGameSys<EventSys>(SystemGB.transform);
        //AddGameSys<ABSys>(SystemGB.transform);
        InitAllGameSyss();
    }
    GameStateType curGameState = GameStateType.None;
    GameStateType dstGameState = GameStateType.None;
    IGameState levGameState;//�뿪�ĳ�������¼������������������
    public void ChangeGameStateToTarget(GameStateType inputState)
    {
        if (curGameState == inputState) return;

        if(curGameState != GameStateType.None)
        {
            levGameState = GetGameStateInMap(curGameState);
        }
        if (curGameState == GameStateType.LoadingState)
        {
            //�����ǰ������loading״̬
            curGameState = dstGameState;
        }
        else
        {
            //�����ǰ���ڷ�loading��״̬
            dstGameState = inputState;
            LoadingState.mLoadSceneDone = false;
            //�ѵ�ǰ״̬��ת��loading״̬���������������³���
            curGameState = GameStateType.LoadingState;

            //��������Ϸ�����¼״̬ʱ���⴦��������loading���棬������״̬ת��������Loading����
            if (dstGameState != GameStateType.LoginState)
            {
                //��Loading����
                //TODO
                //PEWindowMgr.Instance.InitWindowCache(....)
            }
        }

        IGameState dstState = GetGameStateInMap(curGameState);
        if (dstState != null)
        {
            dstState.Enter();
        }
    }
    public void CreateAllGameStates()
    {
        //ע��һЩ�¼�������Ϸ��ͣ���л�״̬�ص���
        //TODO
        StartCoroutine(CreateGSCoroutine());
    }
    private IEnumerator CreateGSCoroutine()
    {
        IGameState gs = CreateGameState<LoginState>(StateGB.transform);
        gs.Init();
        gameStateList.Add(gs);
        /*
        gs = CreateGameState<LoadingState>(StateGB.transform);
        gs.Init();
        gameStateList.Add(gs);
        */

        isStateGBCreateDone = true;
        yield break;
    }
    //----------------------------------------------------------------//

    ///////////////////////////ToolMethonds/////////////////////////////
    void AddGameSys<T>(Transform transform) where T : ISystem, new()
    {
        GameObject go = new GameObject();
        go.transform.parent = transform;
        ISystem sys = go.AddComponent<T>();
        go.name = sys.GetName();
        systemList.Add(sys);
        systemMap.Add(sys.GetType(), sys);
    }
    private void InitAllGameSyss()
    {
        for (int i = 0; i < systemList.Count; i++)
        {
            systemList[i].Init();
        }
    }
    public IGameState GetGameStateInMap(GameStateType state)
    {
        for (int i = 0; i < gameStateList.Count; i++)
        {
            if (gameStateList[i].GetStateType() == state)
            {
                return gameStateList[i];
            }
        }
        return null;
    }
    public GameStateType GetDstGameState()
    {
        return dstGameState;
    }
    public void ClearLastLeaveState()
    {
        if (levGameState != null)
            levGameState.Leave();
    }
    public IGameState CreateGameState<T>(Transform parent) where T : IGameState
    {
        GameObject go = new GameObject();
        go.transform.parent = parent;
        IGameState gs = go.AddComponent<T>();
        go.name = gs.GetName();
        return gs;
    }
    public void SetSceneLoadDoneFlag()
    {
        mLoadSceneDone = true;
    }
    //----------------------------------------------------------------//
}

public class ISystem : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void SysUpdate() { }
    public string GetName()
    {
        return GetType().Name;
    }
}