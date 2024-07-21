using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISS
{
    #region 异常状态函数接口
    public void SS_Hot(float harm) { }//炎热 参数代表伤害
    public void SS_Freeze(float percent) { }//寒冷 参数代表武器间隔延长时间比例
    public void SS_Fixation() { }//定身
    public void SS_Confuse() { }//混淆
    public void SS_Sticky(float percent) { }//粘滞 参数代表人物速度减少比例
    public void SS_Burn(float harm) { }//燃烧 参数代表伤害
    public void SS_Clog(float percent) { }//阻塞 参数代表人物速度减少比例、
    public void SS_Dizzy() { }//抢注
    public void SS_Hurry(float percent) { }//急步 参数代表人物速度增加比例
    public void SS_Blind(float radius) { }//致盲 参数为生成圆的半径
    public void SS_Charm(Transform target, float speed) { }//魅惑 第一个参数为发动该异常效果物体的位置，第二个为人物向该物体移动时的速度
    public void SS_Invincible() { }//无敌
    #endregion
}

