using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject gameObject;
    private void Update() {
        gameObject.GetComponent<Weapon>().Attack();
    }
}
