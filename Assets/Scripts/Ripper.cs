using UnityEngine;

public class Ripper : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody rigid;
    private Collider col;

   [SerializeField] Vector3 moveDirection = Vector3.left;
    void Awake()
    {
       col = transform.GetComponent<Collider>();
       rigid = transform.GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        bool rayBool;
        rayBool = Physics.Raycast(col.bounds.center,moveDirection,out hit,0.5f);

        if(rayBool == false)
        {
            move();
        }
        else
        {
            if(hit.collider.tag == "Player")
            {
                move();
            }
            else if(hit.collider.tag == "Wall")
            {
                if(moveDirection == Vector3.right)
                    moveDirection = Vector3.left;
                else
                    moveDirection = Vector3.right;
            }
        }
    }

    private void move()
    {
        Vector3 movement;
        movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigid.Move(rigid.position + movement, rigid.rotation);
    }
}
