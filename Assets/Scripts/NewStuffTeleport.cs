using UnityEngine;

public class NewStuffTeleport : MonoBehaviour
{
    private int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("CLICKED!");
        }


    }
}
