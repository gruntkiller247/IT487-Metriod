
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool canMove = true;
    private bool canMoveVeritcal = true;

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
        
        if(roomExtents == null)
        {
            transform.position = new Vector3(player.transform.position.x,transform.position.y,-10);
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

        if(player.transform.position.y + cameraHalfHeight >= currentRoomCollider.bounds.max.y)
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


        if(canMoveVeritcal)
        {
            camY = playerLocation.position.y;
        }
        
        if(canMove)
        {
            camX = playerLocation.position.x;
        }
        
        transform.position = new Vector3(camX, camY, -10);
    }

    public void giveRoomColliderToCamera(Collider other)
    {
       
        //Debug.Log("I have been given info from the Camera Helper!");
        
        roomExtents = other.bounds.extents;
        currentRoomCollider = other;
        
        //Debug.Log("The room's extents are: " + extents);
    }
}
