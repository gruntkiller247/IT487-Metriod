using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rigid;
    Collider col;

    public float jumpMax = 10f;

    bool spinJump = false;

    void Awake()
    {
        rigid = transform.GetComponentInParent<Rigidbody>();
        col = transform.GetComponent<Collider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 newVelocity = rigid.linearVelocity;
        if (spinJump && IsGrounded() && rigid.linearVelocity.y <= 0)
        {
            Debug.Log("no longer spijumping");
            spinJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector3 velocity = rigid.linearVelocity;
            velocity.y = jumpMax;
            rigid.linearVelocity = velocity;
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Debug.Log("Spinjumping!");
                spinJump = true;
            }

            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {    
            if (rigid.linearVelocity.y > 0)
            {
                Vector3 velocity = rigid.linearVelocity;
                velocity.y *= 0.5f;
                rigid.linearVelocity = velocity;
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
}
