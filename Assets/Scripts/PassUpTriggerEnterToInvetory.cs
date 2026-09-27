using UnityEngine;

public class PassUpTriggerEnterToInvetory : MonoBehaviour
{
    PlayerInventory playerInventory;
    public bool hasMorphBall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInventory = transform.GetComponentInParent<PlayerInventory>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        playerInventory.OnTriggerEnter(other);
    }


}
