using UnityEngine;

public class SoccerGoal : MonoBehaviour
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<SoccerMorph>())
        {
            Debug.Log("Collided with soccer morph");
            if (collision.gameObject.GetComponentInChildren<SoccerMorph>().morphHitTimer > 0)
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
