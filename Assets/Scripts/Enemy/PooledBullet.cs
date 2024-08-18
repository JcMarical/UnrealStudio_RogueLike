using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 使用对象池管理的敌人子弹需要继承此类
/// </summary>
public class PooledBullet : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
}
