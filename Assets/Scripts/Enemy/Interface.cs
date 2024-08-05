// 定义伤害接口
using UnityEngine.Rendering;

public interface IDamageable
{
    void GetHit(float damage);
    void Repelled(float force);
}
