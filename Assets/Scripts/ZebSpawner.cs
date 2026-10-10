using UnityEngine;

public class ZebSpawner : MonoBehaviour
{
    [SerializeField] GameObject zeb;
    [SerializeField] GameObject player;

    [SerializeField] int distance = 5;
    [SerializeField] int spawnTimerMax = 120;
    private int spawnTimer = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        if(zeb == null)
        {
            zeb = GameObject.FindGameObjectsWithTag("Zeb")[0];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= 1;
        }
        else if (Mathf.Abs(player.transform.position.x - transform.position.x) <= distance)
        {
            spawnTimer = spawnTimerMax;
            Instantiate(zeb);
        }
    }
}
