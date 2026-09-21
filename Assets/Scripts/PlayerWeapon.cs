using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    PlayerDirection playerDirection;

    PlayerInventory playerInventory;

    public GameObject bulletPrefab;
    public Transform firingPositionForward;
    public Transform firingPositionUpward;

    public float firingSpeed = 10f;

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

        if((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Slash)) && (playerInventory.getAmmoCheat() || playerInventory.getAmmoAmount() > 0))
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab);
            
            if(playerDirection.isLookingUp())
            {
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
                    //Debug.Log("Shooting Left!");
                    //Debug.Log($"Velocity is {Vector3.left * firingSpeed}");
                }

            }

            if(!playerInventory.getAmmoCheat())
                playerInventory.fire();
           
        }

    }
}
