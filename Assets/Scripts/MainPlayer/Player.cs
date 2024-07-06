using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Drawing.Text;
using UnityEngine.UI;

namespace MainPlayer
{
    public interface IDamageable
    {
        void GetHit(float harm);
    }


    public class Player : MonoBehaviour, IDamageable
    {
        #region 变量,组件相关
        #region 角色控制器
        public PlayerSettings inputControl;
        public Vector2 inputDirection;
        #endregion

        #region 角色属性
        public PlayerData playerData;
        private LayerMask targetLayer;//角色所在层级
        [Space]
        #endregion

        #region 角色组件
        private Rigidbody2D playerRigidbody;
        private PlayerAnimation playerAnimation;
        private WeaponCtrl weaponCtrl;//获取主角子物体控制武器的脚本

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
        private float dashTimer;//dash冷却计时器
        [Space]
        #endregion

        #region 攻击相关变量
        private bool isAttack=false;//判断是否处于攻击状态
        public float attackInterval;//攻击间隔计时
        public float initialInterval;//当前武器攻击间隔
        [Space]
        #endregion

        #region 其他组件相关
        public GameObject stopCanvas;//暂停界面相关的Image
        #endregion

        #endregion



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
        void Start()
        {
            Initial();
            AddBinding();
        }


        void Update()
        {
            inputDirection=inputControl.GamePlay.Move.ReadValue<Vector2>();
            RecordDash();
            Attack();
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
            initialInterval = 2f;
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
            playerRigidbody.velocity = new Vector3(inputDirection.x, inputDirection.y, 0)*playerData.playerSpeed;
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
            playerAnimation.TransitionType(PlayerAnimation.playerStates.Dash);

            Vector3 target = Check();
            float lookDirection = new Vector3(transform.GetChild(0).localScale.x, 0, 0).magnitude / transform.GetChild(0).localScale.x;
            if (target == Vector3.zero)
            {
                if(inputDirection==Vector2.zero)
                {
                    transform.DOMove(transform.position+new Vector3(lookDirection,0,0) * dashDistance,dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                }
                else
                {
                    transform.DOMove(transform.position+new Vector3(inputDirection.x, inputDirection.y, 0) * dashDistance, dashTime).SetEase(Ease.OutCubic).OnComplete(() => { playerAnimation.isChange = true; isDash = false; });
                } 
            }
            else
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
                dashTimer += Time.deltaTime;
                if(dashTimer>=WaitDash)
                {
                    dashTimer = 0;
                    inputControl.GamePlay.Dash.started += Dash;
                }
            }

            if (isDash)
            {
                ShadowPool.instance.GetFromPool();
            }
        }

        private void Attack()//攻击 左键
        {
           
            if (Input.GetMouseButtonDown(0)&&!isAttack)
            {
                weaponCtrl.Attack();
                isAttack = true;
                attackInterval = initialInterval;
                Debug.Log(1);
            }


            if (Input.GetMouseButton(0)&&isAttack)
            { 
                attackInterval -= Time.deltaTime;
                if (attackInterval<=0)
                {
                    weaponCtrl.Attack();
                    attackInterval = initialInterval;
                    Debug.Log(3);
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isAttack = false;
                Debug.Log(2);
            }
        }


        private void ChangeItem(InputAction.CallbackContext context)//更换道具  F
        {
           
        }


        private async void ChangeWeapon(InputAction.CallbackContext context)//更换武器  Space
        {
            isAttack = false;//打断攻击
            initialInterval= 2f;//计算切换后的武器的时间间隔
            weaponCtrl.ChangeWeapon();
            inputControl.GamePlay.ChangeWeapon.started -= ChangeWeapon;
            await UniTask.Delay(TimeSpan.FromSeconds(10f));
            inputControl.GamePlay.ChangeWeapon.started += ChangeWeapon;
        }


        private void Exchange(InputAction.CallbackContext context)//可消耗道具与当前手持武器的切换 R
        {
     
        }

        private void QuitGame(InputAction.CallbackContext context)//切换游戏暂停界面  Escape
        {
            if(stopCanvas.transform.GetChild(0).gameObject.activeSelf)
            {
                stopCanvas.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                stopCanvas.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        #endregion
    }
}

