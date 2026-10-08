using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
using System;

public class Door : MonoBehaviour
{
    [SerializeField] bool doorOpen = false;
    [SerializeField] Sprite doorSprite;

    SpriteRenderer sr;

    [SerializeField] Collider solidCol;

    [SerializeField] float doorOpenTime = 3f;

    private GameObject player;

    private bool changingRoom = false;

    private PlayerState ps;

    [SerializeField] float movePlayerX = 4f;    //Variables to tell the door where to teleport the player to after the movement scene
    [SerializeField] float movePlayerY = 0f;

    private Collider thePlayerCollider;

    private Sprite tempSprite = null;

    [SerializeField] Camera cam;

    [SerializeField] float timeForCameraToMove = 4f;

    public bool isMissileDoor = false;

    private Vector3 postDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        solidCol = GetComponent<Collider>();

        player = GameObject.FindGameObjectsWithTag("The Player")[0];

        if(player == null)
        {
            Debug.LogError("Player Null!");
        }

        ps = player.GetComponentInParent<PlayerState>();
        

        if(ps == null)
        {
            Debug.LogError("Camera does not have the Player's State!");
        }

        //lm = LayerMask.GetMask("Door");

        if(doorSprite == null)
        {
            doorSprite = sr.sprite;
        }

        cam = Camera.main;

        //The width of 1 block is 6 units. need to do this to acutally move 1 block per second in the transition
        //directionToMovePlayer *= 6; 
    }

    // Update is called once per frame
    void Update()
    {
        if(doorOpen)
        {
            sr.sprite = null;
        }
        else
        {
            sr.sprite = doorSprite;
        }

       if (doorOpen && Mathf.Abs(transform.position.x - player.transform.position.x) < 0.1f)
        {
            //Debug.Log("Player is in position to start the door transition!");

            if(changingRoom == false && ps.getPlayerCanMove())
                StartCoroutine(StartChangeRoom());
        }

    }

    private IEnumerator StartChangeRoom()
    {
        changingRoom = true;
        ps.setPlayerCanMove(false);
        player.GetComponent<Rigidbody>().isKinematic = true;
        PlayerCamera.instance.roomBound = false;
        PlayerWeapon.canShoot = false;
        
        foreach (Collider box in player.GetComponentsInChildren<Collider>())
        {
            box.enabled = false;
        }
        Coroutine changeRoom = StartCoroutine(ChangeRoom());

        yield return new WaitForSeconds(2);
        StopCoroutine(changeRoom);

        changingRoom = false;
        ps.setPlayerCanMove(true);
        player.GetComponent<Rigidbody>().isKinematic = false;
        PlayerCamera.instance.roomBound = true;
        PlayerWeapon.canShoot = true;
        
        foreach (Collider box in player.GetComponentsInChildren<Collider>())
        {
            box.enabled = true;
        }
        yield return null;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("DoorOpen: " + doorOpen);

        if(other.gameObject.tag == "PlayerWeapon")
        {
            //Change the state to open door
            //Do this with a coroutine for some amount of time
            //Debug.Log("Player Bullet!");
            if (isMissileDoor)
            {
                if (other.gameObject.GetComponent<Missile>())
                {
                    startDoorOpen();
                }
                Destroy(other.gameObject);
            }
            else
            {
                startDoorOpen();
                Destroy(other.gameObject);
            }
            
            
        }
    }

    public void startDoorOpen()
    {
        if(doorOpen == false)
        {
            StartCoroutine(DoorOpen());
            
        }
            

    }

    private IEnumerator DoorOpen()
    {
        RaycastHit hit;

        //Debug.Log("In doorOpen");
        doorOpen = true;
        solidCol.enabled = false;

        if(Physics.Raycast(transform.position, Vector3.up, out hit, 2f))
        {
            if(hit.transform.tag == "Door")
            {
                //Debug.Log("There is a door object above me!");
                hit.collider.gameObject.GetComponent<Door>().startDoorOpen();
            }

        }

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if(hit.transform.tag == "Door")
            {
                //Debug.Log("There is a door object below me!");
                hit.collider.gameObject.GetComponent<Door>().startDoorOpen();
            }
        }

        //Debug.Log("\n");

        yield return new WaitForSeconds(doorOpenTime);
        
        doorOpen = false;
        solidCol.enabled = true;
    }

    private IEnumerator ChangeRoom()
    {
        
        postDoor = new Vector3(player.transform.position.x + movePlayerX,player.transform.position.y + movePlayerY,0);
        while (true)
        {
            movePlayerToSpot();
            moveCameraToSpot();
            yield return new WaitForSeconds(1 / 60);
        }
        //Change this to add the camera's movement at some point!
        


        
        //Debug.Log("CanMove: " + ps.getPlayerCanMove() + "\nVelocity: " + player.GetComponent<Rigidbody>().linearVelocity + "\nPosition: " + player.transform.position);
        //Debug.Log("\n");
    }

    private void moveCameraToSpot()
    {
        Vector3 postDoor = new Vector3(cam.transform.position.x + movePlayerX,cam.transform.position.y + movePlayerY,0);
    }

    private void movePlayerToSpot()
    {
        
        GameObject activePlayerState = GameObject.FindGameObjectsWithTag("Player")[0];
        SpriteRenderer pSR = activePlayerState.GetComponent<SpriteRenderer>();
        Rigidbody pRigid = player.GetComponent<Rigidbody>();
        /*if(tempSprite == null)
        {
            tempSprite = pSR.sprite;
        }
        
        pSR.sprite = null;*/
        //player.GetComponent<Rigidbody>().isKinematic = false;


        //Camera cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        //cam.transform.position += directionToMovePlayer;
        Debug.Log("Moving player to this pos");
        player.transform.position = Vector3.MoveTowards(player.transform.position, postDoor, 0.02f);
        
        
        //pSR.sprite = tempSprite;

        

        //Fix the player!
        
        //player.GetComponent<Rigidbody>().isKinematic = true;

        //Debug.Log("Player Moving Coroutine over!");
    }
}
