using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    private PlayerCamera playerCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Room")
        {
            Debug.Log("Still inside"); 
        }
        
           
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Room")
        {
            Debug.Log("Entered a new Room trigger!");
        }
    }
}
