using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EntityHealth : MonoBehaviour
{
    private IEnumerator invulTimes;

    public int hp = 3;

     public float invulTime = 1f;

    public bool canDamange = true;

    public bool testDamage = false;

    CollectibleDrop cd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cd = GetComponent<CollectibleDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        if(testDamage)
        {
            if(canDamange)
            {
                takeDamage(null);
                
            }
            testDamage = false;  
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Thing inside me!: " + other.tag);
        
        if(other.tag == "PlayerWeapon")
        {
            //Damage the self here. Let enemy deal with damage
            if(canDamange)
            {
                takeDamage(other);
            }
            else
            {
                //Debug.Log("Currently Immune to Damage!");
            }

        }
    }

    private void takeDamage(Collider other)
    {

        if(!other)
        {
            Debug.Log("Debug Damage!");
            hp--;
        }
        else
        {
            EntityDamage thing = other.gameObject.GetComponent<EntityDamage>();

            if(!thing)
            {
                thing = transform.GetComponentInParent<EntityDamage>();

                if(!thing)
                {
                    Debug.Log("Player/Friendly to player thing does not have the damage script!");
                }
                else
                    hp-=thing.getDamage();
                    
            }
            else
                hp-=thing.getDamage();
        }

        if(hp <= 0)
        {
            //Debug.Log("Enemy has died!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(!cd)
            {
                Debug.Log("Cannot find script to drop a collectible!");
            }
            else
            {
                cd.dropThing(transform.position);
            }

            gameObject.SetActive(false);
            
        }
        else
        {
            invulTimes = invul(invulTime);
            StartCoroutine(invul(invulTime));
        }
    }

    private IEnumerator invul(float waitTime)
    {
        //Debug.Log("Currently using Invul Frames!");
        canDamange = false;
        yield return new WaitForSeconds(waitTime);

        canDamange = true;
        //Debug.Log("I frames ended!");
    }
}
