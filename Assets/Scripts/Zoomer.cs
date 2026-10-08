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

    //private bool ignoreObsticale = false;
    
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

    [SerializeField] float turnCooldown = 0.15f;
    private float turnTimer = 0f;

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

        turnCooldown = turnCooldown/moveSpeed;
    }

    

    void FixedUpdate()
    {
        //Check place in front and place beneath
        UnityEngine.Vector3 dir = GetForwardDirection();
        RaycastHit hit;
        turnTimer -= Time.fixedDeltaTime;


        //Debug.DrawLine(col.bounds.center,dir+col.bounds.center,Color.green,0.01f);   //In front
        //Debug.DrawLine(col.bounds.center,col.bounds.center + GetForwardDirection(getFaceRight()),Color.red, 0.01f); //Beneath me
        switch(lookingDirection)
        {
            case Directions.north:
                transform.eulerAngles = new UnityEngine.Vector3(0, 0, 90);
                break;
            case Directions.south:
                transform.eulerAngles = new UnityEngine.Vector3(0, 0, 270);
                break;
            case Directions.east:
                transform.eulerAngles = new UnityEngine.Vector3(0, 0, 0);
                break;
            case Directions.west:
                transform.eulerAngles = new UnityEngine.Vector3(0, 0, 180);
                break;
            default:
                break;
        }
        //Shoot forward. Wall or no Wall?
        if(Physics.Raycast(col.bounds.center, dir ,out hit,0.5f)) 
        {
            //Something
            
            if(hit.transform.tag == "Wall")
            {
                //Wall
                //Debug.Log("Wall");
                forwardBlock = true;
                //If wall in front, ask if wall beneath
            }
            else
            {
                //Not a wall but someting like a powerup, other enemy, or the player. We should act as if there is no wall!
                forwardBlock = false;
            }
        }
        else
        {
            //No wall
            //Debug.Log("Air");
            forwardBlock = false;
        }

        //Shoot beneath. Wall or no Wall?
        if(Physics.Raycast(col.bounds.center,GetForwardDirection(getFaceRight()),out hit,0.5f)) 
        {
            //Something!
            if(hit.transform.tag == "Wall")
            {
                //Wall
                downwardBlock = true;
                //Debug.Log("Downwards raycast hit a Wall!");    
            }  
            else
            {
                //Not a wall. Something else! Assume it is empty space!
                //Debug.Log("Downwards raycast hit something else!");
                downwardBlock = false;
            }
        }
        else
        {
            //Debug.Log("Downward raycast hit nothing!");
            //No wall!
            downwardBlock = false;
        }

        
        //Movement - Turn
        //4 potential events
        //dB = T && fB = T  //Both raycast hit a wall. This means we hit a corner and the empty space to turn towards is above us!
        //db = F && fb = T  //We are currently floating. We need to turn downwards aka right!
        //db = T && fb = T  //We are on solid ground and can move forward!
        //db = F && fb = F  //We are floating! Most likely on a corner. Turn towards downward direction!
        
        if(turnTimer <= 0f)
        {
            if(downwardBlock && forwardBlock)
            {
                //We need to turn left of the forward direction!
                turnFaceLeft();
                turnTimer = turnCooldown;
            }
            else if(!downwardBlock && forwardBlock)
            {
                turnFaceRight();
                turnTimer = turnCooldown;
            }
            else if(downwardBlock && !forwardBlock)
            {
                ;
            }
            else if(!downwardBlock && !forwardBlock)
            {
                turnFaceRight();
                turnTimer = turnCooldown;
                
            }
        }


        //Move
        move();
        //Debug.Log(transform.position);
        //Debug.Log("\n\n\n\n\n");
       
    }

    private float GetForwardExtent()
    {
        UnityEngine.Vector3 forward = GetForwardDirection();

        if (forward == UnityEngine.Vector3.right || forward == UnityEngine.Vector3.left)
            return col.bounds.extents.x;

        if (forward == UnityEngine.Vector3.up || forward == UnityEngine.Vector3.down)
            return col.bounds.extents.y;

        return 0f;
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
        UnityEngine.Vector3 movement = GetForwardDirection() * moveSpeed * Time.fixedDeltaTime;
        rigid.Move(rigid.position + movement, rigid.rotation);
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
}