using UnityEngine;

public class SoccerMorph : MonoBehaviour
{
    Color32 color;
    SpriteRenderer sprite;
    //Used to track the ball being hit by enemies.
    public int morphHitTimer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (morphHitTimer > 0)
        {
            morphHitTimer--;
            if (morphHitTimer == 0)
            {
                sprite.color = color;
            }
        }
    }
}
