using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// 角色转换动画有关
/// </summary>
/// 
namespace MainPlayer
{
    public interface IPlayerState
    {
        void OnEnter();
        void OnUpdate();

        void OnFixedUpdate();
        void OnExit();
    }

    public class PlayerAnimation :MonoBehaviour
    {
        #region 动画播放相关
        public bool canChange;//使一些动画无法被其他动画打断

        public bool isChange;//在动画对应的动作结束后通过bool来切换其他动画

        public Vector2 direction;//获取角色移动方向

        private Animator animator;

        public PlayerSettings inputControl;

        #endregion

        #region 状态机相关
        private IPlayerState currentState;
        private Dictionary<playerStates, IPlayerState> states = new Dictionary<playerStates, IPlayerState>();
        #endregion

        public enum playerStates//不同动画状态
        {
            Idle,
            Run,
            Attack,
            Defense,
            Dash,
            Harm,
            Die
        }

        private void Awake()
        {
            inputControl = new PlayerSettings();
            animator = GetComponent<Animator>();

            
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
            AddStates();
            TransitionType(playerStates.Idle);
            canChange = true;
            isChange = false;
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
            states.Add(playerStates.Idle, new IdleState(this,animator));
            states.Add(playerStates.Run, new RunState(this,animator));
            states.Add(playerStates.Dash, new DashState(this));
            states.Add(playerStates.Harm, new HarmState(this));
            states.Add(playerStates.Die, new DieState(this));
        }

        public void TransitionType(playerStates type)//改变状态
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }
            currentState = states[type];
            currentState.OnEnter();
        }

        public void ChangeAnimation(string name, float transitionTime, int layer)//播放动画
        {
            animator.CrossFade(name, transitionTime, layer);
        }

    }


    #region 人物各种状态
    public class IdleState : IPlayerState
    {
        private PlayerAnimation playerAnimation;
        private Animator animator;

        public IdleState(PlayerAnimation playerAnimation, Animator animator)
        {
            this.playerAnimation = playerAnimation;
            this.animator = animator;
        }

        public void OnEnter()
        {
            playerAnimation.isChange = false;
            playerAnimation.ChangeAnimation("Idle", 0, 0);
            animator.SetFloat("aniSpeed", Player.Instance.realPlayerSpeed * 0.25f);
        }

        public void OnUpdate()
        {
            animator.SetFloat("aniSpeed", Player.Instance.realPlayerSpeed * 0.25f);
            if (playerAnimation.canChange && playerAnimation.direction != Vector2.zero)
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
        private Animator animator;
        private float input;

        public RunState(PlayerAnimation playerAnimation,Animator animator)
        {
            this.playerAnimation = playerAnimation;
            this.animator= animator;
        }

        public void OnEnter()
        {
            playerAnimation.isChange = false;
            playerAnimation.ChangeAnimation("Move", 0, 0);
            animator.SetFloat("input", Mathf.Abs(Player.Instance.inputDirection.x));
            animator.SetFloat("aniSpeed", Player.Instance.RealPlayerSpeed*0.25f);
        }

        public void OnUpdate()
        {
            animator.SetFloat("aniSpeed", Player.Instance.RealPlayerSpeed * 0.25f);
            animator.SetFloat("input", Mathf.Abs(Player.Instance.inputDirection.x));
            if (playerAnimation.canChange && playerAnimation.direction == Vector2.zero)
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
            playerAnimation.ChangeAnimation("Dash", 0, 0);
            playerAnimation.canChange = false;
        }

        public void OnUpdate()
        {
            if (!playerAnimation.canChange && playerAnimation.isChange)
            {
                if (playerAnimation.direction == Vector2.zero)
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

    public class HarmState : IPlayerState
    {
        private PlayerAnimation playerAnimation;

        public HarmState(PlayerAnimation playerAnimation)
        {
            this.playerAnimation = playerAnimation;
        }

        public void OnEnter()
        {
            playerAnimation.ChangeAnimation("GetHit", 0, 0);
            playerAnimation.canChange = false;
        }

        public void OnUpdate()
        {
            if (!playerAnimation.canChange && playerAnimation.isChange)
            {
                if (playerAnimation.direction == Vector2.zero)
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

    public class DieState : IPlayerState
    {
        private PlayerAnimation playerAnimation;

        public DieState(PlayerAnimation playerAnimation)
        {
            this.playerAnimation = playerAnimation;
        }

        public void OnEnter()
        {
            playerAnimation.ChangeAnimation("Die", 0, 0);
            playerAnimation.canChange = false;
        }

        public void OnUpdate()
        {
 
        }
        public void OnExit()
        {

        }

        public void OnFixedUpdate()
        {

        }
    }
    #endregion

}
