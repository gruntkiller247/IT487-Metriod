using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Rigidbody rigid;
    public float moveSpeed = 5f;
    public float jumpPower = 12f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = transform.GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rigid.linearVelocity;
        newVelocity.x=Input.GetAxis("Horizontal") * moveSpeed;
        
    
        if(Input.GetKeyDown(KeyCode.Z) && isGrounded())
        {
            newVelocity.y = jumpPower;
        }

        rigid.linearVelocity = newVelocity;
    }

    private bool isGrounded()
    {
        Collider col = transform.GetComponentInChildren<Collider>();
        Ray ray = new Ray(col.bounds.center,Vector3.down);
        float radius = col.bounds.extents.x - 0.05f;
    
        float fullDistance = col.bounds.extents.y + 0.05f;
        return Physics.SphereCast(ray,radius,fullDistance);
    }
}
