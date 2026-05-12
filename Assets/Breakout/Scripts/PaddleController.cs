using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float Speed;
    public float ScreenBounds;
    public float MaxBounceX = 6f;
    public float StartSizeX = 2.5f;
    public float SizeDecreasePerStep = 0.3f;  
    void Update()
    {
        //Calculate what my position should be
        //I don't use a rigidbody because this isn't physics movement
        //The only thing in this game with a RB is the ball
        Vector3 pos = transform.position;
        
        //If I hit left, go left
        if (Input.GetKey(KeyCode.LeftArrow))
            pos += new Vector3(-Speed * Time.deltaTime, 0, 0);
        //If I hit right, go right
        else if (Input.GetKey(KeyCode.RightArrow))
            pos += new Vector3(Speed * Time.deltaTime, 0, 0);
        
        //If I go off the edges of the screen, don't
        if (pos.x > ScreenBounds || pos.x < -ScreenBounds)
            pos.x = Mathf.Clamp(pos.x, -ScreenBounds, ScreenBounds);
        
        //Plug in the position I calculated to my transform
        transform.position = pos;

    }

    //What X velocity should the ball have when it hits the paddle?
    public float BounceAngle(BallController ball)
    {
        float offset = ball.transform.position.x - transform.position.x;
        //Get half the width, so I can scale this nicely
        float halfWidth = GetComponent<Collider2D>().bounds.extents.x;
        float normalizedOffset = Mathf.Clamp(offset / halfWidth, -1f, 1f);
        //Return an X velocity based on where the ball hit the paddle
        return normalizedOffset * MaxBounceX;
    }

    public void UpdateSizeFrom (int score)
    {
        //Calculate how many times I should have shrunk based on the score
        int shrinkSteps = score / 1000; // Shrink every 1000 points
        shrinkSteps = Mathf.Min(shrinkSteps, 3);
        //Calculate my new size
        float newSizeX = StartSizeX - shrinkSteps * SizeDecreasePerStep;
        //Apply the new size to my transform
        Vector3 newScale = transform.localScale;
        newScale.x = newSizeX;
        transform.localScale = newScale;
    }
}
