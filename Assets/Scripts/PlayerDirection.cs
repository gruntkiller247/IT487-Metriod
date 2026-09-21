using System;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{

    public bool facingRight = true;
    public bool lookingUp = false;

    public SpriteRenderer spriteRenderer;

    public Sprite spriteLookingForward;
    public Sprite spriteLookingUpward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        bool holdingUp = Input.GetKey(KeyCode.UpArrow);

        if(lookingUp && !holdingUp)
        {
            lookingUp = false;
            spriteRenderer.sprite = spriteLookingForward;
        }
        else if(!lookingUp && holdingUp)
        {
            lookingUp = true;
            spriteRenderer.sprite = spriteLookingUpward;
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
}
