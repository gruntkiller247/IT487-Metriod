
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool canMove = true;
    private bool canMoveVeritcal = true;

    private Collider currentRoomCollider;

    private  Vector3 roomExtents;

    private Camera cam;

    private PlayerState ps;

    private bool colliderUpated = false;

    private bool roomBound = true;

    public static PlayerCamera instance;

    [SerializeField] string colliderName;
    
    private bool cameraLock = false;

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

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Transform playerLocation = player.transform;
        float camX = transform.position.x;
        float camY = transform.position.y;

        if(cameraLock == true)
        {
            //Let the door camera controls move the camera!
            return;
        }


        if (roomBound == true)
        {
            colliderName = currentRoomCollider.name;
            if (colliderUpated == true)
            {
                //Debug.Log("\nCollider updated!");
                colliderUpated = false;
                if (currentRoomCollider.bounds.extents.x < (cam.orthographicSize * cam.aspect))
                {
                    //Debug.Log("\nCAMERA IN IF!");
                    transform.position = new Vector3(currentRoomCollider.bounds.center.x, transform.position.y, -10);
                }
                else
                {
                    //Debug.Log("\nCAMERA IN ELSE!");
                    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
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
                    //Debug.Log("Camera cannot move Horizontally!");
                    //Need to move the camera to the correct X extent either + or -
                    if(player.transform.position.x < currentRoomCollider.bounds.center.x)
                    {
                        //Closer to the left
                    }
                    else
                    {
                        //Closer to the right

                    }
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
        
        
    }

    public void giveRoomColliderToCamera(Collider other)
    {
       
        //Debug.Log("I have been given info from the Camera Helper!");
        
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
