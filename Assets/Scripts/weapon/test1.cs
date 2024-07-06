using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    private void FixedUpdate() {
        transform.position+=new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0)*0.1f;
    }
}
