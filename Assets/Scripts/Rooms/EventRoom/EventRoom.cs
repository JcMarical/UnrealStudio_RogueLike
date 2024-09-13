using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoom : MonoBehaviour
{
    public Transform zeroPosition;
    public Transform centerPosition;
    public Vector3[,] positions;
    public List<Vector3> validPositionsList;

    public Vector2 playerDetectSize = new Vector2(14, 8);
    public LayerMask playerLayer = 1 << 6;
    public bool isHappened;

    private void Awake()
    {
        positions = new Vector3[14, 8];

        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                positions[i, j] = zeroPosition.position + new Vector3(i, j, 0);
                validPositionsList.Add(positions[i, j]);
            }
        }

        validPositionsList.Remove(positions[1, 1]);
        validPositionsList.Remove(positions[1, 2]);
        validPositionsList.Remove(positions[2, 1]);
        validPositionsList.Remove(positions[12, 1]);
        validPositionsList.Remove(positions[12, 2]);
        validPositionsList.Remove(positions[11, 1]);
        validPositionsList.Remove(positions[1, 6]);
        validPositionsList.Remove(positions[1, 5]);
        validPositionsList.Remove(positions[2, 6]);
        validPositionsList.Remove(positions[12, 6]);
        validPositionsList.Remove(positions[12, 5]);
        validPositionsList.Remove(positions[11, 6]);
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(centerPosition.position, playerDetectSize, 0, playerLayer) && !isHappened)
        {
            isHappened = true;
            EventRoomMgr.Instance.currentRoom = this;
            EventRoomMgr.Instance.EnterEvent();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerPosition.position, playerDetectSize);
    }
}
