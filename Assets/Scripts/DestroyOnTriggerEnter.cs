using UnityEngine;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            if(transform.tag == "PlayerWeapon")
            {
                Destroy(transform.gameObject);
            }
        }
        else if(other.transform.tag == "Wall")
        {
            Destroy(transform.gameObject);
        }
        else if(other.transform.tag == "Player")
        {
            if(transform.tag != "PlayerWeapon")
            {
                Destroy(transform.gameObject);
            }
        }
        
        /*Debug.Log(
        $"Hit: {other.name}\n" +
        $"GameObject: {other.gameObject.name}\n" +
        $"Layer: {LayerMask.LayerToName(other.gameObject.layer)}\n" +
        $"Layer Number: {other.gameObject.layer}\n" +
        $"Is Trigger: {other.isTrigger}\n" +
        $"Tag: {other.tag}"
        );*/
    }
}
