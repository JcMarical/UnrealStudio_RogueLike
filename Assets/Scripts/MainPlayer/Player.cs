using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainPlayer
{
    public class Player : MonoBehaviour
    {
        #region 角色控制器
        public PlayerSettings inputControl;
        public Vector2 inputDirection;
        #endregion

        #region 角色属性
        public PlayerData playerData;
        #endregion

        #region 角色组件
        private Rigidbody2D playerRigidbody;
        private Animator playerAnimator;
        #endregion

        #region Vector3变量
        public Vector3 localScale;
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
            playerRigidbody= GetComponent<Rigidbody2D>();
        }


        void Update()
        {
            inputDirection=inputControl.GamePlay.Move.ReadValue<Vector2>();  
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            playerRigidbody.velocity = new Vector3(inputDirection.x, inputDirection.y, 0)*playerData.playerSpeed;
            if(inputDirection.x>0)
            {
                transform.localScale = localScale;
            }
            if(inputDirection.x<0)
            {
                transform.localScale = new Vector3(-localScale.x, localScale.y, 0);
            }
        }
    }
}

