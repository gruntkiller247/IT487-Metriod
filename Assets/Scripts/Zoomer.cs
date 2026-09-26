using System.Numerics;
using Unity.VisualScripting;
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
        west,
        error 
    }

    [SerializeField] bool limitFrameRate = false;
    LayerMask layerMask;
    //private bool prepMove = false;

    bool turn = false;

    bool leftTurn = false;
    bool rightTurn = false;

    bool forwardBlock = false;
    bool downwardBlock = false;

    void Awake()
    {
        col = transform.GetComponent<Collider>();
        rigid = transform.GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask("Wall");
        //sr = transform.GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(limitFrameRate)
            Application.targetFrameRate = 1;
    }

    void FixedUpdate()
    {
        //Check place in front and place beneath
        UnityEngine.Vector3 dir = GetForwardDirection();
        RaycastHit hit;

        
        Debug.DrawLine(col.bounds.center,dir+col.bounds.center,Color.green,0.5f);   //In front
        Debug.DrawLine(col.bounds.center,col.bounds.center + GetForwardDirection(getFaceRight()),Color.red,0.5f); //Beneath me

        if(Physics.Raycast(col.bounds.center, dir ,out hit,0.5f))
        {
            forwardBlock = true;
            if(hit.transform.tag == "Wall")
            {
                Debug.Log("Wall");
                //Need to turn
                turn = true;
            }
            else
            {
                turn = false;
                ignoreObsticale = true;
            }
        }
        else
        {
            //Debug.Log("Air");
            forwardBlock = false;
        }

        //if need to turn, need to find which way to turn, Left or Right
        //Raycast down to see if there is a block
        //If no block, turn right, else left

        if(Physics.Raycast(col.bounds.center,GetForwardDirection(getFaceRight()),out hit, 0.5f)) 
        {
            downwardBlock = true;
            if(hit.transform.tag == "Wall")
            {
                Debug.Log("Downwards raycast hit a Wall!");    
            }  
            else
            {
                Debug.Log("Downwards raycast hit something else!");
            }
        }
        else
        {
            Debug.Log("Downward raycast hit nothing!");
            //We need to turn right!
            //rightTurn = true;
            downwardBlock = false;
        }

        //Movement - Turn

        if(!ignoreObsticale && !downwardBlock && !forwardBlock)
        {
            //Air in front and beneath. Need to turn towards the 
            //Downward block direction
            lookingDirection = getFaceRight();
        }
        else if(!ignoreObsticale && downwardBlock && forwardBlock)
        {
            //If both block need to turn the oposite direction of down
            lookingDirection = getFaceLeft();
        }

        //Move
        move();
        Debug.Log("");
    }

    private void turnFaceLeft()
    {
        switch(lookingDirection)
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

            Debug.LogError("Zoomer Turn Face Left parsing Error!");
            break;
        }
    }

    private void turnFaceRight()
    {
        switch(lookingDirection)
        {
            case Directions.north:
                lookingDirection = Directions.east;
            break;

            case Directions.east:
                lookingDirection = Directions.south;
            break;

            case Directions.south:
                lookingDirection = Directions.west;
            break;

            case Directions.west:
                lookingDirection = Directions.north;
            break;

            default:

            Debug.LogError("Zoomer Turn Face Right parsing Error!");
            break;
        }

        return ;
    }

    private Directions getFaceRight()
    {
        switch(lookingDirection)
        {
            case Directions.north:
                return Directions.east;

            case Directions.east:
                return Directions.south;

            case Directions.south:
                return Directions.west;
            

            case Directions.west:
                return Directions.north;

            default:

            Debug.LogError("Zoomer Turn Face Right parsing Error!");
            return Directions.error;
            
        }
    }

    private Directions getFaceLeft()
    {
        switch(lookingDirection)
        {
            case Directions.north:
                return Directions.west;

            case Directions.east:
                return Directions.north;

            case Directions.south:
                return Directions.east;
            

            case Directions.west:
                return Directions.south;

            default:

            Debug.LogError("Zoomer Turn Face Right parsing Error!");
            return Directions.error;
            
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
                rigid.linearVelocity = newVelocity;*/
                
                rigid.Move(new UnityEngine.Vector3(transform.position.x,transform.position.y+1 * moveSpeed,0 ),UnityEngine.Quaternion.identity);
                    break;
            case Directions.east:
                //transform.position += new Vector3(1,0,0);
                /*newVelocity.x = moveSpeed;
                newVelocity.y = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new UnityEngine.Vector3(transform.position.x+1* moveSpeed,transform.position.y,0),UnityEngine.Quaternion.identity);
                    break;
            case Directions.south:
                //transform.position += new Vector3(0,-1,0);
                /*newVelocity.y = -moveSpeed;
                newVelocity.x = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new UnityEngine.Vector3(transform.position.x,transform.position.y-1* moveSpeed,0) ,UnityEngine.Quaternion.identity);
                    break;
            case Directions.west:
                //transform.position += new Vector3(-1,0,0);
                /*newVelocity.x = -moveSpeed;
                newVelocity.y = 0;
                rigid.linearVelocity = newVelocity;
                */
                rigid.Move(new UnityEngine.Vector3(transform.position.x-1* moveSpeed,transform.position.y,0) ,UnityEngine.Quaternion.identity);
                    break;
            default:
                Debug.Log("ERROR!");
                break;
        }
    }

    private UnityEngine.Vector3 GetForwardDirection()
    {
        switch (lookingDirection)
        {
            case Directions.north:
                return UnityEngine.Vector3.up;

            case Directions.east:
                return UnityEngine.Vector3.right;

            case Directions.south:
                return UnityEngine.Vector3.down;

            case Directions.west:
                return UnityEngine.Vector3.left;

            default:
                return UnityEngine.Vector3.zero;
        }
    }
    private UnityEngine.Vector3 GetForwardDirection(Directions inDirection)
    {
        switch (inDirection)
        {
            case Directions.north:
                return UnityEngine.Vector3.up;

            case Directions.east:
                return UnityEngine.Vector3.right;

            case Directions.south:
                return UnityEngine.Vector3.down;

            case Directions.west:
                return UnityEngine.Vector3.left;

            default:
                return UnityEngine.Vector3.zero;
        }
    }


    // Update is called once per frame
    /*
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
                Debug.Log("Double Nothing!");
                if(lookingDirection == Directions.south /*&& prepMove)
                {
                    //prepMove = false;
                    Debug.Log("Double Blank and moving south!");
                    transform.Rotate(0,0,90);
                    lookingDirection = Directions.west;
                    
                    
                }
                else //if(prepMove)
                {
                    //Gets stuck here
                    //prepMove = false;
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
        Debug.Log("Position: " + transform.position);
        Debug.Log("");

    }
    */

    //Returns 0 if the block in front is emptyspace
    //Returns 1 if the block in front is a block
    /*
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
            Debug.Log("forwardRay: Name of thing hit: " + hit.collider.gameObject.tag);

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
        
        //new Ray(col.bounds.center+col.bounds.extents, forward);
        Ray ray = new Ray(col.bounds.center, forward);
        float radius = col.bounds.extents.x * 0.05f;

        //Debug.DrawLine(col.bounds.center + col.bounds.extents,forward + col.bounds.center + col.bounds.extents,Color.green,100);
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
    */


    /*
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

    private Vector3 rotateLeft(Vector3 edit)
    {
        return new Vector3(edit.y, -edit.x, 0);
    }
    */
}