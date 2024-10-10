using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBackGround : MonoBehaviour
{
    public GameObject mapSprite;
    public bool isStartRoom;
    SpriteRenderer mapSpriteRenderer;

    private void Start()
    {
        mapSpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
        if (isStartRoom)
        {
            mapSprite.SetActive(true);
        }
        else { mapSprite.SetActive(false); }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
            mapSpriteRenderer.color = Color.blue;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapSpriteRenderer.color = Color.white; // 将颜色恢复为白色
        }
    }
}
