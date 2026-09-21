using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;

    PlayerInventory playerInventory;

    bool isStanding = false;
    
    void Awake()
    {
        playerInventory = transform.GetComponent<PlayerInventory>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStanding && Input.GetKeyDown(KeyCode.DownArrow) && playerInventory.HasMorphBall())
        {
            standing.SetActive(false);
            morphed.SetActive(true);
            isStanding = !isStanding;
        }

        if(!isStanding && (Input.GetKeyDown(KeyCode.A)) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            standing.SetActive(true);
            morphed.SetActive(false);
            isStanding = !isStanding;
        }
    }
}
