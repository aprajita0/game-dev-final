using UnityEngine;

public class BallController : MonoBehaviour
{
    //My Rigibody
    public Rigidbody2D RB;
    //My starting velocity. This should be set in the editor
    public Vector2 StartVel;
    //My starting position, where I respawn into. I set this in Start()
    public Vector3 StartPos;
    public Vector2 LastVel;

    public float BallSpeed;
    //How much faster should I get when break a brick?
    public float SpeedIncreasePerBrick = 0.15f;
    //This stops from going sideways
    public float MinYDirection = 0.5f;
    
    void Start()
    {
        //I record where I started, so I can respawn there
        StartPos = transform.position;

        BallSpeed = StartVel.magnitude;

        RB.linearVelocity = StartVel;
    }

    void Update()
    {
        //If I'm off-screen, I respawn with my initial position & speed
        if (transform.position.y < -10)
        {
            if (BreakoutManager.Me == null || BreakoutManager.Me.LoseLife())
            {
                Respawn();
            }
        }
    }

    void FixedUpdate()
    {
        //I record my velocity every physics fme
        //so know what direction I was moving before a collision
        LastVel = RB.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If I hit something, I'm going to bounce. Let's calculate my new
        //velocity
        //Use my saved velocity from before the collision happened
        Vector2 vel = LastVel;

        if (vel.magnitude < 0.01f)
            vel = RB.linearVelocity;
        if (vel.magnitude < 0.01f)
            vel = StartVel;

        PaddleController pc =
            collision.gameObject.GetComponent<PaddleController>();
        if (pc != null)
        {
            Vector2 newVel = new Vector2(pc.BounceAngle(this), 1.5f);

            newVel = FixFlatBounce(newVel, true);

            newVel = newVel.normalized * BallSpeed;

            RB.linearVelocity = newVel;
            return;
        }

       
        BrickController bc = collision.gameObject.GetComponent<BrickController>();
        if (bc != null)
        {
            //i bounce based on the side of the brick I hit
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 newVel = Vector2.Reflect(vel, contact.normal);

            //Make the game speed up a little whenever I break a brick
            BallSpeed += SpeedIncreasePerBrick;

            //Make sure I'm not too flat
            newVel = FixFlatBounce(newVel, newVel.y > 0);

            //Keep my direction, but apply my updated speed
            newVel = newVel.normalized * BallSpeed;

            bc.Break();

            //I've calculated any bouncing I need to do
            RB.linearVelocity = newVel;
            return;
        }

        //If I hit a wall, bounce based on the collision normal
        if (
            collision.gameObject.CompareTag("VWall")
            || collision.gameObject.CompareTag("HWall")
        )
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 newVel = Vector2.Reflect(vel, contact.normal);

            //Make sure I'm not too flat
            newVel = FixFlatBounce(newVel, newVel.y > 0);

            //Keep my direction, but apply my current speed
            newVel = newVel.normalized * BallSpeed;

            RB.linearVelocity = newVel;
        }
    }

    //Fix my boring bounces that are too horizontal
    Vector2 FixFlatBounce(Vector2 dir, bool forceUp)
    {
        dir = dir.normalized;

        if (Mathf.Abs(dir.y) < MinYDirection)
        {
            dir.y = forceUp ? MinYDirection : -MinYDirection;

            
            if (Mathf.Abs(dir.x) < 0.01f)
            {
                if (LastVel.x >= 0)
                    dir.x = 1f;
                else
                    dir.x = -1f;
            }

            dir = dir.normalized;
        }

        return dir;
    }
    public void Respawn()
    {
        transform.position = StartPos;
        BallSpeed = StartVel.magnitude;
        RB.linearVelocity = StartVel;
    }
}