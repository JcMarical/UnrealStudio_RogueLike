using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Shield", menuName = "Data/ObtainableObjects/Func/Shield", order = 4)]
public class PFunc_Shield : PropFunc
{
    public Sprite SheildSprite;
    public GameObject Sheild;
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void UseProp()
    {
        base.UseProp();
        if(!PlayerShield.Instance) Instantiate(Sheild,Player.Instance.gameObject.transform.position,Quaternion.identity).AddComponent<PlayerShield>();
    }

    public override void Finish()
    {
        base.Finish();
    }
}

public class PlayerShield : TInstance<PlayerShield>
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Resist(GameObject WhoAttackMe)
    {
        if (WhoAttackMe.CompareTag("enemyattack"))
        {
            Destroy(WhoAttackMe);
        }
        Destroy(gameObject);
    }
}
