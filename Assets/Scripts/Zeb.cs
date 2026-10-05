using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
public class Zeb : MonoBehaviour
{

    [SerializeField] int distanceFromPlayer = 2;
    [SerializeField] int distanceFromPlayerToDie = 20;
    public float moveSpeedUp = 1f;
    public float moveSpeedSideways = 0.5f;

    //private bool shouldMoveSideways = true;
    private Rigidbody rigid;

    private float lowView;
    private float highView;
    
    GameObject player;

    private bool runTowardsPlayer = false;

    private bool playerNear = false;

    private Vector3 playerDirection;

    private bool directionFound = false;

    void Awake()
    {
        rigid = transform.GetComponent<Rigidbody>();

        if(player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Should slowly move upwards until they "spot" the player, then move towards the player. Ignore anything it "sees" that isn't the player
        //Ideally the spawn range should small enough that it only sees the player near the pipe, so 1-3 blocks
        //Vector3 movement;
        //RaycastHit hit;
        
        lowView = transform.position.x - distanceFromPlayer;
        highView = transform.position.x + distanceFromPlayer;

        if(player != null && runTowardsPlayer == false && (player.transform.position.x <= highView || player.transform.position.x >= lowView ))
        {
            //Start moving upwards
            //If this ever becomes false it means the player has gone elsewhere away from me.
            //If that is the case I should just run into a wall and die
            playerNear = true;
        }

        if(playerNear)
        {
            //Start moving upwards until I reach the player's height
            if(gameObject.transform.position.y <= player.gameObject.transform.position.y)
            {
                StartCoroutine(moveUp());
            }
            else
            {
                runTowardsPlayer = true;
                playerNear = false;
            }
        }

        if(runTowardsPlayer && directionFound == false)
        {
            //Run towards the player
            if(player.gameObject.transform.position.x < transform.position.x)
            {
                //Move left
                playerDirection = Vector3.left;
                
            }
            else
            {
                //Move Right
                //rigid.AddForce(Vector3.right,ForceMode.Impulse);
                playerDirection = Vector3.right;
            }
            directionFound = true;
        }
        else
        {
            //Move in that direction
            rigid.linearVelocity = playerDirection * moveSpeedSideways;

            //distanceFromPlayerToDie
            //p.x = 20
            //Distance +-20
            //m.x = 10
            //-10 <-> 30
            //player.transform.position.x 
            if(player.transform.position.x > transform.position.x + distanceFromPlayerToDie || player.transform.position.x < transform.position.x - distanceFromPlayerToDie)
            {
                //Debug.Log("Killing myself!");
                //Debug.Log("Player: " + player.transform.position.x);
                //Debug.Log("Distance: " + distanceFromPlayerToDie);
                //Debug.Log("Greator: " + (distanceFromPlayerToDie+transform.position.x) + "\nLesser: " + (distanceFromPlayerToDie + transform.position.x));
                GetComponent<EntityHealth>().kill();
            }
        }
    }

    private IEnumerator moveUp()
    {
        Vector3 movement;
        movement = Vector3.up * moveSpeedUp * Time.fixedDeltaTime;
        rigid.Move(rigid.position + movement, rigid.rotation);
        yield return null;//new WaitForSeconds(waitTime);
    }
}
