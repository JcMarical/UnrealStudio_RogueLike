using MainPlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
                Player.Instance.RealPlayerSpeed++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Player.Instance.RealLucky++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.Instance.RealUnlucky++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log(Player.Instance.RealPlayerSpeed);
                Debug.Log(Player.Instance.RealLucky);
                Debug.Log(Player.Instance.RealUnlucky);
                Debug.Log(Player.Instance.RealPlayerHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                IncreaseHealth(5f);
                Debug.Log(Player.Instance.RealPlayerHealth);
                Debug.Log(Player.Instance.RealMaxHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ReduceHealth(5f);
                Debug.Log(Player.Instance.RealPlayerHealth);
                Debug.Log(Player.Instance.RealMaxHealth);
            }

            if(Input.GetKeyDown (KeyCode.Alpha7))
            {
                Player.Instance.RealMaxHealth += 10;
                Debug.Log(Player.Instance.RealPlayerHealth);
                Debug.Log(Player.Instance.RealMaxHealth);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Player.Instance.RealMaxHealth -= 10;
                Debug.Log(Player.Instance.RealPlayerHealth);
                Debug.Log(Player.Instance.RealMaxHealth);
            }
        }

        public void ReduceHealth(float health)
        {
            Player.Instance.RealPlayerHealth -= health;
        }

        public void IncreaseHealth(float health)
        {
            Player.Instance.RealPlayerHealth += health;
        }

        public void Quit()
        {
            SaveSystem.SaveData(BindingChange.Instance.bindings, "/BindingDictionary.json");
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

    }
}
