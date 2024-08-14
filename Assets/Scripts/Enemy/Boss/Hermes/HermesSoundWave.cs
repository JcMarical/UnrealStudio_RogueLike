using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HermesSoundWave : PooledBullet
{
    public float speed;
    public Vector2 direction;
    public float duration;
    public bool canTrack;
    public GameObject player;

    private float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            pool.Release(gameObject);

        if (canTrack)
        {
            direction = (player.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    public void Initialize(float speed, Vector2 direction, float duration, bool canTrack)
    {
        this.speed = speed;
        this.direction = direction;
        this.duration = duration;
        this.canTrack = canTrack;
        timer = duration;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
    }
}
