using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;
    SpriteRenderer mapSpriteRenderer;

    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(0).gameObject;
        mapSpriteRenderer = mapSprite.GetComponent<SpriteRenderer>();
        mapSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
            mapSpriteRenderer.color = Color.red;
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
