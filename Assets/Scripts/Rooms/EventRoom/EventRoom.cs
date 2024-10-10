using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoom : MonoBehaviour
{
    public Transform centerPosition;

    public Vector2 playerDetectSize = new Vector2(24, 14);
    public LayerMask playerLayer = 1 << 6;
    public bool isHappened;
    private float timer = 0f;
    private bool isWaiting = false;

    private void Update()
    {
        if (Physics2D.OverlapBox(centerPosition.position, playerDetectSize, 0, playerLayer) && !isHappened)
        {
            isHappened = true;
            EventRoomMgr.Instance.currentRoom = this;
            isWaiting = true;
            timer = 0f;
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                EventRoomMgr.Instance.EnterEvent();
                isWaiting = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerPosition.position, playerDetectSize);
    }
}
