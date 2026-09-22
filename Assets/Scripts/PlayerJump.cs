using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rigid;
    Collider col;

    public float jumpMax = 12f;

    public float jumpGain = 0.000000025f;
    private float jumpBonus = 0f;

    private bool isCharging = false;
   

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
        Vector3 newVelocity = rigid.linearVelocity;

        /*if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpBonus = 0f;
        }*/

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            if (jumpBonus < jumpMax)
                jumpBonus += jumpGain;
            Debug.Log("Holding the key! JumpBonus: " + jumpBonus);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            newVelocity.y = jumpBonus;
            rigid.linearVelocity = newVelocity;

            jumpBonus = 0f;
        }
    }


    bool IsGrounded()
    {
        Ray ray = new Ray(col.bounds.center, Vector3.down);
        float radius = col.bounds.extents.x * 0.05f;

        float fullDistance = col.bounds.extents.y + 0.05f;

        return Physics.SphereCast(ray,radius,fullDistance);
    }
}
