using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
using System;
using NUnit.Framework.Internal;

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

    static int doorCount = 0;

    private PlayerWeapon pw;

    private PlayerCamera pc;

    private Rigidbody playerRigid;

    private SpriteRenderer playerSR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        solidCol = GetComponent<Collider>();

        player = GameObject.FindGameObjectsWithTag("The Player")[0];

        if(player == null)
        {
            Debug.LogError("Door: Player Null!");
        }

        playerSR = player.GetComponentInChildren<SpriteRenderer>();

        if(playerSR == null)
        {
            Debug.LogError("Door does not have the Player's Sprite!");
        }
        else
        {
            tempSprite = playerSR.sprite;
        }

        ps = player.GetComponentInParent<PlayerState>();
        

        if(ps == null)
        {
            Debug.LogError("Door does not have the Player's State!");
        }

        pw = player.GetComponentInChildren<PlayerWeapon>();

        if(pw == null)
        {
            Debug.LogError("Door does not have the Player's weapon!");
        }

        playerRigid = player.GetComponent<Rigidbody>();

        if(playerRigid == null)
        {
            Debug.LogError("Door does not have the Player's Rigid!");
        }

        if(doorSprite == null)
        {
            doorSprite = sr.sprite;
        }

        cam = Camera.main;

        pc = cam.GetComponent<PlayerCamera>();

        if(pc == null)
        {
            Debug.Log("Door does not have the Player Camera!");
        }
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
            {
                StartCoroutine(StartChangeRoom());
                doorCount = 0;
            }
                
        }

    }

    private IEnumerator StartChangeRoom()
    {
        changingRoom = true;
        ps.setPlayerCanMove(false);
        pc.SetRoomBound(false);

        if(pw != null)
            pw.SetPlayerCanShoot(false);

        playerRigid.linearVelocity = Vector3.zero;

        pc.SetCameraLock(true);


        //Disable the player's sprite!
        if(playerSR != null)
        {
            playerSR.sprite = null;
        }

        postDoor = new Vector3(player.transform.position.x + movePlayerX,player.transform.position.y + movePlayerY,0);
        movePlayerToSpot();
        yield return StartCoroutine(moveCameraToSpot());

        //Reenable the player's sprite!
        if(playerSR != null)
        {
            playerSR.sprite = tempSprite;
        }
        
        changingRoom = false;
        ps.setPlayerCanMove(true);
        pc.SetRoomBound(true);

        if(pw != null)
            pw.SetPlayerCanShoot(true);

        pc.SetCameraLock(false);
        
        /*
        foreach (Collider box in player.GetComponentsInChildren<Collider>())
        {
            box.enabled = true;
        }
        */
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

    private IEnumerator moveCameraToSpot()
    {
        float duration = 2.0f; // Duration of the movement in seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Move the camera
            cam.transform.position = Vector2.Lerp(transform.position, new Vector3(postDoor.x,transform.position.y,-10), elapsedTime / duration);
            //Z axis is ignored in the above! Using Vector2
            cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,-10);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        /*
        }
            Vector3 postDoor = new Vector3(cam.transform.position.x + movePlayerX,cam.transform.position.y + movePlayerY,0);
            yield return null;
        }
        */
    }
    

    private void movePlayerToSpot()
    {
        /*
        GameObject activePlayerState = GameObject.FindGameObjectsWithTag("Player")[0];
        SpriteRenderer pSR = activePlayerState.GetComponent<SpriteRenderer>();
        Rigidbody pRigid = player.GetComponent<Rigidbody>();
        */

        player.transform.position = postDoor; //Vector3.MoveTowards(player.transform.position, postDoor, 0.02f);
    }
}
