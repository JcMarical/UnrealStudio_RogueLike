using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainPlayer
{
    /// <summary>
    /// 角色冲刺残影有关脚本
    /// </summary>
    public class ShadowPool : MonoBehaviour
    {
        public static ShadowPool instance;

        public GameObject shadowPrefab;

        public int shadowCount;

        private Queue<GameObject> activeObjects = new Queue<GameObject>();

        private void Awake()
        {
            instance = this;

            InitPool();
        }

        public void InitPool()
        {
            for (int i = 0; i < shadowCount; i++)
            {
                var newShade = Instantiate(shadowPrefab, transform);

                ReturnPool(newShade);
            }
        }

        public void ReturnPool(GameObject gameObject)
        {
            gameObject.SetActive(false);

            activeObjects.Enqueue(gameObject);
        }

        public GameObject GetFromPool()
        {
            if (activeObjects.Count == 0)
            {
                InitPool();
            }
            var outShadow = activeObjects.Dequeue();

            outShadow.SetActive(true);

            return outShadow;
        }

    }
}

