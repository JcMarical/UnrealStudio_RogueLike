using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Buckler", menuName = "Data/ObtainableObjects/Func/Buckler", order = 13)]
public class PFunc_Buckler : PropFunc
{
    private GameObject player;
    [SerializeField] private GameObject Shield;
    public override void OnAwake()
    {
        base.OnAwake();
        player = Player.Instance.gameObject;
        UseProp();
    }

    public override void UseProp()
    {
        base.UseProp();
        if (!GameObject.Find("PlayerShield"))
        {
            GameObject newShield = Instantiate(Shield, player.transform.position, Quaternion.identity);
            newShield.name = "PlayerShield";
            newShield.transform.SetParent(player.transform);
            newShield.AddComponent<PlayerShield>();
        }
        //TODO:¹¥»÷·¶Î§Ôö¼Ó0.1
    }

    public override void Finish()
    {
        base.Finish();
    }
}
