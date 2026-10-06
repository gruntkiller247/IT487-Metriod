using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    PlayerDirection playerDirection;

    PlayerInventory playerInventory;

    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firingPositionForward;
    public Transform firingPositionUpward;

    public float firingSpeed = 10f;

    public bool missileSelect = false;
    

    void Awake()
    {
        playerDirection = transform.GetComponentInParent<PlayerDirection>();
        playerInventory = transform.GetComponentInParent<PlayerInventory>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ammo: " + playerInventory.getAmmoAmount());

        if((Input.GetKeyDown(KeyCode.LeftShift)) && playerInventory.hasMissiles == true)
        {
            missileSelect = !missileSelect;
        }

        if((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Slash)) && (playerInventory.getAmmoCheat() || playerInventory.getAmmoAmount() > 0))
        {
            GameObject bulletInstance;
            if (missileSelect)
            {
                bulletInstance = GameObject.Instantiate(missilePrefab);
            }
            else
            {
                bulletInstance = GameObject.Instantiate(bulletPrefab);
                if (playerInventory.hasLongBeam)
                {
                    bulletInstance.GetComponent<DestroyOnTime>().destroyTime *= 2;
                }
            }
            
            
            
            if(playerDirection.isLookingUp())
            {
                bulletInstance.transform.Rotate(0, 0, 90);
                bulletInstance.transform.position = firingPositionUpward.position;
                bulletInstance.GetComponent<Rigidbody>().linearVelocity = Vector3.up * firingSpeed;
                
                //Debug.Log("Shooting up!");
            }
            else
            {
                bulletInstance.transform.position = firingPositionForward.position;
                
                if(playerDirection.isLookingRight())
                {
                    bulletInstance.GetComponent<Rigidbody>().linearVelocity = Vector3.right * firingSpeed;
                    //Debug.Log("Shooting Right!");
                    //Debug.Log($"Velocity is {Vector3.right * firingSpeed}");
                }
                else
                {
                    bulletInstance.GetComponent<Rigidbody>().linearVelocity = Vector3.left * firingSpeed;
                    bulletInstance.GetComponent<SpriteRenderer>().flipX = true;
                    //Debug.Log("Shooting Left!");
                    //Debug.Log($"Velocity is {Vector3.left * firingSpeed}");
                }

            }
            

            //Use this code for the Missile whenever it gets implemented!
            if(!playerInventory.getAmmoCheat())
                playerInventory.fire();
           
        }

    }
}
