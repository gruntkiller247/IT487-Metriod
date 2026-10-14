
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] bool canMove = true;
    [SerializeField] bool canMoveVeritcal = true;

    private Collider currentRoomCollider;

    private  Vector3 roomExtents;

    private Camera cam;

    private PlayerState ps;

    private bool colliderUpated = false;

    private bool roomBound = true;

    public static PlayerCamera instance;

    [SerializeField] string colliderName;
    
    private bool cameraLock = false;

    float camX;
    float camY;

    [SerializeField] float lpsx;  //last player spot X
    [SerializeField] float lpsy;  //last player spot Y
    void Start()
    {
       
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        if(player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        cam = GetComponent<Camera>();

        ps = player.GetComponentInParent<PlayerState>();

        camX = transform.position.x;
        camY = transform.position.y;



    }

    // Update is called once per frame
    void Update()
    {
        if(canMove == true)
        {
            lpsx = player.transform.position.x;
        }

        if(canMoveVeritcal == true)
        {
            lpsy = player.transform.position.y;
        }
        
       
    }

    void LateUpdate()
    {
        Transform playerLocation = player.transform;
        

        if(cameraLock == true)
        {
            //Let the door camera controls move the camera!
            return;
        }



        if (ps.getPlayerCanMove() == false || roomExtents == null)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);
        }
        else if(colliderUpated == true)
        {
            camX = currentRoomCollider.bounds.min.x;    //Works somewhat. Need to add directional info
            camY = player.transform.position.y;
            colliderUpated = false;
        }
        else
        {
            //Need to move the camera with the player unless they are near the edge of the room horizontally or vertically
            float cameraHalfWidth = cam.orthographicSize * cam.aspect;
            colliderName = currentRoomCollider.name; //Debug info


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

            float cameraHalfHeight = cam.orthographicSize;

            if (player.transform.position.y + cameraHalfHeight >= currentRoomCollider.bounds.max.y)
            {
                canMoveVeritcal = false;
            }
            else if (player.transform.position.y - cameraHalfHeight <= currentRoomCollider.bounds.min.y)
            {

                canMoveVeritcal = false;
            }
            else
            {
                canMoveVeritcal = true;
            }

            if (canMoveVeritcal)
            {
                camY = playerLocation.position.y;
            }
            else
            {
                //Debug.Log("Camera cannot move vertically!");
                camY = currentRoomCollider.bounds.center.y;
                //Debug.Log("In else Y");
            }

            if (canMove)
            {
                camX = playerLocation.position.x;
                
            }
            else
            {
                //camX = currentRoomCollider.bounds.extents.x - currentRoomCollider.bounds.center.x;
                //camX = currentRoomCollider.bounds.center.x;
                //Debug.Log("In else X");
                if(currentRoomCollider.bounds.min.x + cameraHalfWidth >= currentRoomCollider.bounds.max.x - cameraHalfWidth)
                {
                    //The camera is in a smaller room than its' own size
                    //Set the camX to the room's center
                    camX = currentRoomCollider.bounds.center.x;
                }
                 
                
            }

            transform.position = new Vector3(camX, camY, -10);
        }

        /*
        if (roomBound == true)
        {
            //colliderName = currentRoomCollider.name;
            if (colliderUpated == true)
            {
                //Debug.Log("\nCollider updated!");
                colliderUpated = false;
                
                if (currentRoomCollider.bounds.extents.x < (cam.orthographicSize * cam.aspect))
                {
                    //Debug.Log("\nCAMERA IN IF!");
                    //transform.position = new Vector3(currentRoomCollider.bounds.center.x, transform.position.y, -10);
                }
                else
                {
                    //Debug.Log("\nCAMERA IN ELSE!");
                    //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                }
                

            }
            else
            {
                if (ps.getPlayerCanMove() == false || roomExtents == null)
                {
                    transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);
                }
                else
                {

                    float cameraHalfWidth = cam.orthographicSize * cam.aspect;


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

                }

                float cameraHalfHeight = cam.orthographicSize;

                if (player.transform.position.y + cameraHalfHeight >= currentRoomCollider.bounds.max.y)
                {

                    canMoveVeritcal = false;
                }
                else if (player.transform.position.y - cameraHalfHeight <= currentRoomCollider.bounds.min.y)
                {

                    canMoveVeritcal = false;
                }
                else
                {
                    canMoveVeritcal = true;
                }


                if (canMoveVeritcal)
                {
                    camY = playerLocation.position.y;
                    //camY = currentRoomCollider.bounds.center.y;
                }
                else
                {
                    //Debug.Log("Camera cannot move vertically!");
                    camY = currentRoomCollider.bounds.center.y;
                }

                if (canMove)
                {
                    camX = playerLocation.position.x;
                    //camX = currentRoomCollider.bounds.center.x;
                    
                }
                else
                {
                    //camX = currentRoomCollider.bounds.extents.x - currentRoomCollider.bounds.center.x;
                    camX = lpsx;
                }

                //Debug.Log("Normal Camera Movement!\n\n");

                transform.position = new Vector3(camX, camY, -10);
            }
        }
        else
        {
            Debug.Log("Room Bound False Camera Movement!");
            transform.position = new Vector3(playerLocation.position.x, playerLocation.position.y, -10);
        }
        */
        
        
    }

    public void giveRoomColliderToCamera(Collider other)
    {
       
        Debug.Log("I have been given info from the Camera Helper!");
        
        roomExtents = other.bounds.extents;
        currentRoomCollider = other;
        colliderUpated = true;
        
        //Debug.Log("The room's extents are: " + extents);
    }

    public void SetRoomBound(bool inVal)
    {
        roomBound = inVal;
    }

    public bool GetRoomBound()
    {
        return roomBound;
    }

    public void SetCameraLock(bool inVal)
    {
        cameraLock = inVal;
    }

    public bool GetCameraLock()
    {
        return cameraLock;
    }
}
