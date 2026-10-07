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

    [SerializeField] int doorAnimationTime = 2;        //The number of times to run the door transition function. For 4 blocks make it 4!

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
        //Debug.Log("Changing Room!");

        changingRoom = true;
        ps.setPlayerCanMove(false);
        //Debug.Log("PlayerMove: " + ps.getPlayerCanMove());

        yield return StartCoroutine(movePlayerToSpot(1));


        //Debug.Log("Changing Room Over!");
        ps.setPlayerCanMove(true);
        changingRoom = false;
        Debug.Log("Player should be able to move! ChangeRoom is over!");
    }

    //This function is dirty and I need to know a better way at some point!
    //I am afraid to look at the speed!
    private IEnumerator movePlayerToSpot(int count)
    {
        //Need to move the player 5 movement directions over the time given
        //Do not need to mess with the time and movement atm. Focus on the movement

        GameObject activePlayerState = GameObject.FindGameObjectsWithTag("Player")[0];
        //The above should give either the crouched or standing state of the player's gameobject
        
        
        //Get the child's stuff

        //Error? Might need to change this to check a bool if the player's sprite changes! Such as if this triggers in ball mode!
        SpriteRenderer pSR = activePlayerState.GetComponent<SpriteRenderer>();
        if(tempSprite == null)
        {
            tempSprite = pSR.sprite;
        }
        
        pSR.sprite = null;
        thePlayerCollider = activePlayerState.GetComponentInChildren<Collider>();

        if(thePlayerCollider != null)
        {
            thePlayerCollider.enabled = false;
        }


        //We disable the things in the parent to allow movement through the door
        player.GetComponent<Rigidbody>().isKinematic = true;


        Camera cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        player.transform.position += directionToMovePlayer;
        
        cam.transform.position += directionToMovePlayer;


        if(count < doorAnimationTime)
        {
            StartCoroutine(movePlayerToSpot(++count));
        }
        else
        {
            yield return null;
        }

        //Re enable/configer player
        pSR.sprite = tempSprite;
        
        if(thePlayerCollider != null)
        {
            thePlayerCollider.enabled = true;
        }

        player.GetComponent<Rigidbody>().isKinematic = false;

        //Debug.Log("Player Moving Coroutine over!");
    }
}
