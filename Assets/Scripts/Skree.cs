using UnityEngine;

public class Skree : MonoBehaviour
{
    [SerializeField] int distanceFromPlayer = 5;
    public float moveSpeedDown = 1f;
    public float moveSpeedSideways = 0.5f;

    private bool shouldMoveSideways = true;
    private Rigidbody rigid;

    private float lowView;
    private float highView;
    
    GameObject player;

    private Collider col;

    void Awake()
    {
        rigid = transform.GetComponent<Rigidbody>();
        col = transform.GetComponent<Collider>();

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
        RaycastHit hit;
        Vector3 movement;
        lowView = transform.position.x - distanceFromPlayer;
        highView = transform.position.x + distanceFromPlayer;
        
        if(player != null && (player.transform.position.x <= highView || player.transform.position.x >= lowView ))
        {
            //Debug.Log("I should be allowed to move!");
            
            if(!Physics.Raycast(col.bounds.center,Vector3.down,out hit,0.5f))
            {
                movement = Vector3.down * moveSpeedDown * Time.fixedDeltaTime;
                rigid.Move(rigid.position + movement, rigid.rotation);
                
                if(Random.Range(0,2) == 1)
                    shouldMoveSideways = true;
                else
                    shouldMoveSideways = false;

                if(shouldMoveSideways)
                {
                    if(transform.position.x - player.transform.position.x > 0)
                    {
                        //Move right
                        movement = Vector3.left * moveSpeedSideways * Time.fixedDeltaTime;
                        rigid.Move(rigid.position + movement, rigid.rotation);

                    }
                    else
                    {
                        //move left
                        movement = Vector3.right * moveSpeedSideways * Time.fixedDeltaTime;
                        rigid.Move(rigid.position + movement, rigid.rotation);
                    }
                    //shouldMoveSideways = false;
                }
                //else
                    //shouldMoveSideways = true;
               
            }
            else
            {
                //Debug.Log("I have hit the ground!");
            }
            
        }

        //Next check if I have changed my Y position since the last 3 or so seconds
        //If no, shoot projectiles then die
    }
}
