using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerCamera = GetComponent<PlayerCamera>();

        if(playerCamera == null)
        {
            Debug.LogError("Did not find Player Camera in the Helper!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player is still within a room!");      
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Room")
        {
            Debug.Log("Entered a new Room trigger!");
            playerCamera.giveRoomColliderToCamera(other);
        }
    }

    public void OnTriggerExit()
    {
        Debug.Log("Player has left a room!");
    }
}
