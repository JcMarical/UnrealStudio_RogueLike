using UnityEngine;
class ConstField:W_TInstance<ConstField>
{
    public float LengthPerCeil=1;
    public float DeviationOfVelocity=0.05f;//判断物体静止的速度误差
    public string EnemyTag="Enemy";//敌人tag
}
