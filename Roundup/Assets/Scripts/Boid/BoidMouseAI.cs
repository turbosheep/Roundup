using UnityEngine;
using System.Collections;

public class BoidMouseAI : MonoBehaviour
{

    public float separation = 1f;
    public float neighborRadius = 5.0f;

    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float edgeWeight = 1f;

    private bool isWrapping = false;  // Used in WrapAround()
    private bool iwx = false;  // Used in StayOnScreen()
    private bool iwy = false;  // Used in StayOnScreen()

    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;

    public BoidMouseAI[] boids { get; set; }

    public float mForce = 1f;
    public float mSpeed = 1f;

    public float distanceFromEdge = 1f;

    protected Motion _motion;
    protected Quaternion startRotation;

    // Use this for initialization
    void Start()
    {
        _motion = GetComponent<Motion>();
        startRotation = transform.rotation;
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        acceleration = new Vector2();
        location = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (boids != null)
        {
            Vector3 mousePos = _motion.GetMousePos();
            float distance = Vector3.Distance(transform.position, mousePos);
            if(distance > _motion.sightRadius || !Input.GetMouseButton(0))
            {
                flock();
                velocity = Vector2.Lerp(velocity, velocity + acceleration * Time.deltaTime, 1);
                velocity = Vector2.ClampMagnitude(velocity, mSpeed);
                location = Vector2.Lerp(location, location + velocity * Time.deltaTime, 1);
                location *= WrapAround();      //WRAPS the boid in astroids type manner.
                acceleration = Vector2.zero;
                this.gameObject.transform.position = location;
            }
            else
            {
                //_motion.DoRepel();
                Flee(mousePos);
                location = new Vector2(transform.position.x, transform.position.y);
                location *= WrapAround();
                transform.position = location;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void flock()
    {

        //find the three components of the flock, plus the force to stay onscreen
        Vector2 sep = Separate();
        Vector2 ali = Align();
        Vector2 coh = Cohesion();

        //weight them
        sep *= separationWeight;
        ali *= alignmentWeight;
        coh *= cohesionWeight;

        //apply to the boid
        ApplyForce(sep);
        ApplyForce(ali);
        ApplyForce(coh);
    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force;
    }

    /// <summary>
    /// Calculate the force applied to steer a boid towards a target.
    /// </summary>
    /// <param name="target"></param>				
    /// <returns></returns>
    private Vector2 Seek(Vector2 target)
    {
        //get a vector to the desired location, capped for speed
        Vector2 desired = target - location;
        Vector2 steer = desired - velocity;
        return steer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector2 Separate()
    {
        Vector2 steer = new Vector2();
        int count = 0;

        foreach (BoidMouseAI b in boids)
        {
            Vector2 dist = location - b.location;
            float d = dist.magnitude;
            if (d > 0 && d < separation)
            {
                dist /= d;
                steer += dist;
                count++;
            }
        }
        if (count > 0)
        {
            steer /= (float)count;
        }
        if (steer.magnitude > 0)
        {
            steer *= mSpeed;
            steer = steer - velocity;
            return steer;
        }
        return new Vector2();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector2 Align()
    {
        Vector2 sum = new Vector2();
        int count = 0;
        foreach (BoidMouseAI b in boids)
        {
            float d = (location - b.location).magnitude;
            if (d > 0 && d < neighborRadius)
            {
                sum += b.velocity;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= (float)count;
            Vector2 steer = sum - velocity;
            return steer;
        }
        return new Vector2();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector2 Cohesion()
    {
        Vector2 sum = new Vector2();
        int count = 0;

        foreach (BoidMouseAI b in boids)
        {
            float d = (location - b.location).magnitude;
            if (d > 0 && d < neighborRadius)
            {
                sum += b.location;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= (float)count;
            return Seek(sum);
        }
        return new Vector2();
    }



    private float WrapAround()
    {
        Renderer r = GetComponent<Renderer>(); // Should eventually move this as class global for efficiency.
        if (r.isVisible)
        {
            isWrapping = false;
            return 1;
        }

        if (!isWrapping)
        {
            isWrapping = true;
            return -1f;
        }

        return 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector2 StayOnscreen()
    {
        Vector2 newV = velocity;
        Vector3 edge = Camera.main.WorldToViewportPoint(transform.position);

        if (!iwx)
        {
            if (edge.x < 0.1f)
            {
                newV.x = 1;
                iwx = true;
            }
            else if (edge.x > 0.9f)
            {
                newV.x = -1;
                iwx = true;
            }
        }

        if (!iwy)
        {
            if (edge.y < 0.1f)
            {
                newV.y = 1;
                iwy = true;
            }
            else if (edge.y > 0.9f)
            {
                newV.y = -1;
                iwy = true;
            }
        }

        if (edge.x > 0.2f && edge.x < 0.8f)
        {
            iwx = false;
        }

        if (edge.y > 0.2f && edge.y < 0.8f)
        {
            iwy = false;
        }

        return newV;
    }

    void Flee(Vector3 pos)
    {
        _motion.RotateMarker(pos);
        transform.position -= (_motion.GetMarker().up + new Vector3(velocity.x, velocity.y)) * _motion.movementSpeed * Time.deltaTime;
    }

}
