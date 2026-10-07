
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Rigidbody rigid;
    public float moveSpeed = 5f;
    public float moveAccel = 1f;
    public float moveFriction = 1f;
    public float jumpPower = 12f;

    PlayerState ps;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = transform.GetComponent<Rigidbody>();
        ps = transform.GetComponentInParent<PlayerState>();
        player = GameObject.FindGameObjectsWithTag("The Player")[0];
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rigid.linearVelocity;
        //Debug.Log("PlayerMove: " + ps.getPlayerCanMove() + "\n");
        

        if(ps.getPlayerCanMove() == true)
        {
            //Debug.Log("CanMove: " + ps.getPlayerCanMove() + "\nVelocity: " + player.GetComponent<Rigidbody>().linearVelocity + "\nPosition: " + player.transform.position);
            //Movement should be based on raw input so we can be more precise with it.
            //This section uses manual acceleration and friction to make the player move closer to original NES Metroid's movement, which quickly gets to top speed and doesn't slide around much at all.
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (Mathf.Abs(newVelocity.x + (Input.GetAxisRaw("Horizontal") * moveAccel)) >= moveSpeed)
                {
                    newVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
                }
                else
                {
                    newVelocity.x += Input.GetAxisRaw("Horizontal") * moveAccel;
                }
            }
            else if (newVelocity.x != 0)
            {
                //Check if the player is spinjumping. If they are, they should maintain horizontal momentum even if nothing is held, so we won't apply friction.
                //If the player isn't spinjumping, they should slow down or stop when holding neutral.
                bool isSpinJumping = GetComponentInChildren<PlayerJump>().IsSpinJumping();
                if (!isSpinJumping)
                {
                    newVelocity.x -= Mathf.Sign(newVelocity.x) * moveFriction;
                    //If close to zero, just set it to zero.
                    if (Mathf.Abs(newVelocity.x) < 1)
                    {
                        newVelocity.x = 0;
                    }
                }
            }
            //Check for walls when moving.
            if (newVelocity.x != 0)
            {
                Collider col = transform.GetComponentInChildren<Collider>();
                float edge = col.bounds.center.x + (col.bounds.extents.x * Mathf.Sign(newVelocity.x));
                Vector2 rayStart = new Vector2(edge, col.bounds.center.y);

                RaycastHit hit;
                Physics.Raycast(rayStart, new Vector2(Mathf.Sign(newVelocity.x), 0), out hit, Mathf.Abs(newVelocity.x/60));
                if (hit.collider != null)
                {
                    //Debug.Log("HIT SOMETHING WITH RAYCAST");
                    transform.position = new Vector2(transform.position.x + hit.distance, transform.position.y);
                    newVelocity.x = 0;
                }    
            }
            
            rigid.linearVelocity = newVelocity;
        }
        else
        {
            //rigid.linearVelocity = Vector3.zero;
        }

       



        /*if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            newVelocity.y = jumpPower;
        }*/
       
    }


    public bool isGrounded()
    {
        Collider col = transform.GetComponentInChildren<Collider>();
        Ray ray = new Ray(col.bounds.center,Vector3.down);
        float radius = col.bounds.extents.x - 0.05f;
    
        float fullDistance = col.bounds.extents.y + 0.05f;
        return Physics.SphereCast(ray,radius,fullDistance);
    }
}
