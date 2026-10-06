using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool canMove = true;

    [SerializeField] float distanceFromWall = 16f;

    private Collider currentRoomCollider;

    private  Vector3 roomExtents;

    LayerMask layerMask;

    private Camera cam;
    
    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");

        if(player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        cam = GetComponent<Camera>();
        
        

        //Player loc is the location we need to compare for the camera
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
           Debug.Log("Hit a wall!");
        }
        */
    }

    void LateUpdate()
    {
        Transform playerLocation = player.transform;
        
        //Camera width = 16
        //Camera height = 16
        if(roomExtents == null)
        {
            transform.position = new Vector3(player.transform.position.x,transform.position.y,-10);
        }
        else
        {
            //Compare the extents to the width/height restirction of the camera Currently 16 See above
            //Then prevent the camera from moving once it is within the distance
            //Debug.Log(extents);

            //THE EXTENTS ARE POSITION IGNORANT!
            //Extents are also mesaured from the center of the room
            //
            //current pos + extent.x 16 width  >= current pos
            //If yes I am close to the edge of the room and the camera should stand still! 


            //At the upper edge of the room


            //How to calculate the - or + dynamically?

            float cameraHalfWidth = cam.orthographicSize * cam.aspect;
            //Debug.Log("Camera halfWidth: " + cameraHalfWidth);

            //Debug.Log("Player x location: " + (playerLocation.position.x));
            //Debug.Log("extents: " + roomExtents.x);
            //Debug.Log("Formula on right: " + ((roomExtents.x*2) - cameraHalfWidth));


            if (playerLocation.position.x + cameraHalfWidth >= currentRoomCollider.bounds.max.x)
            {
                //Debug.Log("To close to the Right Edge!");
                canMove = false;
            }
            else if (playerLocation.position.x - cameraHalfWidth <= currentRoomCollider.bounds.min.x)
            {
                //Debug.Log("To close to the Left Edge!");
                canMove = false;
            }
            else
            {
                canMove = true;
            }

            /*
            if(playerLocation.position.x >= ((roomExtents.x*2) - cameraHalfWidth))
            {
                
                canMove = false;
            }
            else if(playerLocation.position.x <= ((roomExtents.x/2) + cameraHalfWidth))
            {
                 
                canMove = false;
            }
            else
            {
                canMove = true;
            }
            */
            
        }

        if(canMove)
        {
            transform.position = new Vector3(player.transform.position.x,transform.position.y,-10);
        }

        
        
    }

    public void giveRoomColliderToCamera(Collider other)
    {
       
        //Debug.Log("I have been given info from the Camera Helper!");
        
        roomExtents = other.bounds.extents;
        currentRoomCollider = other;
        
        //Debug.Log("The room's extents are: " + extents);
    }
}
