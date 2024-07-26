using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Enemytest : MonoBehaviour
{
    void Update()
    {
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
    }
}
