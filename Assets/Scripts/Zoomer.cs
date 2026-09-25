using UnityEngine;

public class Zoomer : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody rigid;
    private Collider col;
    //private Directions localNorth = Directions.south; //Assuming looking down by default
    //private Directions lastMove = Directions.west;

    private bool ignoreObsticale = false;
    
    public Directions lookingDirection = Directions.east;

    public bool canMove = true;
    public enum Directions
    {
        north = 0,
        east,
        south,
        west
    }

    [SerializeField] bool limitFrameRate = false;

    LayerMask layerMask;
    private bool prepMove = false;

    void Awake()
    {
        col = transform.GetComponent<Collider>();
        rigid = transform.GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask("Wall");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(limitFrameRate)
            Application.targetFrameRate = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!canMove)
          return;

        move();
          
        //Debug.Log("My looking direction was: " + lookingDirection);
        if(raycastForward() == 0 || ignoreObsticale)
        {

            //Debug.Log("Space in front is empty!");
            if(raycastForwardThenDown() == 1)
            {
                ;
            }
            else
            {

                if(lookingDirection == Directions.south /*&& prepMove*/)
                {
                    //prepMove = false;
                    //Debug.Log("Double Blank and moving south!");
                    transform.Rotate(0,0,90);
                    lookingDirection = Directions.west;
                    
                    
                }
                else //if(prepMove)
                {
                    prepMove = false;
                    transform.Rotate(0, 0, -90);
                    switch (lookingDirection)
                    {
                        case Directions.north:
                            lookingDirection = Directions.east;
                            break;
                        case Directions.east:
                            lookingDirection = Directions.south;
                            break;
                        case Directions.south:
                            lookingDirection = Directions.east;
                            break;
                        case Directions.west:
                        lookingDirection = Directions.north;
                            break;
                        default:
                            Debug.Log("ERROR!");
                            break;
                    }
                }
                /*else
                {
                    prepMove = true;
                }*/

                //move();
            }
        }
        else
        {
            if(ignoreObsticale)
            {
                //This "wall" is something like the player or a powerup!
                //Move as normal!
            }
            else
            {
                transform.Rotate(0, 0, 90);
                switch (lookingDirection)
                {
                    case Directions.north:
                        lookingDirection = Directions.west;
                        break;
                    case Directions.east:
                        lookingDirection = Directions.north;
                        break;
                    case Directions.south:
                        lookingDirection = Directions.east;
                        break;
                    case Directions.west:
                    lookingDirection = Directions.south;
                        break;
                    default:
                        Debug.Log("ERROR!");
                        break;
                }
            }
            
        }

        move();
        //Debug.Log("Position: " + transform.position);
        Debug.Log("");

    }

    //Returns 0 if the block in front is emptyspace
    //Returns 1 if the block in front is a block
    private int raycastForward()
    {
        RaycastHit hit;

        Vector3 forward = GetForwardDirection();

        Ray ray = new Ray(col.bounds.center, forward);
        float radius = col.bounds.extents.x * 0.05f;

        Debug.DrawLine(col.bounds.center,forward + col.bounds.center,Color.red,100);
        //Stuff breaks when I change the raycast, work on later
        //if(Physics.Raycast(col.bounds.center,forward,out hit,1f,layerMask))
        if(Physics.SphereCast(ray,radius,out hit, 1))
        {
            
            //Wanting to move forwards but hit a wall, rotate left
            //Debug.Log("forwardRay: Name of thing hit: " + hit.collider.gameObject.tag);

            if(hit.collider.gameObject.tag != "Wall")
            {
                //The ray hit something that isn't a wall such as the player or an item etc...
                //We just ignore them and keep moving in this direction
                ignoreObsticale = true; 
                Debug.Log("hit a thing that is not a wall! Need to ignore it!");
            }
            else
                ignoreObsticale = false;

            return 1;
        }
        else
        {
            //Hit nothing, what we want. Need to check if forward then down is a block
            Debug.Log("forwardRay DID NOT HIT ANYTHING!");
            return 0;
        }

        //return -1;
    }

    //returns 0 if the block is emptyspace
    //returns 1 if the block is a block
    private int raycastForwardThenDown()
    {
        RaycastHit hit;
        Vector3 forward = GetForwardDirection();
        forward = rotateRight(forward);
        
        Ray ray = new Ray(col.bounds.center, forward);
        float radius = col.bounds.extents.x * 0.05f;


        Debug.DrawLine(col.bounds.center,forward + col.bounds.center,Color.green,100);

        //if(Physics.Raycast(col.bounds.center,forward,out hit, 1f,layerMask))
        if(Physics.SphereCast(ray,radius,out hit, 1))
        {
            Debug.Log("forwardThenDown: Name of thing hit: " + hit.collider.gameObject.name);
            return 1;
        }
        else
        {
            Debug.Log("forwardThenDown: DID NOT HIT ANYTHING!");
            return 0;
        }
            
        
    }
    private Vector3 GetForwardDirection()
    {
        switch (lookingDirection)
        {
            case Directions.north:
                return Vector3.up;

            case Directions.east:
                return Vector3.right;

            case Directions.south:
                return Vector3.down;

            case Directions.west:
                return Vector3.left;

            default:
                return Vector3.zero;
        }
    }

    private void move()
    {
        //Vector3 newVelocity = rigid.linearVelocity;
        switch (lookingDirection)
        {
            
            case Directions.north:
                //transform.position += new Vector3(0,1,0);
                /*newVelocity.y = moveSpeed;
                newVelocity.x = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new Vector3(transform.position.x,transform.position.y+1 * moveSpeed,0 ),Quaternion.identity);
                    break;
            case Directions.east:
                //transform.position += new Vector3(1,0,0);
                /*newVelocity.x = moveSpeed;
                newVelocity.y = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new Vector3(transform.position.x+1* moveSpeed,transform.position.y,0),Quaternion.identity);
                    break;
            case Directions.south:
                //transform.position += new Vector3(0,-1,0);
                /*newVelocity.y = -moveSpeed;
                newVelocity.x = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new Vector3(transform.position.x,transform.position.y-1* moveSpeed,0) ,Quaternion.identity);
                    break;
            case Directions.west:
                //transform.position += new Vector3(-1,0,0);
                /*newVelocity.x = -moveSpeed;
                newVelocity.y = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new Vector3(transform.position.x-1* moveSpeed,transform.position.y,0) ,Quaternion.identity);
                    break;
            default:
                Debug.Log("ERROR!");
                break;
        }
    }

    private Vector3 rotateRight()
    {
        Vector3 forward = GetForwardDirection();

        return new Vector3(forward.y, -forward.x, 0);
    }

    private Vector3 rotateRight(Vector3 edit)
    {
        //Vector3 forward = GetForwardDirection();

        return new Vector3(edit.y, -edit.x, 0);
    }
}