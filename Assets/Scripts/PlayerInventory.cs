using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    private IEnumerator invulTimes;

    public float invulTime = 5f;

    public bool hasMorphBall = false;

    public int hp = 3;

    public int ammo = 50;       //Amount of Missiles held

    public bool canDamange = true;

    public bool testDamage = false;

    public bool invulCheat = false;

    public bool ammoCheat = false;

    public TMP_Text healthText; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthText == null)
        {
            Debug.Log("Player has no HP UI Text!");
            setHpText(0);
        }
        else
         setHpText();
    }

    // Update is called once per frame
    void Update()
    {

        if(invulCheat)
            canDamange = false;
        //Debug.Log("canDamange: " + canDamange);
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
        /*switch(other.tag)
        {
            case "MorphBall":
                Destroy(other.GameObject());
                hasMorphBall = !hasMorphBall;
                return;
        }
        */
        if(other.tag == "MorphBall")
        {
            Destroy(other.GameObject());
            hasMorphBall = !hasMorphBall;
        }
        else if(other.tag == "Enemy")
        {
            //Damage the self here. Let enemy deal with damage
            if(canDamange)
            {
                takeDamage(other);
            }
            else
            {
                Debug.Log("Currently Immune to Damage!");
            }

        }
        else if(other.tag == "PickUp")
        {
            if(other.tag == "HP Pickup")
             hp+= other.GetComponent<CollectibleDrop>().getHpAmount();
            else if(other.tag == "Missile Pickup")
             ammo+=other.GetComponent<CollectibleDrop>().getMissilesAmount();
        }
    }

    public bool HasMorphBall()
    {
        return hasMorphBall;
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
                Debug.Log("Enemy does not have the damage script!");
            }
            else
                hp-=thing.getDamage();
        }

        if(hp <= 0)
        {
            Debug.Log("Player has died!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
        else
        {
            if(healthText != null)
            {
                setHpText();
            }
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

    public bool getAmmoCheat()
    {
        return ammoCheat;
    }

    public int getAmmoAmount()
    {
        return ammo;
    }

    public void fire()
    {
        ammo--;
    }

    public void fire(int amount)
    {
        ammo-=amount;
    }

    private void setHpText()
    {
        healthText.text = "EN--"+hp;
    }

    //For error handling
    private void setHpText(int num)
    {
         healthText.text = "EN--"+num;
    }
     
}
