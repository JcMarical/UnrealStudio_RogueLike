using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;


namespace MainPlayer
{
    public interface IDamageable
    {
        void GetHit(float harm);
    }

    public class Player : TInstance<Player>,IDamageable
    {
        #region 变量,组件相关
        #region 角色控制器
        public PlayerSettings inputControl;
        public Vector2 inputDirection;
        public float MouseKey;
        [Space]
        #endregion

        #region 角色属性与数值
        public PlayerData playerData;
        public float realPlayerSpeed=5f;//速度
        public float RealPlayerHealth=100f;
        public float realPlayerHealth//生命
        {
            get
            {
                return RealPlayerHealth;
            }
            set
            {
                if(value>=100)
                {
                    value = 100;
                }
                if(value<=0)
                {
                    value = 0;
                }
                RealPlayerHealth = value;
            }
        }
        public float realPlayerDenfense=10f;//防御值
        public float realMaxHealth=100f;//角色最大生命
        public int realLucky;//幸运值
        public int realUnlucky;//不幸值
        public string realStrange;//玩家异常状态
        public float realWeight;//玩家重量
        private LayerMask targetLayer;//角色所在层级
        [Space]
        #endregion

        #region 角色组件与物体
        private Rigidbody2D playerRigidbody;
        private PlayerAnimation playerAnimation;

        #endregion

        #region 放缩相关
        [Header("角色大小")]
        public Vector3 localScale;//角色大小
        [Space]
        #endregion

        #region 冲刺相关变量
        public float dashDistance;//冲刺距离
        public float dashTime;//冲刺时间
        public float WaitDash;//等待冲刺的时间
        private bool isDash;//判断是否在冲刺状态
        private bool canDash;//判断冲刺是否处于CD
        public float dashTimer;//dash冷却计时器
        [Space]
        #endregion

        #region 攻击相关变量
        private bool isAttack=false;//判断是否处于攻击状态
        public float attackInterval;//攻击间隔计时
        public float initialInterval;//当前武器攻击间隔
        [Space]
        #endregion

        #region 武器相关变量
        private WeaponCtrl weaponCtrl;//获取主角子物体控制武器的脚本
        public float changeWeaponInterval;//切换武器的间隔时间
        [Space]
        #endregion

        #region 其他物体相关
        public GameObject stopCanvas;//暂停界面相关的Image
        private BindingChange bindingChange;
        #endregion

        #endregion
        protected override void Awake()
        {
            base.Awake();
            if (bindingChange == null)
            {
                bindingChange = FindObjectOfType<BindingChange>();
            }
            inputControl = bindingChange.inputControl;
        }

        private void OnEnable()
        {
            inputControl.Enable();
        }

        private void OnDisable()
        {
            inputControl.Disable();
        }
        void Start()
        {
            dashTimer = 1f;
            Initial();
            AddBinding();
        }


        void Update()
        {
            inputDirection=inputControl.GamePlay.Move.ReadValue<Vector2>();
            MouseKey = inputControl.GamePlay.Attack.ReadValue<float>();

            Attack();
            RecordDash();

            //以下代码测试用，用来打开更换键位的UI
            if(Input.GetMouseButtonDown(1))
            {
                if (stopCanvas.transform.GetChild(2).gameObject.activeSelf)
                {
                    inputControl.Enable();
                    stopCanvas.transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    inputControl.Disable();
                    stopCanvas.transform.GetChild(2).gameObject.SetActive(true);
                }
            }

            //if(realPlayerHealth<=0)
            //{
            //    realPlayerHealth = 0;
            //}
            //if(realPlayerHealth>=100)
            //{
            //    realPlayerHealth = 100;
            //}
        }

        private void FixedUpdate()
        {
            Move();
        }

        void AddBinding()//添加按键绑定
        {
            inputControl.GamePlay.Dash.started += Dash;     
            inputControl.GamePlay.ChangeWeapon.started += ChangeWeapon;
            inputControl.GamePlay.ChangeItem.started += ChangeItem;
            inputControl.GamePlay.Exchange.started += Exchange; 
            inputControl.GamePlay.QuitGame.started += QuitGame;
    
        }

        void Initial()//初始化
        {
            playerAnimation = GetComponentInChildren<PlayerAnimation>();
            playerRigidbody = GetComponent<Rigidbody2D>();
            targetLayer = LayerMask.GetMask("Player");
            weaponCtrl = GetComponentInChildren<WeaponCtrl>();
        }
     

        #region 角色相关方法

        private Vector3 Check()//检测前方是否有障碍物
        {
            RaycastHit2D hit;
            if (inputDirection != Vector2.zero)
            {
                hit = Physics2D.Raycast(transform.position, new Vector3(inputDirection.x, inputDirection.y, 0), dashDistance, ~targetLayer, 4.9f, 5.1f);

            }
            else
            {
                Vector3 lookDirection = new Vector3(transform.GetChild(0).localScale.x, 0, 0);
                hit = Physics2D.Raycast(transform.position, lookDirection , dashDistance, ~targetLayer, 4.9f, 5.1f);
            }

            if (hit.point==Vector2.zero)
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, 0);
                return hitPos;
            }
        }

        public void GetHit(float harm)//受伤
        {
            
        }

        private void Move()//移动
        {
            playerRigidbody.velocity = new Vector3(inputDirection.x, inputDirection.y, 0)*realPlayerSpeed;
            if(inputDirection.x>0)
            {
                transform.GetChild(0).localScale = localScale;
            }
            if(inputDirection.x<0)
            {
                transform.GetChild(0).localScale = new Vector3(-localScale.x, localScale.y, 0);
            }
        }

        private void Dash(InputAction.CallbackContext context)//冲刺  L
        {
            inputControl.GamePlay.Dash.started -= Dash;
            isDash = true;
            canDash = true;
            dashTimer = 0;
            playerAnimation.TransitionType(PlayerAnimation.playerStates.Dash);

            Vector3 target = Check();
            float lookDirection = new Vector3(transform.GetChild(0).localScale.x, 0, 0).magnitude / transform.GetChild(0).localScale.x;
            if (target == Vector3.zero)//没有目标时
            {
                if(inputDirection==Vector2.zero)//键盘无输入
                {
                    transform.DOMove(transform.position+new Vector3(lookDirection,0,0) * dashDistance,dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                }
                else//键盘有输入
                {
                    transform.DOMove(transform.position+new Vector3(inputDirection.x, inputDirection.y, 0) * dashDistance, dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                } 
            }
            else//有目标时
            {
                float distance = Vector3.Distance(transform.position, target);
                Vector3 targetPos = (target- transform.position) * 0.95f;
                transform.DOMove(transform.position+targetPos, distance*dashTime/dashDistance).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
            }

        }

        public void RecordDash()//Dash冷却计时
        {
            if(canDash)
            {
                if(dashTimer>=WaitDash)
                {
                    dashTimer = 1;
                    inputControl.GamePlay.Dash.started += Dash;
                }
                else
                {
                    dashTimer += Time.deltaTime;
                    PlayerItemsUI.dashAlpha(dashTimer);
                }
            }

            if (isDash)
            {
                ShadowPool.instance.GetFromPool();
            }
        }

        private void Attack()//攻击 左键
        {
            if(MouseKey!=0)
            {
                initialInterval = weaponCtrl.GetWeaponData()[0].AttachInterval;

                if (Input.GetMouseButtonDown(0) && !isAttack && attackInterval <= 0)
                {
                    weaponCtrl.Attack();
                    isAttack = true;
                    attackInterval = initialInterval;
                    Debug.Log(1);
                }

                if (Input.GetMouseButton(0) && isAttack)
                {
                    if (attackInterval <= 0)
                    {
                        weaponCtrl.Attack();
                        attackInterval = initialInterval;
                        Debug.Log(3);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isAttack = false;
                Debug.Log(2);
            }

            if (attackInterval >= -1f)
            {
                attackInterval -= Time.deltaTime;
            }
        }


        private void ChangeItem(InputAction.CallbackContext context)//更换道具  F
        {
           
        }


        private async void ChangeWeapon(InputAction.CallbackContext context)//更换武器  Space
        {
                isAttack = false;//打断攻击
                weaponCtrl.ChangeWeapon();
                inputControl.GamePlay.ChangeWeapon.started -= ChangeWeapon;
                await UniTask.Delay(TimeSpan.FromSeconds(changeWeaponInterval));
                inputControl.GamePlay.ChangeWeapon.started += ChangeWeapon;
        }


        private void Exchange(InputAction.CallbackContext context)//可消耗道具与当前手持武器的切换 R
        {
     
        }

        private void QuitGame(InputAction.CallbackContext context)//切换游戏暂停界面  Escape
        {
            if(stopCanvas.transform.GetChild(0).gameObject.activeSelf)
            {
                inputControl.Enable();
                stopCanvas.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                inputControl.Disable();
                stopCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        #endregion
    }
}

