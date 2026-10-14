using UnityEngine;

public class NewStuffTeleport : MonoBehaviour
{
    private int count;

    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 0;
        player = GameObject.FindGameObjectsWithTag("The Player")[0];

        if(player == null)
        {
            Debug.LogError("Teleportation cheat for new areas cannot find the player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("CLICKED!");
            count+=1;
            switch(count)
            {
                case 0:
                    break;

                case 1:
                    player.transform.position = new Vector3(318.15f,65.15f,0);


                    break;

                case 2:
                    player.transform.position = new Vector3(167.15f,19.15f,0);
                    break;

                case 3:
                    player.transform.position = new Vector3(213.15f,128.15f,0);
                    count = 0;
                    break;


                default:
                count = 0;
                    break;
            }
        }


    }
}
