using System;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{

    public bool facingRight = true;
    public bool lookingUp = false;

    public SpriteRenderer spriteRenderer;

    public Sprite spriteLookingForward;
    public Sprite spriteLookingUpward;

    public Sprite invulSpriteForward;
    public Sprite invulSpriteUpward;

    private bool defaultSprite = true;

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
        //Debug.Log("defaultSprite: " + defaultSprite);

        float horizontalAxis = Input.GetAxis("Horizontal");

        if(facingRight && horizontalAxis < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1,1,1);
           
        }
        else if(!facingRight && horizontalAxis > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(1,1,1);
           
        }

        bool holdingUp = false;

        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            holdingUp = true;
        }
        else
        {
            holdingUp = false;
        }

        if(lookingUp && !holdingUp)
        {
            lookingUp = false;
            
            if(defaultSprite)
                spriteRenderer.sprite = spriteLookingForward;
            else
                spriteRenderer.sprite = invulSpriteForward;

            
        }
        else if(!lookingUp && holdingUp)
        {
            lookingUp = true;
            
            if(defaultSprite)
                spriteRenderer.sprite = spriteLookingUpward;
            else
                spriteRenderer.sprite = invulSpriteUpward;
        }
        

    }

    public bool isLookingRight()
    {
        return facingRight;
    }

    public bool isLookingUp()
    {
        return lookingUp;
    }

    public void useInvulSkins()
    {
        defaultSprite = false;
        if (lookingUp)
            spriteRenderer.sprite = invulSpriteUpward;
        else
            spriteRenderer.sprite = invulSpriteForward;   
    }

    public void useDefaultSkins()
    {
        defaultSprite = true;
        if (lookingUp)
            spriteRenderer.sprite = spriteLookingUpward;
        else
            spriteRenderer.sprite = spriteLookingForward;
    }


}
