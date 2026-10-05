using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private bool canMoveUp = true;
    private bool canMoveDown = true;

    [SerializeField] float distanceFromWall = 16f;

    private Collider currentRoomCollider;

     LayerMask layerMask;
    
    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");
        if(player == null)
        {
             player = GameObject.FindGameObjectsWithTag("Player")[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
           Debug.Log("Hit a wall!");
        }
        
    }

    void LateUpdate()
    {
        
        //Camera width = 16
        //Camera height = 16
       

        transform.position = new Vector3(player.transform.position.x,transform.position.y,-10);
        
    }

    public void helperTellsMe(Collider other)
    {
        
    }
}
