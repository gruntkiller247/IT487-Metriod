using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasMorphBall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MorphBall")
        {
            Destroy(other.GameObject());
            hasMorphBall = !hasMorphBall;
        }
    }

    public bool HasMorphBall()
    {
        return hasMorphBall;
    }
}
