using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;

    PlayerInventory playerInventory;
    PlayerRun playerRun;

    bool isStanding = false;
    
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
}
