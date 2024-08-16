using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class BuffObstacles : MonoBehaviour
{
    public PlayerSS_FSM playerSSFSM;
    public string buff;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerSSFSM=collision.GetComponent<PlayerSS_FSM>();
            playerSSFSM.AddState("SS_Acide", 2);
        }
    }
}
