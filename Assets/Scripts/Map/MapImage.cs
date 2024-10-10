using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapImage : MonoBehaviour
{
    public GameObject m_Sprite;

    private void Start()
    {
        m_Sprite.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_Sprite.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            m_Sprite.SetActive(false);
        }
    }
}
