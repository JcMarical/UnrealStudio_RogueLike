using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_EnergeDrink", menuName = "Data/ObtainableObjects/Func/EnergeDrink", order = 6)]
public class PFunc_EnergeDrink : PropFunc
{
    public float EffectTime = 30;
    public override void OnAwake()
    {
        base.OnAwake();
        AwakeTime = Time.time;
    }

    public override void UseProp()
    {
        base.UseProp();
        Player.Instance.StartCoroutine(UseEnergyDrink());
    }

    private IEnumerator UseEnergyDrink()
    {
        PlayerBuffMonitor.Instance.MoveSpeedBuff = (Player.Instance.RealPlayerSpeed + 1) / Player.Instance.RealPlayerSpeed;
        yield return new WaitForSeconds(EffectTime);
        PlayerBuffMonitor.Instance.MoveSpeedBuff = (Player.Instance.RealPlayerSpeed - 1) / Player.Instance.RealPlayerSpeed;
    }

    public override void Finish()
    {
        base.Finish();
    }
}
