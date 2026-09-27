using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    //[SerializeField] int amountSprites = 2;
    [SerializeField] Sprite[] sprites;
    private int spriteInUse = 0;

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 dir = GetForwardDirection();

        if(useOtherSprite)
        {
            useOtherSprite = false;
            sr.sprite = sprite1;
            rotateLeft(dir);
            

        }
        else
        {
            useOtherSprite = true;
            sr.sprite = sprite2;
            rotateLeft(dir);
        }
        */
    }
}
