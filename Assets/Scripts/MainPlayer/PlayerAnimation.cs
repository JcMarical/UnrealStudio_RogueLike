using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public interface IPlayerState
{
    void OnEnter();

    void OnUpdate();
    void OnExit();

  

}
public class PlayerAnimation : MonoBehaviour
{
    #region 动画播放相关
    public bool canChange=true;

    public Vector2 direction;

    private Animator animator;

    private PlayerSettings input;


    #endregion

    #region 状态机相关
    private IPlayerState currentState;
    private Dictionary<playerStates,IPlayerState> states=new Dictionary<playerStates,IPlayerState>();
    #endregion

    public enum playerStates
    {
        Idle,
        Run,
        Attack,
        Defense
    }

    private void Awake()
    {
        input =new PlayerSettings();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        AddStates();
        TransitionType(playerStates.Idle);
    }

    private void Update()
    {
        direction = input.GamePlay.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        currentState.OnUpdate();
    }

    void AddStates()
    {
        states.Add(playerStates.Idle, new IdleState(this));
        states.Add(playerStates.Run, new RunState(this));
    }

    public void TransitionType(playerStates type)
    {
        if(currentState!=null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    public void ChangeAnimnation(string name,float transitionTime)
    {
        animator.CrossFade(name, transitionTime, 0);
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
        playerAnimation.ChangeAnimnation("Idle", 0);    
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
         playerAnimation.ChangeAnimnation("Run",0);
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

}
