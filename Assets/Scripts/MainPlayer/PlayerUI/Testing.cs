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
                PlayerAttributesUI.attributesChanging(Player.Instance.realPlayerSpeed,"MoveSpeed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Player.Instance.realLucky++;
                PlayerAttributesUI.attributesChanging(Player.Instance.realLucky, "Lucky");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.Instance.realUnlucky++;
                PlayerAttributesUI.attributesChanging(Player.Instance.realUnlucky, "Unlucky");
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
                ReduceHealth(5f);
                Debug.Log(Player.Instance.realPlayerHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                IncreaseHealth(5f);
                Debug.Log(Player.Instance.realPlayerHealth);
            }
        }

        void ReduceHealth(float health)
        {
            Player.Instance.realPlayerHealth -= health;
            PlayerItemsUI.healthDown(health);
        }

        void IncreaseHealth(float health)
        {
            Player.Instance.realPlayerHealth += health;
            PlayerItemsUI.healthUp(health);   
        }

    }
}
