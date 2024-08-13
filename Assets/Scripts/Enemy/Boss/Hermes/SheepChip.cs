using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赫尔墨斯羊的筹码
/// </summary>
public class SheepChip : MonoBehaviour
{
    public float speed;
    public Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().Play("Hit");
        speed = 0;
    }

    public void Initialized(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction;
    }

    public void DestroyGameobject() => Destroy(gameObject);
}
