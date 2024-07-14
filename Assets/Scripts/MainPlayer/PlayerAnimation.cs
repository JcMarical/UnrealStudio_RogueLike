using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// 角色转换动画有关
/// </summary>
public interface IPlayerState
{
    void OnEnter();
    void OnUpdate();

    void OnFixedUpdate();
    void OnExit();
}

public class PlayerAnimation : MonoBehaviour
{
    #region 动画播放相关
    public bool canChange=true;//使一些动画无法被其他动画打断

    public bool isChange = false;//在动画对应的动作结束后通过bool来切换其他动画

    public Vector2 direction;//获取角色移动方向

    private Animator animator;

    public PlayerSettings inputControl;
    #endregion

    #region 状态机相关
    private IPlayerState currentState;
    private Dictionary<playerStates,IPlayerState> states=new Dictionary<playerStates,IPlayerState>();
    #endregion

    public enum playerStates//不同动画状态
    {
        Idle,
        Run,
        Attack,
        Defense,
        Dash
    }

    private void Awake()
    {
        inputControl = new PlayerSettings();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        AddStates();
        TransitionType(playerStates.Idle);
    }

    private void Update()
    {
        direction = inputControl.GamePlay.Move.ReadValue<Vector2>();
        currentState.OnUpdate();
    }
    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    void AddStates()//添加状态
    {
        states.Add(playerStates.Idle, new IdleState(this));
        states.Add(playerStates.Run, new RunState(this));
        states.Add(playerStates.Dash, new DashState(this));
    }

    public void TransitionType(playerStates type)//改变状态
    {
        if(currentState!=null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    public void ChangeAnimation(string name,float transitionTime,int layer)//播放动画
    {
        animator.CrossFade(name, transitionTime, layer);
    }

}

public class IdleState : IPlayerState
{
    private PlayerAnimation playerAnimation;

    public IdleState(PlayerAnimation playerAnimation)
    {
        this.playerAnimation = playerAnimation;
    }

    public void OnEnter()
    {
        playerAnimation.ChangeAnimation("Idle", 0,0);    
    }

    public void OnUpdate()
    {
        if (playerAnimation.canChange &&playerAnimation.direction != Vector2.zero)
        {
            playerAnimation.TransitionType(PlayerAnimation.playerStates.Run);
        }
    }
    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }
}

public class RunState : IPlayerState
{
    private PlayerAnimation playerAnimation;

    public RunState(PlayerAnimation playerAnimation)
    {
        this.playerAnimation = playerAnimation;
    }

    public void OnEnter()
    {
         playerAnimation.ChangeAnimation("Run",0,0);
    }

    public void OnUpdate()
    {
        if (playerAnimation.canChange &&playerAnimation.direction == Vector2.zero)
        {
            playerAnimation.TransitionType(PlayerAnimation.playerStates.Idle);
        }
    }
    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }
}

public class DashState : IPlayerState
{
    private PlayerAnimation playerAnimation;

    public DashState(PlayerAnimation playerAnimation)
    {
        this.playerAnimation = playerAnimation;
    }

    public void OnEnter()
    {
        playerAnimation.ChangeAnimation("Dash", 0,0);
        playerAnimation.canChange = false;
    }

    public void OnUpdate()
    {
        if (!playerAnimation.canChange&&playerAnimation.isChange) 
        {
            if(playerAnimation.direction == Vector2.zero)
            {
                playerAnimation.TransitionType(PlayerAnimation.playerStates.Idle);
            }
            else
            {
                playerAnimation.TransitionType(PlayerAnimation.playerStates.Run);
            }
        }
    }
    public void OnExit()
    {
        playerAnimation.canChange = true;
        playerAnimation.isChange = false;
    }

    public void OnFixedUpdate()
    {

    }
}
