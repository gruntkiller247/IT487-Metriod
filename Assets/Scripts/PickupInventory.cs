using UnityEngine;

public class PickupInventory : MonoBehaviour
{

    public int hpAmount = 0;
    public int missileAmount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHp(int inHp)
    {
        hpAmount=inHp;
    }

    public void setMissiles(int inMis)
    {
        missileAmount = inMis;
    }

    public int getHp()
    {
        return hpAmount;
    }

    public int getMissiles()
    {
        return missileAmount;
    }
    
}
