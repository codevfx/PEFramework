/************************************************************
    File      : GameRoot
	Author    : Plane
    Version   : 1.0
    Function  : 游戏全局管理控制类
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

    private List<IGameState> gameStateList = new List<IGameState>();//存储所有的gameState
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

        //游戏启动时需要通过Loading状态进入LoginState，所以这里先把Loading状态创建出来
        IGameState ls = CreateGameState<LoadingState>(StateGB.transform);
        ls.Init();
        gameStateList.Add(ls);

        StartCoroutine(SwitchToLoginState());
    }
    void Update()
    {
        //新场景加载完成后，立即切换到应的游戏状态
        if (mLoadSceneDone)
        {
            ChangeGameStateToTarget(dstGameState);
            mLoadSceneDone = false;
        }

        //统一处理各个系统的Update函数
        for (int i = 0; i < systemList.Count; i++)
        {
            systemList[i].SysUpdate();
        }
    }

    private IEnumerator SwitchToLoginState()
    {
        //等待PEWindowMgr完成初始化后才开始创建核心系统，进入LoginState
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
    IGameState levGameState;//离开的场景，记录下来用于清理场景垃圾
    public void ChangeGameStateToTarget(GameStateType inputState)
    {
        if (curGameState == inputState) return;

        if(curGameState != GameStateType.None)
        {
            levGameState = GetGameStateInMap(curGameState);
        }
        if (curGameState == GameStateType.LoadingState)
        {
            //如果当前正处于loading状态
            curGameState = dstGameState;
        }
        else
        {
            //如果当前处于非loading的状态
            dstGameState = inputState;
            LoadingState.mLoadSceneDone = false;
            //把当前状态跳转到loading状态，加载启动加载新场景
            curGameState = GameStateType.LoadingState;

            //当启动游戏进入登录状态时特殊处理，不弹出loading界面，其它的状态转换都弹出Loading界面
            if (dstGameState != GameStateType.LoginState)
            {
                //打开Loading界面
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
        //注册一些事件，如游戏暂停，切换状态回调等
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