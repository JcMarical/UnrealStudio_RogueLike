// �����˺��ӿ�
using UnityEngine.Rendering;

public interface IDamageable
{
    void GetHit(float damage,float IncreasedInjury);
    void Repelled(float force,string type);
}
