using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainPlayer
{
    /// <summary>
    /// 角色冲刺残影有关脚本
    /// </summary>
    public class ShadowSprite : MonoBehaviour
    {
        private Transform player;

        private SpriteRenderer spriteRenderer;
        private SpriteRenderer playerRenderer;
        private Renderer thisRenderer;

        private Color color;

        [Header("时间控制参数")]
        public float activeTime;
        public float activeStart;

        [Header("不透明度调整")]
        private float alpha;
        public float alphaSet;
        public float alphaSpeed;

        private void OnEnable()
        {
            player = Player.Instance.transform.GetChild(0);
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerRenderer = player.GetComponent<SpriteRenderer>();
            thisRenderer = GetComponent<Renderer>();

            activeTime = 1f;

            alphaSet = 1f;
            alphaSpeed = 0.8f;
            alpha = alphaSet;

            spriteRenderer.sprite = playerRenderer.sprite;
            spriteRenderer.size = playerRenderer.size;
            thisRenderer.material = playerRenderer.material;

            transform.position = player.position;
            transform.localScale = player.localScale;
            transform.rotation = player.rotation;

            activeStart = Time.time;

            color = new Color(1f, 1f, 1f, 1);
        }
        void Update()
        {
            alpha *= alphaSpeed;
            color.a = alpha;

            spriteRenderer.color = color;

            if (Time.time > activeStart + activeTime)
            {
                ShadowPool.instance.ReturnPool(this.gameObject);
            }
        }
    }
}

