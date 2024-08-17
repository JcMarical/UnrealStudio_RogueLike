using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PFunc_Shield", menuName = "Data/ObtainableObjects/Func/Shield", order = 4)]
public class PFunc_Shield : PropFunc
{
    public GameObject Player;
    public GameObject Shield;
    public override void OnAwake()
    {
        base.OnAwake();
        Player = GameObject.Find("Player");
        Shield = Resources.Load<GameObject>("Prefabs/PlayerShield");
    }

    public override void UseProp()
    {
        base.UseProp();
        if (!GameObject.Find("PlayerShield"))
        {
            GameObject newShield = Instantiate(Shield,Player.transform.position ,Quaternion.identity);
            newShield.name = "PlayerShield";
            newShield.transform.SetParent(Player.transform);
            newShield.AddComponent<PlayerShield>();
        }
    }

    public override void Finish()
    {
        base.Finish();
    }
}

public class PlayerShield : MonoBehaviour
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
