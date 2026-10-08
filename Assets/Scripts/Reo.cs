using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reo : MonoBehaviour
{
    public enum State
    {
        idle,
        drop,
        jump
    }

    public float moveSpeed = 1f;

    public float moveSpeedDown = 1f;

    private float originalMoveSpeedDown = 1f;

    [SerializeField] int distanceFromPlayer = 2;

    private float lowView;
    private float highView;

    GameObject player;
    
    [SerializeField] State monState;

    private Rigidbody rigid;
    LayerMask lm;

    PlayerJump pj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("The Player")[0];
        monState = State.idle;

        rigid = transform.GetComponent<Rigidbody>();
        lm = LayerMask.GetMask("Door");
        pj = player.GetComponentInChildren<PlayerJump>();

        if(pj == null)
        {
            Debug.LogError("Reo cannot find Player Jump!");
        }

        originalMoveSpeedDown = moveSpeedDown;
    }

    // Update is called once per frame
    void Update()
    {
                
        lowView = transform.position.x - distanceFromPlayer;
        highView = transform.position.x + distanceFromPlayer;

        think();

        switch(monState)
        {
            case State.idle:
                //Do nothing!
                break;


            case State.drop:
                //Move towards Samus
                moveDown();

                break;

            case State.jump:
                moveUp();
                break;

            default:

                Debug.LogError("Reo does not have a legal Monster State!");
                break;
        }

    }

    private void think()
    {
        RaycastHit hit;
        if(monState == State.idle && player.transform.position.x <= highView || player.transform.position.x >= lowView )
        {
            //State change -> drop
            monState = State.drop;
            
        }
        else if(monState == State.idle)
        {
            //Do nothing. Sit and wait
        }
        else if(monState == State.drop)
        {
            //If player has jumped, state = jump
            //Otherwise keep swooping towards the player

            if(pj.IsJumping() == true)
            {
                monState = State.jump;
                moveSpeedDown = originalMoveSpeedDown;
            }
            
        }
        else if(monState == State.jump)
        {
            //Go until collide with ceiling. State = idle. Velocity = 0
            
            if(Physics.Raycast(transform.position,Vector3.up,out hit, lm))
            {
                Debug.Log("I hit the roof!");
                monState = State.idle;
                rigid.linearVelocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
            }
           

        }
    }

    private void moveDown()
    {
        Vector3 movement;



        if(transform.position.x - player.transform.position.x > 0)
        {
            //Move right
            movement = Vector3.left * moveSpeedDown * Time.fixedDeltaTime;
            rigid.Move(rigid.position + movement, rigid.rotation);

        }
        else
        {
            //move left
            movement = Vector3.right * moveSpeedDown * Time.fixedDeltaTime;
            rigid.Move(rigid.position + movement, rigid.rotation);
        }

        



        movement = Vector3.down * moveSpeedDown * Time.fixedDeltaTime;
        rigid.Move(rigid.position + movement, rigid.rotation);

        moveSpeed--;

        //yield return null;//new WaitForSeconds(waitTime);
    }

    private void moveUp()
    {
        Vector3 movement;
        movement = Vector3.up * moveSpeed * Time.fixedDeltaTime;
        rigid.Move(rigid.position + movement, rigid.rotation);
        //yield return null;//new WaitForSeconds(waitTime);
    }
}
