using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RarityandProbabilityofStorePerLayer
{
    public Rarities minRarity;//��ǰ���ܻ�õ���Ʒ�����ϡ�ж�
    public Rarities maxRarity;//��ǰ���ܻ�õ���Ʒ�����ϡ�ж�
    public Dictionary<Rarities, float> Probability;//��ǰ��ÿ��ϡ�жȵĸ��� key��ϡ�ж� value������

    public RarityandProbabilityofStorePerLayer(Rarities minRarity,Rarities maxRarity, Dictionary<Rarities, float> Probability)
    {
        this.minRarity = minRarity;
        this.maxRarity = maxRarity;
        this.Probability = Probability;
    }

    public float GetChanceByRarity(Rarities Rarity)
    {
        if ((int)Rarity <= (int)maxRarity)
        {
            return Probability[Rarity];
        }
        else return -1;
    }
};

public class GameManager : TInstance<GameManager>
{
    public RarityandProbabilityofStorePerLayer[] RAP;//�̵�ÿ���ϡ�жȺ͸��ʣ���дΪRAP��ע�⣺RAP[0]��ռλ������Ӧ��ʹ�ã�����
    public int CurrentLayer = 1;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���ص�ǰ���̵������Ʒ��ϡ�жȺ͸���
    /// </summary>
    /// <returns></returns>
    public RarityandProbabilityofStorePerLayer GetCurrentRAP()
    { 
        return RAP[CurrentLayer];
    }
}
