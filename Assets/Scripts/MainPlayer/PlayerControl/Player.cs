using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Reflection;
using System.Security.Permissions;

namespace MainPlayer
{
    public class Player :TInstance<Player>,ISS,IDamageable
    {
        #region 变量,组件相关
        #region 角色控制器相关
        public Vector2 inputDirection;
        private float MouseKey;
        [Space]
        #endregion

        #region 角色属性与数值
        public PlayerData playerData;
        public SpriteRenderer realPlayerPicture; 
        public float realPlayerSpeed = 5f;//速度
        public float realPlayerHealth//生命
        {
            get
            {
                return RealPlayerHealth;
            }
            set
            {
                if (!((isInvincible||areInvincle)&&RealPlayerHealth-value>0))
                {
                    if (value >= realMaxHealth)
                    {
                        value = realMaxHealth;
                    }
                    if (value <= 0)
                    {
                        value = 0;
                    }

                    if(RealPlayerHealth - value > 0&&value>0)
                    {
                        areInvincle = true;
                        realPlayerPicture.DOColor(new Color(1, 1, 1, 0.5f), 0.2f).SetEase(Ease.OutCubic).SetLoops(10, LoopType.Yoyo).OnComplete(() => { areInvincle = false;});
                    }
                    RealPlayerHealth = value;
                    //healthChanging(RealPlayerHealth);
                }
                else
                {
                    value = RealPlayerHealth;
                }
            }
        }
        private float RealPlayerHealth = 100f;
        public static event Action<float> healthChanging;

        public float realMaxHealth//角色最大生命
        {
            get
            {
                return RealMaxHealth;
            }
            set
            {

                if (value <= 0)
                {
                    value = 0;
                }
                if (value <= realPlayerHealth)
                {
                    realPlayerHealth = value;
                }
                RealMaxHealth = value;
                //GenerateHeart(RealMaxHealth);
            }
        }
        private float RealMaxHealth = 100f;
        public static event Action<float> GenerateHeart;

        public float realPlayerDenfense = 10f;//防御值
        public int realLucky = 0;//幸运值
        public int realUnlucky = 0;//不幸值
        public string realStrange;//玩家异常状态
        public float realWeight;//玩家重量
        private LayerMask targetLayer;//角色所在层级
        [Space]
        #endregion

        #region 角色组件与物体
        private Rigidbody2D playerRigidbody;
        [HideInInspector]
        public PlayerAnimation playerAnimation;
        #endregion

        #region 放缩相关
        [Header("角色大小")]
        public Vector3 localScale;//角色大小
        [Space]
        #endregion

        #region 冲刺相关变量
        [Header("冲刺相关")]
        public float dashDistance;//冲刺距离
        public float dashTime;//冲刺时间
        public float WaitDash;//等待冲刺的时间
        private bool isDash;//判断是否在冲刺状态
        private bool canDash;//判断冲刺是否处于CD
        public float dashTimer//dash冷却计时器
        {
            get
            {
                return DashTimer;
            }
            set
            {
                dashAlpha(DashTimer);
                DashTimer = value;
            }
        }
        private float DashTimer;
        public static event Action<float> dashAlpha;
        [Space]
        #endregion

        #region 攻击相关变量
        [Header("攻击相关")]
        private bool isAttack = false;//判断是否处于攻击状态
        public float attackInterval;//攻击间隔计时
        public float initialInterval;//当前武器攻击间隔
        private float repelTimer;//后退时间
        public float Force;//后退受到的力
        [HideInInspector]
        public Vector2 repelDirection;//后退方向
        [HideInInspector]
        public bool isRepel;//判断是否处于击退状态
        [Space]
        #endregion

        #region 武器相关变量
        [Header("武器相关")]
        private WeaponCtrl weaponCtrl;//获取主角子物体控制武器的脚本
        public float changeWeaponInterval;//切换武器的间隔时间
        [Space]
        #endregion

        #region 异常状态相关
        [HideInInspector]
        public bool isInvincible=false;//判断是否处于无敌状态
        #endregion

        #region 受击相关
        [HideInInspector]
        public bool areInvincle = false;//处于受击无敌状态
        [HideInInspector]
        public GameObject attackEnemy;//发起攻击的敌人
        #endregion

        #region Dotween动画
        private Tweener dashTween;
        #endregion

        #region 其他物体相关
        public GameObject stopCanvas;//暂停界面相关的Image
        public GameObject mask;//致盲时生成的图片
        #endregion


        #endregion
        protected override void Awake()
        {
            base.Awake();
            playerAnimation = GetComponentInChildren<PlayerAnimation>();
        }

        void Start()
        {
            FieldInitial();
            ComponentInitial();
            AddBinding();
        }


        void Update()
        {
            if (realPlayerHealth>0)
            {
                inputDirection = BindingChange.Instance.inputControl.GamePlay.Move.ReadValue<Vector2>();
                MouseKey = BindingChange.Instance.inputControl.GamePlay.Attack.ReadValue<float>();

                Attack();
                RecordDash();
            }
            else
            {
                #if UNITY_EDITOR //在编辑器模式下
                //UnityEditor.EditorApplication.isPlaying = false;
                BindingChange.Instance.inputControl.Disable();
                playerAnimation.inputControl.Disable();
                playerRigidbody.velocity = new Vector2(0, 0);
                Destroy(gameObject,2f);
                playerAnimation.TransitionType(PlayerAnimation.playerStates.Die);
                #else
                Application.Quit();
                #endif
            }

            //以下代码测试用，用来打开更换键位的UI
            if (Input.GetMouseButtonDown(1))
            {
                if (stopCanvas.transform.GetChild(2).gameObject.activeSelf)
                {
                    BindingChange.Instance.inputControl.Enable();
                    stopCanvas.transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    BindingChange.Instance.inputControl.Disable();
                    stopCanvas.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }

        private void FixedUpdate()
        {
            if(realPlayerHealth>0)
            {
                Move();
                Repel();
            }
        }

        void AddBinding()//添加按键绑定
        {
            BindingChange.Instance.inputControl.GamePlay.Dash.started += Dash;
            BindingChange.Instance.inputControl.GamePlay.ChangeWeapon.started += ChangeWeapon;
            BindingChange.Instance.inputControl.GamePlay.ChangeItem.started += ChangeItem;
            BindingChange.Instance.inputControl.GamePlay.Exchange.started += Exchange;
            BindingChange.Instance.inputControl.GamePlay.QuitGame.started += QuitGame;

        }

        void FieldInitial()//变量初始化
        {
            DashTimer = 1f;
            RealMaxHealth = 100f;
            RealPlayerHealth = RealMaxHealth;
            isRepel = false;
            attackEnemy = null;
        }

        void ComponentInitial()//组件初始化
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            targetLayer = LayerMask.GetMask("Player");
            weaponCtrl = GetComponentInChildren<WeaponCtrl>();
            realPlayerPicture = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }


        #region 角色相关方法

        private Vector3 Check()//检测前方是否有障碍物
        {
            RaycastHit2D hit;
            if (inputDirection != Vector2.zero)
            {
                hit = Physics2D.Raycast(transform.position, new Vector3(inputDirection.x, inputDirection.y, 0), dashDistance, ~targetLayer,4.9f,5.1f);

            }
            else
            {
                Vector3 lookDirection = new Vector3(transform.GetChild(0).localScale.x, 0, 0);
                hit = Physics2D.Raycast(transform.position, lookDirection, dashDistance, ~targetLayer, 4.9f, 5.1f);
            }

            if (hit.point == Vector2.zero)
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, 0);
                return hitPos;
            }
        }

        private void Move()//移动
        {
            playerRigidbody.velocity = new Vector3(inputDirection.x, inputDirection.y, 0) * realPlayerSpeed;
            if (inputDirection.x > 0)
            {
                transform.GetChild(0).localScale = localScale;
            }
            if (inputDirection.x < 0)
            {
                transform.GetChild(0).localScale = new Vector3(-localScale.x, localScale.y, 0);
            }
        }

        public async void Dash(InputAction.CallbackContext context)//冲刺  L
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            BindingChange.Instance.inputControl.GamePlay.Dash.started -= Dash;
            isDash = true;
            canDash = true;
            dashTimer = 0;
            playerAnimation.TransitionType(PlayerAnimation.playerStates.Dash);
         
            Vector3 target = Check();
            float lookDirection = new Vector3(transform.GetChild(0).localScale.x, 0, 0).magnitude / transform.GetChild(0).localScale.x;
            if (target == Vector3.zero)//没有目标时
            {
                if (inputDirection == Vector2.zero)//键盘无输入
                {
                    dashTween=transform.DOMove(transform.position + new Vector3(lookDirection, 0, 0) * dashDistance, dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                }
                else//键盘有输入
                {
                    dashTween = transform.DOMove(transform.position + new Vector3(inputDirection.x, inputDirection.y, 0) * dashDistance, dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                }
            }
            else//有目标时
            {
                float distance = Vector3.Distance(transform.position, target);
                Vector3 targetPos = (target - transform.position) * 0.95f;
                dashTween = transform.DOMove(transform.position + targetPos, distance * dashTime / dashDistance).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
            }
        }

        public void RecordDash()//Dash冷却计时
        {
            if (canDash)
            {
                if (dashTimer >= WaitDash)
                {
                    dashTimer = 1;
                    BindingChange.Instance.inputControl.GamePlay.Dash.started += Dash;
                }
                else
                {
                    dashTimer += Time.deltaTime;
                }
            }

            if (isDash)
            {
                ShadowPool.instance.GetFromPool();
            }
        }

        private void Attack()//攻击 左键
        {
            if (MouseKey != 0)
            {
                initialInterval = weaponCtrl.GetFacWeaponData().AttachInterval_fac;

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
            BindingChange.Instance.inputControl.GamePlay.ChangeWeapon.started -= ChangeWeapon;
            await UniTask.Delay(TimeSpan.FromSeconds(changeWeaponInterval));
            BindingChange.Instance.inputControl.GamePlay.ChangeWeapon.started += ChangeWeapon;
        }


        private void Exchange(InputAction.CallbackContext context)//可消耗道具与当前手持武器的切换 R
        {

        }

        private void QuitGame(InputAction.CallbackContext context)//切换游戏暂停界面  Escape
        {
            if (stopCanvas.transform.GetChild(0).gameObject.activeSelf)
            {
                BindingChange.Instance.inputControl.Enable();
                stopCanvas.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                BindingChange.Instance.inputControl.Disable();
                stopCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void GetHit(float damage) //受伤
        {
            if (!FindAnyObjectByType<PlayerShield>())
            {
                realPlayerHealth -= damage;
            }
            else
            {
                FindAnyObjectByType<PlayerShield>()?.Resist(attackEnemy);
            }
        }

        public void Repel() //被击退
        {
            if (isRepel)
            {
                if (repelTimer <= 0.2f)
                {
                    repelTimer += Time.deltaTime;
                    PauseTween();
                    playerRigidbody.AddForce(repelDirection * Force);
                    BindingChange.Instance.inputControl.Disable();
                    playerAnimation.inputControl.Disable();
                }
                else
                {
                    playerAnimation.isChange = true;
                    BindingChange.Instance.inputControl.Enable();
                    playerAnimation.inputControl.Enable();
                    repelTimer = 0;
                    isRepel = false;
                }
            }
        }

        public void PauseTween()//暂停Dotween动画
        {          
            if(dashTween!=null)
            {
                dashTween.Kill();
                isDash = false;
                dashTween = null;
            }
        }
        #endregion

        #region 角色异常状态
        public void SS_Acide(float harm)//炎热 参数代表伤害
        {
            if (!isInvincible)
            {
                realPlayerHealth -= harm;
            }
        }

        public void SS_Freeze(float percent)//寒冷 参数代表武器间隔延长时间比例
        {
            if (!isInvincible)
            {
                PlayerBuffMonitor.Instance.AtkSpeedBuff *= (1 + percent);
            }
        }

        public void SS_Fixation()//定身 
        {
            if (!isInvincible)
            {
                BindingChange.Instance.inputControl.GamePlay.Move.Disable();
                playerAnimation.inputControl.GamePlay.Move.Disable();
                BindingChange.Instance.inputControl.GamePlay.Dash.started -= Dash;
            }
            //以下为定身结束后恢复正常代码
            //BindingChange.Instance.inputControl.GamePlay.Move.Enable();
            //playerAnimation.BindingChange.Instance.inputControl.GamePlay.Move.Enable();
            //BindingChange.Instance.inputControl.GamePlay.Dash.started += Dash;
        }

        public void SS_Confuse()//混淆
        {
            if (!isInvincible)
            {
                inputDirection = inputDirection * (-1);
            }
        }

        public void SS_Sticky(float percent)//粘滞 参数代表人物速度减少比例
        {
            if (!isInvincible)
            {
                PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1 - percent);
            }
        }

        public void SS_Burn(float harm)//燃烧 参数代表伤害
        {
            if (!isInvincible)
            {
                realPlayerHealth -= harm;
            }
        }

        public void SS_Clog(float percent)//阻塞 参数代表人物速度减少比例
        {
            if (!isInvincible)
            {
                PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1 - percent);
            }
        }

        public void SS_Dizzy()//抢注
        {
            if (!isInvincible)
            {
                BindingChange.Instance.inputControl.Disable();
            }
            //以下为抢注结束后恢复正常代码
            //BindingChange.Instance.inputControl.Enable();
        }

        public void SS_Hurry(float percent)//急步 参数代表人物速度增加比例
        {
            if (!isInvincible)
            {
                PlayerBuffMonitor.Instance.MoveSpeedBuff *= (1 + percent);
            }
        }

        public void SS_Blind(float radius)//致盲 参数为生成圆的半径
        {
            if (!isInvincible)
            {
                if (mask == null)
                {
                    var obj = Resources.Load<GameObject>("Player/Mask");
                    mask = Instantiate(obj, transform);
                    mask.transform.localScale = new Vector3(radius, radius, 1);
                }
                else
                {
                    mask.SetActive(true);
                }
            }

            //以下为致盲结束后恢复正常代码
            // if (mask != null)
            //{
            //    mask.SetActive(false);
            //}
        }

        public void SS_Charm(Transform target, float speed)//魅惑 第一个参数为发动该异常效果物体的位置，第二个为人物向该物体移动时的速度
        {
            if (!isInvincible)
            {
                BindingChange.Instance.inputControl.Disable();
                playerAnimation.inputControl.Disable();
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            //以下为魅惑结束后恢复正常代码
            //BindingChange.Instance.inputControl.Enable();
            //playerAnimation.BindingChange.Instance.inputControl.Enable();
        }

        public void SS_Invincible()//无敌
        {
            isInvincible = true;

            //以下为无敌结束后恢复正常代码
            //isInvincible = false;
        }

        public void SS_Injury(float harm)//破甲 参数代表伤害增加的数值
        {
            if (!isInvincible)
            {
                PlayerBuffMonitor.Instance.InjuryBuff += harm;
            }

            //Debug.Log(weaponCtrl.GetFacWeaponData().DamageValue_fac);
        }
        #endregion
    }
}

