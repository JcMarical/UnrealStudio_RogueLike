using UnityEngine;
class ConstField:TInstance<ConstField>
{
    public float LengthPerCeil=1;
    public float DeviationOfVelocity=0.5f;//判断物体静止的速度误差
    public string EnemyTag="Enemy";//敌人tag
}
