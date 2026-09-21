using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rigid;
    Collider col;

    public float jumpPower = 12f;

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

        if(Input.GetKeyDown(KeyCode.A) && IsGrounded())
        {
            newVelocity.y = jumpPower;
        }

        rigid.linearVelocity = newVelocity;
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(col.bounds.center, Vector3.down);
        float radius = col.bounds.extents.x * 0.05f;

        float fullDistance = col.bounds.extents.y + 0.05f;

        return Physics.SphereCast(ray,radius,fullDistance);
    }
}
