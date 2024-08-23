using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Data/ObtainableObjects/Resource", order = 0)]
public class Resource_Data : ObtainableObjectData
{
    public event Action OnResourceChanged; 

    [Header("数据持久化")]
    public int Amount;
    public int TakeOutCost =1;

    public void GainResource(int amount)
    {
        Amount += amount;
        OnResourceChanged?.Invoke();
    }

    public bool CostResource(int amount)
    {
        if (Amount >= amount)
        {
            Amount -= amount;
            OnResourceChanged?.Invoke();
            return true;
        }
        return false;
    }
}