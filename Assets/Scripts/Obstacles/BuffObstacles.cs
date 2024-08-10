using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffObstacles : MonoBehaviour
{
    public PlayerSS_FSM playerSSFSM;
    // Start is called before the first frame update
    void Start()
    {
        playerSSFSM=FindObjectOfType<PlayerSS_FSM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerSSFSM != null && collision.CompareTag("player"))
        {
            playerSSFSM.AddState(new SS_Sticky(), 10);
        }
    }
}
