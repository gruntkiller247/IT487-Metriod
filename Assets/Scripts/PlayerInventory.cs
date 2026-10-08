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
    public bool hasLongBeam = false;

    public bool hasMissiles = false;

    [SerializeField] int hp = 3;

    public int ammo = 50;       //Amount of Missiles held

    public bool canDamange = true;

    public bool testDamage = false;

    public bool invulCheat = false;

    public bool ammoCheat = false;

    public TMP_Text healthText; 
    public TMP_Text missileText; 

    private PlayerDirection pd;

    void Awake()
    {
        pd = GetComponentInParent<PlayerDirection>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthText == null)
        {
            Debug.Log("Player has no HP UI Text!");
            setHpText(0);
            
        }
        else
        {
            setHpText();
        }
        

        if(missileText == null)
        {
            Debug.Log("No Missile Text!");
            setMissileText(0);
        }
        else
        {
            setMissileText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            invulCheat = !invulCheat;
            ammoCheat = !ammoCheat;
        }

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
        //Debug.Log("Other tag: " + other.tag);
        //Debug.Log("Other name: " + other.name);
        if(other.tag == "MorphBall")
        {
            Destroy(other.GameObject());
            hasMorphBall = !hasMorphBall;
        }
        else if (other.tag == "LongBeam")
        {
            Destroy(other.GameObject());
            hasLongBeam = true;
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
        else if(other.tag == "HpPickUp")
        {
            //Debug.Log(other.GetComponent<PickupInventory>().getHp());
            hp += other.GetComponent<PickupInventory>().getHp();
            setHpText();   
        }
        else if(other.tag == "MisslePickUp")
        {
            ammo += other.GetComponent<PickupInventory>().getMissiles();
            setMissileText();
        }
    }

    public bool HasMorphBall()
    {
        return hasMorphBall;
    }

    public void takeDamage(Collider other)
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
            //Change skin
            
            StartCoroutine(invul(invulTime));
            PlayerSound.instance.PlaySound(PlayerSound.instance.hurt);
        }
    }

    private IEnumerator invul(float waitTime)
    {
        //Debug.Log("Currently using Invul Frames!");
        pd.useInvulSkins();
        canDamange = false;
        yield return new WaitForSeconds(waitTime);

        pd.useDefaultSkins();
        canDamange = true;

        //Reset Skin
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
        setMissileText();
    }

    public void fire(int amount)
    {
        ammo-=amount;
        setMissileText();
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
    private void setMissileText()
    {
        missileText.text = "MI--"+ammo;
    }

    //For error handling
    private void setMissileText(int num)
    {
        missileText.text = "MI--"+num;
    }

    public Transform getPlayerLocation()
    {
        return gameObject.transform;
    }
     
}
