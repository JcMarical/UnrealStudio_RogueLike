using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Bananapeel : TrapWeapon
{
    public override void TrapWeaponAttack()
    {
        Instantiate(gameObject,transform.position,Quaternion.identity).GetComponent<Collider2D>().enabled=true;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag(ConstField.Instance.EnemyTag)){
            other.transform.GetComponent<Enemy>().GetHit(10);
            other.transform.GetComponent<EnemySS_FSM>().AddState("SS_Sticky",3f);
        }
    }
}
