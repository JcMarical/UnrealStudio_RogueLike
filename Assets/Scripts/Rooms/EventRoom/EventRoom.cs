using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoom : MonoBehaviour
{
    public Transform zeroPosition;
    public Vector3[,] positions;
    public List<Vector3> invalidPositionsList;

    private void Awake()
    {
        positions = new Vector3[14, 8];

        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 8; j++)
                positions[i, j] = zeroPosition.position + new Vector3(i, j, 0);
        }

        invalidPositionsList.Add(positions[1, 1]);
        invalidPositionsList.Add(positions[1, 2]);
        invalidPositionsList.Add(positions[2, 1]);
        invalidPositionsList.Add(positions[12, 1]);
        invalidPositionsList.Add(positions[12, 2]);
        invalidPositionsList.Add(positions[11, 1]);
        invalidPositionsList.Add(positions[1, 6]);
        invalidPositionsList.Add(positions[1, 5]);
        invalidPositionsList.Add(positions[2, 6]);
        invalidPositionsList.Add(positions[12, 6]);
        invalidPositionsList.Add(positions[12, 5]);
        invalidPositionsList.Add(positions[11, 6]);
    }
}
