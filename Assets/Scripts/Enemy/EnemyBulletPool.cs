using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    public int maxSize = 10000;
    public GameObject bullet;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 10, maxSize);
    }

    public GameObject CreateFunc()
    {
        GameObject obj = Instantiate(bullet, transform);
        obj.GetComponent<PooledBullet>().pool = pool;
        return obj;
    }

    public void ActionOnGet(GameObject obj) => obj.SetActive(true);

    public void ActionOnRelease(GameObject obj) => obj.SetActive(false);

    public void ActionOnDestroy(GameObject obj) => Destroy(obj);

    public GameObject CreateBullet(Vector3 position)
    {
        GameObject b = pool.Get();
        b.transform.position = position;
        return b;
    }
}
