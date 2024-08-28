using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public GameObject[] enemiesForSelect=new GameObject[3];
    public int isSelected;
    public int value;
    void Start() 
    {
        Sequence mySequence=DOTween.Sequence();
        foreach(GameObject p in enemiesForSelect){
            mySequence.Insert(0,DOTweenModuleSprite.DOFade(p.GetComponent<SpriteRenderer>(), 0, 2f)); 
        }
        mySequence.AppendCallback(()=>{
            switch(value){
                case 1:
                    Debug.Log(1);
                    break;
                case 2:
                    Debug.Log(2);
                    break;
                case 3:
                    Debug.Log(3);
                    break;
            }
            isSelected=value;
        });
        mySequence.Play();
        Debug.Log(isSelected);
    }
}
