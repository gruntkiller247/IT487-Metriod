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

    void Awake()
    {
        col = transform.GetComponent<Collider>();
        //rigid = transform.GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Application.targetFrameRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Ask the question with a raycast, what direction should I cling to?
        //do raycast in all directions, find the one cloest to me within 1 tile
        //align my model to that block
        //Default to moving left

         

        /*Directions directionToLookAt = whatDirection();
        transform.Rotate(0, 0, -90); rotates right
        
        Vector3 newVelocity = rigid.linearVelocity;
        newVelocity.x = moveSpeed;
        rigid.linearVelocity = newVelocity;
        
        localNorth = Directions.east;
        //Debug.Log("Local: " + localNorth);
        //Debug.Log("Look at: " + directionToLookAt);
        while(localNorth != directionToLookAt)
        {
            transform.Rotate(0, 0, -90);
            localNorth++;
            Debug.Log("I AM ROTATING!");
        }*/

        //Ray cast 1 block in front, if empty space continue
        //If empty space, ping 1 in front and 1 down check for a block
        //If that block is walkable, walk forward



        //If first fails, rotate left
        //If first is good, second fails: move forward then rotate right
        //if second fails go froward

        //Need to keep track of my current dirction + what rotation I am

        if(!canMove)
          return;

        if(raycastForward() == 0 || ignoreObsticale)
        {
            //Space in front of me is emptyspace
            //Debug.Log("Space in front is empty!");
           


            if(raycastForwardThenDown() == 1)
            {
                //Move forward relative. Continue as normal. Pass + Pass
                //Take current position
                //If going north y++
                //If going east x++
                //If going south y--
                //If going west x--
                //Debug.Log("Space in front and down is a block!");
                
                switch (lookingDirection)//What is wrong here
                {
                    case Directions.north:
                        transform.position += new Vector3(0,1,0);
                        break;
                    case Directions.east:
                        transform.position += new Vector3(1,0,0);
                        break;
                    case Directions.south:
                        transform.position += new Vector3(0,-1,0);
                        break;
                    case Directions.west:
                        transform.position += new Vector3(-1,0,0);
                        break;
                    default:
                        Debug.Log("ERROR!");
                        break;
                }
                
                
            }
            else
            {
                //Empty space then an empty space. Pass + fail
                //Move rotate right then move forward
                //lookingDirection
                //Trying to do this move 
                //Debug.Log("Space in front and down is empty space!");

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

                switch (lookingDirection)//What is wrong here
                {
                    case Directions.north:
                        transform.position += new Vector3(0,1,0);
                        break;
                    case Directions.east:
                        transform.position += new Vector3(1,0,0);
                        break;
                    case Directions.south:
                        transform.position += new Vector3(0,-1,0);
                        break;
                    case Directions.west:
                        transform.position += new Vector3(-1,0,0);
                        break;
                    default:
                        Debug.Log("ERROR!");
                        break;
                }


            }
        }
        else
        {
            //Space in front of me is a block/wall. Fail

            //Need to check if we should ignore this "Wall"

            if(ignoreObsticale)
            {
                //This "wall" is something like the player or a powerup!
                //Move as normal!
               
            }
            else
            {
                transform.Rotate(0, 0, 90);

                //Debug.Log("Space in front of me is a wall!");
                //Debug.Log("My looking direction was: " + lookingDirection);
                    //Debug.Log("Trying to rotate Left!");
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

        //Debug.Log("");

    }

    //Returns 0 if the block in front is emptyspace
    //Returns 1 if the block in front is a block
    private int raycastForward()
    {
        Vector3 dir = GetForwardDirection(); //= Vector3.down;
        
        /*if(lookingDirection == Directions.north)
            dir = Vector3.up;
        else if(lookingDirection == Directions.east)
            dir = Vector3.right;
        else if(lookingDirection == Directions.south)
            dir = Vector3.down;
        else
            dir = Vector3.left;*/

        Ray ray = new Ray(col.bounds.center, dir);
        float radius = col.bounds.extents.x * 0.05f;

        RaycastHit hit;

        if(Physics.SphereCast(ray,radius,out hit, 1))
        {
            //Wanting to move forwards but hit a wall, rotate left
            //Debug.Log("forwardRay: Name of thing hit: " + hit.collider.gameObject.tag);

            if(hit.collider.gameObject.tag != "Wall")
            {
                //The ray hit something that isn't a wall such as the player or an item etc...
                //We just ignore them and keep moving in this direction
                ignoreObsticale = true; 
                //Debug.Log("hit a thing that is not a wall! Need to ignore it!");
            }
            else
                ignoreObsticale = false;

            return 1;
        }
        else
        {
            //Hit nothing, what we want. Need to check if forward then down is a block
            //Debug.Log("forwardRay DID NOT HIT ANYTHING!");
            return 0;
        }

        //return -1;
    }

    //returns 0 if the block is emptyspace
    //returns 1 if the block is a block
    private int raycastForwardThenDown()
    {
        //Take what direction I am going
        //lookingDirection
        //If north, +1Y then +1X
        //If east, -1Y then +1X
        //If south, -1Y then -1X
        //If west, -1Y then -1X

        

        /*switch(lookingDirection)
        {
            case Directions.north:
            dir = new Vector3(1,1,0);
            break;

            case Directions.east:
            dir = new Vector3(1,-1,0);
            break;

            case Directions.south:
            dir = new Vector3(-1,-1,0);
            break;

            default:
            dir = new Vector3(1,-1,0);
            break;
        }
        */

        Vector3 forward = GetForwardDirection();
        Vector3 oneInFront = col.bounds.center + forward;

        Ray ray = new Ray(col.bounds.center, rotateRight());
        float radius = col.bounds.extents.x * 0.05f;

        RaycastHit hit;

        if(Physics.SphereCast(ray,radius,out hit, 1))
        {
            //Debug.Log("forwardThenDown: Name of thing hit: " + hit.collider.gameObject.name);
            return 1;
        }
        else
        {
            //Debug.Log("forwardThenDown: DID NOT HIT ANYTHING!");
            return 0;
        }
            
        
    }

    private Vector3 rotateRight()
    {
        Vector3 forward = GetForwardDirection();

        return new Vector3(forward.y, -forward.x, 0);
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


    /*
    Directions whatDirection()
    {
        Vector3 dir = Vector3.down;
        Ray ray = new Ray(col.bounds.center, dir);
        float radius = col.bounds.extents.x * 0.05f;

        //float fullDistance = col.bounds.extents.y + 0.05f;

        if(Physics.SphereCast(ray,radius,1))
        {
            //Something is true south of me
            return Directions.south;
        }
        //return Physics.SphereCast(ray,radius,fullDistance);

        
        //Rotate right, looking east
        if(Physics.SphereCast(ray,radius,1))
        {
            return Directions.east;
        }

        if(Physics.SphereCast(ray,radius,1))
        {
            return Directions.north;
        }

        return Directions.west;
    }
    */
}
