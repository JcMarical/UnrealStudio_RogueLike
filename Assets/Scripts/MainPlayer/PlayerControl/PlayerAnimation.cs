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

    public class AnimationProperties
    {
        public Vector2 direction;//获取角色移动方向

        public int aniSpeed;//动画播放速度
        public int inputX;//按键输入X值
        public int inputY;//按键输入Y值
        public int attackSpeed;//动画攻击速度
        public int realSpeed;//人物真实速度
        public int angle;//判断鼠标点击位置与人物位置连成的线与水平的夹角

        public float baseSpeed;//动画移动初始速度
        public float baseAttackSpeed;//动画攻击初始速度

        public AnimationProperties()
        {
            aniSpeed = Animator.StringToHash("aniSpeed");
            inputX = Animator.StringToHash("inputX");
            inputY = Animator.StringToHash("inputY");
            attackSpeed = Animator.StringToHash("attackSpeed");
            realSpeed = Animator.StringToHash("realSpeed");
            angle = Animator.StringToHash("angle");
            baseSpeed = 0.4f;
            baseAttackSpeed = 1f;
        }
    }

    public class PlayerAnimation :MonoBehaviour
    {
        #region 动画播放相关
        public bool canChange;//使一些动画无法被其他动画打断

        public bool isChange;//在动画对应的动作结束后通过bool来切换其他动画

        [HideInInspector]
        public AnimationProperties properties;//动画属性

        [HideInInspector]
        public Animator animator;

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
            properties = new AnimationProperties();
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
            properties.direction = inputControl.GamePlay.Move.ReadValue<Vector2>();
            currentState.OnUpdate();
        }
        private void FixedUpdate()
        {
            currentState.OnFixedUpdate();
        }

        void AddStates()//添加状态
        {
            states.Add(playerStates.Idle, new IdleState(this,animator,properties));
            states.Add(playerStates.Run, new RunState(this,animator, properties));
            states.Add(playerStates.Dash, new DashState(this,properties));
            states.Add(playerStates.Harm, new HarmState(this, properties));
            states.Add(playerStates.Die, new DieState(this));
            states.Add(playerStates.Attack, new AttackState(this, animator,properties));
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
        public void SetWeaponColliderEnable(){
            StaticData.Instance.GetActiveWeapon().transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
        }
        public void SetWeaponColliderDisenable(){
           StaticData.Instance.GetActiveWeapon().transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
        }
    }


    #region 人物各种状态
    public class IdleState : IPlayerState
    {
        private PlayerAnimation playerAnimation;
        private Animator animator;
        private AnimationProperties properties;

        public IdleState(PlayerAnimation playerAnimation, Animator animator, AnimationProperties properties)
        {
            this.playerAnimation = playerAnimation;
            this.animator = animator;
            this.properties = properties;
        }

        public void OnEnter()
        {
            playerAnimation.isChange = false;
            playerAnimation.ChangeAnimation("Idle", 0, 0);
            animator.SetFloat(properties.aniSpeed, Player.Instance.RealPlayerSpeed * properties.baseSpeed);
        }

        public void OnUpdate()
        {
            if (playerAnimation.canChange && properties.direction != Vector2.zero)
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
        private AnimationProperties properties;

        public RunState(PlayerAnimation playerAnimation,Animator animator, AnimationProperties properties)
        {
            this.playerAnimation = playerAnimation;
            this.animator = animator;
            this.properties = properties;
        }

        public void OnEnter()
        {
            playerAnimation.isChange = false;
            playerAnimation.ChangeAnimation("Move", 0, 0);
            animator.SetFloat(properties.inputX, Mathf.Abs(Player.Instance.inputDirection.x));
            animator.SetFloat(properties.inputY,Player.Instance.inputDirection.y);
            animator.SetFloat(properties.aniSpeed, Player.Instance.RealPlayerSpeed* properties.baseSpeed);
        }

        public void OnUpdate()
        {
            animator.SetFloat(properties.aniSpeed, Player.Instance.RealPlayerSpeed * properties.baseSpeed);
            animator.SetFloat(properties.inputY, Player.Instance.inputDirection.y);
            animator.SetFloat(properties.inputX, Mathf.Abs(Player.Instance.inputDirection.x));
            if (playerAnimation.canChange && properties.direction == Vector2.zero)
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
        private AnimationProperties properties;

        public DashState(PlayerAnimation playerAnimation, AnimationProperties properties)
        {
            this.playerAnimation = playerAnimation;
            this.properties = properties;
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
                if (properties.direction == Vector2.zero)
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

    public class AttackState : IPlayerState
    {
        private PlayerAnimation playerAnimation;
        private Animator animator;
        private AnimationProperties properties;

        public AttackState(PlayerAnimation playerAnimation, Animator animator, AnimationProperties properties)
        {
            this.playerAnimation = playerAnimation;
            this.animator = animator;
            this.properties = properties;
        }

        public void OnEnter()
        {
            playerAnimation.canChange = false;
            animator.SetTrigger("isAttack");
            animator.SetLayerWeight(1, 1);
        }

        public void OnUpdate()
        {
            animator.SetFloat(properties.attackSpeed, properties.baseAttackSpeed / Player.Instance.intervalBonus);
            animator.SetFloat(properties.realSpeed, properties.direction.magnitude);
            animator.SetFloat(properties.angle, Player.Instance.angle);

            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime>=0.95f&&!animator.GetCurrentAnimatorStateInfo(1).loop)
            {
                playerAnimation.isChange = true;
            }

            if (!playerAnimation.canChange && playerAnimation.isChange)
            {
                if (properties.direction == Vector2.zero)
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
            Player.Instance.attackDirection = 0;
            Player.Instance.speedDown = 1f;
            animator.SetLayerWeight(1, 0);
            playerAnimation.ChangeAnimation("AttackEmpty", 0, 1);
            animator.ResetTrigger("isAttack");
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
        private AnimationProperties properties;

        public HarmState(PlayerAnimation playerAnimation, AnimationProperties properties)
        {
            this.playerAnimation = playerAnimation;
            this.properties = properties;
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
                if (properties.direction == Vector2.zero)
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
