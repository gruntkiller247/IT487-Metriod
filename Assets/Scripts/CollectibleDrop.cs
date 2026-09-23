using UnityEngine;

public class CollectibleDrop : MonoBehaviour
{

    public bool canDropMissiles = false;
    public bool canDropHealth = true;

    public int hpAmount = 5;

    public int missileAmount = 3;

    public GameObject hpDrop;

    public GameObject missileDrop;

    public bool alwaysDropHp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dropThing(Vector3 position)
    {
        
        int num;
        num = Random.Range(1, 11);

        if(num >= 5)
        {
            if(canDropMissiles)
            {
                if(num >= 5 && num < 8)
                {
                    dropHP(position);
                }
                else 
                {
                    dropMissiles(position);
                }
            }
            else
            {
                if(num >= 5)
                 dropHP(position);
            }
        }
    }

    void dropMissiles(Vector3 position)
    {
        Instantiate(missileDrop,position, Quaternion.identity);
    }

    void dropHP(Vector3 position)
    {
        Instantiate(hpDrop,position, Quaternion.identity);
    }

    public int getHpAmount()
    {
        return hpAmount;
    }

    public int getMissilesAmount()
    {
        return missileAmount;
    }
}
