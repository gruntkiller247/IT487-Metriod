using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;

    PlayerInventory playerInventory;
    PlayerRun playerRun;

    bool isStanding = true;

    private bool playerCanMove = true;
    
    void Awake()
    {
        playerInventory = transform.GetComponent<PlayerInventory>();
        playerRun = transform.GetComponent<PlayerRun>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isDown = false;

        if(playerCanMove == true)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //Debug.Log("Pressing Down!");
                isDown = true;
            }
            else
            {
                isDown = false;
            }

            if(isStanding && playerRun.isGrounded() && isDown && playerInventory.HasMorphBall())
            {
                standing.SetActive(false);
                morphed.SetActive(true);
                isStanding = false;
            }

            if(!isStanding && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                standing.SetActive(true);
                morphed.SetActive(false);
                isStanding = true;
            }
        }
        else
        {
            //Debug.Log("Player Cannot move!");
        }
        
    }

    public bool getStanding()
    {
        return isStanding;
    }

    //Temporary method for activating the morph visibly. Will be replace later by more elegant animation and etc. - Ethelyn
    //EDIT: nvm. doesn'tn't work. will make more elegant later -Ethelyn
    public void morphBody(bool morph)
    {
        if (morph == true)
        {
            standing.SetActive(false);
            morphed.SetActive(true);
        }
        else
        {
            standing.SetActive(true);
            morphed.SetActive(false);
        }
    }

    public void setPlayerCanMove(bool inBool)
    {
        playerCanMove = inBool;
    }

    public bool getPlayerCanMove()
    {
        return playerCanMove;   
    }
}
