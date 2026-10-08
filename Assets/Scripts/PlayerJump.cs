using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rigid;
    Collider col;

    public float jumpMax = 10f;

    bool spinJump = false;

    private bool isJumping = false;

    PlayerState ps;

    void Awake()
    {
        rigid = transform.GetComponentInParent<Rigidbody>();
        col = transform.GetComponent<Collider>();
        ps = transform.GetComponentInParent<PlayerState>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.getPlayerCanMove() == true)
        {
            if(ps.getStanding())
            {
                //Vector3 newVelocity = rigid.linearVelocity;
                if (spinJump && IsGrounded() && rigid.linearVelocity.y <= 0)
                {
                    ps.morphBody(false);
                    spinJump = false;
                }

                if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
                {
                    PlayerSound.instance.PlaySound(PlayerSound.instance.jump);

                    Vector3 velocity = rigid.linearVelocity;
                    velocity.y = jumpMax;
                    rigid.linearVelocity = velocity;

                    isJumping = true;
                    if (Input.GetAxisRaw("Horizontal") != 0)
                    {
                        spinJump = true;
                        //The below doesn't work because of how these weird child objects are set up. I will be controlling this through animations in the future. -Ethelyn
                        //ps.morphBody(true);
                    }

                    
                }

                if (Input.GetKeyUp(KeyCode.X))
                {    
                    if (rigid.linearVelocity.y > 0)
                    {
                        Vector3 velocity = rigid.linearVelocity;
                        velocity.y *= 0.5f;
                        rigid.linearVelocity = velocity;
                    }
                }
            }
        }
        

        


        /*if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //State is charging = true;
            isCharging = true;
            timeJumpPressed = Time.time;
            Debug.Log("Jump button pressed at: " + timeJumpPressed);
        }

        //Debug.Log("isCharging: " + isCharging + "\nTime.time: " + Time.time);

        if(isCharging)
        {
            jumpBonus+=jumpGain * Time.deltaTime;;
            Debug.Log("Jump Bonus: " + jumpBonus);
        }

        if(Input.GetKeyUp(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Jump button unpressed!");
            //This is when the player jumps
            isCharging = false;
            timeJumpPressed = 0;

            if(jumpBonus == 0)
                newVelocity.y = jumpMax;
            else
                newVelocity.y = jumpBonus;

            rigid.linearVelocity = newVelocity;
            jumpBonus = 0;
        }

        /*if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            newVelocity.y = jumpMax;
            rigid.linearVelocity = newVelocity;
        }*/
    }


    bool IsGrounded()
    {
        Ray ray = new Ray(col.bounds.center, Vector3.down);
        float radius = col.bounds.extents.x * 0.05f;

        float fullDistance = col.bounds.extents.y + 0.05f;

        return Physics.SphereCast(ray,radius,fullDistance);
    }

    public bool IsSpinJumping()
    {
        return spinJump;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
}
