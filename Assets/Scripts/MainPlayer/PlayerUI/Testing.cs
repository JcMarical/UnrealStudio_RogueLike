using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainPlayer
{
    public class Testing : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.Instance.realPlayerSpeed++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Player.Instance.realLucky++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.Instance.realUnlucky++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log(Player.Instance.realPlayerSpeed);
                Debug.Log(Player.Instance.realLucky);
                Debug.Log(Player.Instance.realUnlucky);
                Debug.Log(Player.Instance.realPlayerHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                IncreaseHealth(5f);
                Debug.Log(Player.Instance.realPlayerHealth);
                Debug.Log(Player.Instance.realMaxHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ReduceHealth(5f);
                Debug.Log(Player.Instance.realPlayerHealth);
                Debug.Log(Player.Instance.realMaxHealth);
            }

            if(Input.GetKeyDown (KeyCode.Alpha7))
            {
                Player.Instance.realMaxHealth += 20;
                Debug.Log(Player.Instance.realPlayerHealth);
                Debug.Log(Player.Instance.realMaxHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Player.Instance.realMaxHealth -= 20;
                Debug.Log(Player.Instance.realPlayerHealth);
                Debug.Log(Player.Instance.realMaxHealth);
            }
        }

        public void ReduceHealth(float health)
        {
            Player.Instance.realPlayerHealth -= health;
        }

        public void IncreaseHealth(float health)
        {
            Player.Instance.realPlayerHealth += health;
        }

    }
}
