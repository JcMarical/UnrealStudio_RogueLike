using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MainPlayer;

[CreateAssetMenu(fileName = "PFunc_Shield", menuName = "Data/ObtainableObjects/Func/Shield", order = 4)]
public class PFunc_Shield : PropFunc
{
    public GameObject player;
    public GameObject Shield;
    public override void OnAwake()
    {
        base.OnAwake();
        player = Player.Instance.gameObject;
        Shield = Resources.Load<GameObject>("Prefabs/PlayerShield");
    }

    public override void UseProp()
    {
        base.UseProp();
        if (!GameObject.Find("PlayerShield"))
        {
            GameObject newShield = Instantiate(Shield, player.transform.position ,Quaternion.identity);
            newShield.name = "PlayerShield";
            newShield.transform.SetParent(player.transform);
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
