using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 老虎机的葡萄子弹
/// </summary>
public class SlotMachineGrape : MonoBehaviour
{
    private Animator animator;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timer = 2;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            animator.Play("Explode");
    }

    public void DestroyGameobject() => Destroy(gameObject);
}
