using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

public class Door : MonoBehaviour
{
    [SerializeField] bool doorOpen = false;
    [SerializeField] Sprite doorSprite;

    private Sprite doorClosedSprite;

    SpriteRenderer sr;

    [SerializeField] Collider solidCol;

    [SerializeField] float doorOpenTime = 3f;

    private GameObject player;

    private bool changingRoom = false;

    [SerializeField] Vector3 directionToMovePlayer = Vector3.right;

    private PlayerState ps;

    [SerializeField] float movePlayerX = 4f;    //Variables to tell the door where to teleport the player to after the movement scene
    [SerializeField] float movePlayerY = 0f;

    private Collider thePlayerCollider;

    private Sprite tempSprite = null;

    //LayerMask lm;

    //[SerializeField] Sprite opened;
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

       if (Mathf.Abs(transform.position.x - player.transform.position.x) < 0.1f)
        {
            Debug.Log("Player is in position to start the door transition!");
            StartCoroutine(ChangeRoom());
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("DoorOpen: " + doorOpen);

        if(other.gameObject.tag == "PlayerWeapon")
        {
            //Change the state to open door
            //Do this with a coroutine for some amount of time
            //Debug.Log("Player Bullet!");

            startDoorOpen();
            
        }
    }

    public void startDoorOpen()
    {
        if(doorOpen == false)
            StartCoroutine(DoorOpen());
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
        changingRoom = true;
        ps.setPlayerCanMove(false);

        //Change this to add the camera's movement at some point!
        yield return StartCoroutine(movePlayerToSpot());



        ps.setPlayerCanMove(true);
        changingRoom = false;
        Debug.Log("Player should be able to move! ChangeRoom is over!");
    }


    private IEnumerator movePlayerToSpot()
    {
        
        GameObject activePlayerState = GameObject.FindGameObjectsWithTag("Player")[0];
        SpriteRenderer pSR = activePlayerState.GetComponent<SpriteRenderer>();

        if(tempSprite == null)
        {
            tempSprite = pSR.sprite;
        }
        
        pSR.sprite = null;
        //player.GetComponent<Rigidbody>().isKinematic = false;


        //Camera cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        //cam.transform.position += directionToMovePlayer;
        Vector3 postDoor = new Vector3(player.transform.position.x + movePlayerX,player.transform.position.y + movePlayerY,0);
        player.transform.position = postDoor;
        
        

        yield return null;

        //Fix the player!
        pSR.sprite = tempSprite;
        //player.GetComponent<Rigidbody>().isKinematic = true;

        //Debug.Log("Player Moving Coroutine over!");
    }
}
