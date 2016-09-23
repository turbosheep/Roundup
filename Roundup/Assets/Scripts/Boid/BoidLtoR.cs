using UnityEngine;
using System.Collections.Generic;

public class BoidLtoR : MonoBehaviour {

    public float separation = 1f;
    public float neighborRadius = 5.0f;
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float edgeWeight = 1f;
    public float mForce = 1f;
    public float mSpeed = 1f;
    public float startRepelRange = 1f;
    public float distanceFromEdge = 1f;

    private bool isWrapping = true;  // Used in WrapAround()
    private bool iwx = false;  // Used in StayOnScreen()
    private bool iwy = false;  // Used in StayOnScreen()
    private Vector2 threatDir;
    private GameObject threat;
    private Transform skin;
    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;

    public List<BoidLtoR> boids { get; set; }

    // Use this for initialization
    void Start()
    {
        velocity = Vector2.right;
        acceleration = Vector2.zero;
        location = transform.position;
        threatDir = Vector2.zero;
        skin = transform.FindChild("Skin");
        skin.rotation = Quaternion.FromToRotation(skin.position, velocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (boids != null)
        {
            threat = GameObject.FindGameObjectWithTag("Dog");
            if (threat)
            {
                float dist = Vector2.Distance(transform.position, threat.transform.position);

                if (startRepelRange - dist > 0)
                {
                    float FearFactor = startRepelRange / Mathf.Max(0.1f, (startRepelRange - dist) * 2f);
                    float speed = 1f;
                    threatDir = (transform.position - threat.transform.position).normalized * (speed * FearFactor);
                    velocity += threatDir;
                }
            }


            flock();
            velocity = Vector2.Lerp(velocity, velocity + acceleration * Time.deltaTime, 1);
            velocity = Vector2.ClampMagnitude(velocity, mSpeed);
            velocity.x = velocity.x <= 0.5f ? 0.5f : velocity.x;
            location = Vector2.Lerp(location, location + velocity * Time.deltaTime, 1);
            if(WrapAround() == -1f)
            {
                location.y *= -1;
                location.x = location.x > 0 ? -location.x : location.x; //WRAPS the boid in astroids type manner.

            }


            float rot = Mathf.Atan2(velocity.normalized.y, velocity.normalized.x) * Mathf.Rad2Deg;
            Quaternion finish = Quaternion.Euler(0f, 0f, rot-180f);

            if (threat)
                skin.rotation = Quaternion.Slerp(skin.rotation, finish, Time.deltaTime * 5f);
            else
                skin.rotation = Quaternion.Slerp(skin.rotation, finish, Time.deltaTime);


            //if (Mathf.Abs(skin.rotation.eulerAngles.z) > 90f)
            //{
            //    skin.GetComponent<SpriteRenderer>().flipY = true;
            //}
            //else
            //{
            //    skin.GetComponent<SpriteRenderer>().flipY = false;
            //}

            acceleration = Vector2.zero;
            this.gameObject.transform.position = location;
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
        //Vector2 edge = StayOnscreen();

        //weight them
        sep *= separationWeight;
        ali *= alignmentWeight;
        coh *= cohesionWeight;
        //edge *= edgeWeight;

        //apply to the boid
        ApplyForce(sep);
        ApplyForce(ali);
        ApplyForce(coh);
        //ApplyForce(edge);
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
        //desired.Normalize();
        //if (desired.magnitude > mSpeed)
        //	desired = Vector2.ClampMagnitude(desired, mSpeed);

        //get the seering (desired location - velocity), capped for force
        Vector2 steer = desired - velocity;
        //if (steer.magnitude > mForce)
        //	steer = Vector2.ClampMagnitude(steer, mForce);

        //return the steering vector
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

        foreach (BoidLtoR b in boids)
        {
            Vector2 dist = location - b.location;
            float d = dist.magnitude;
            if (d > 0 && d < separation)
            {
                //dist.Normalize();
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
            //steer.Normalize();
            steer *= mSpeed;
            steer = steer - velocity;
            //if (steer.magnitude > mForce)
            //	steer = Vector2.ClampMagnitude(steer, mForce);
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
        foreach (BoidLtoR b in boids)
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
            //sum = Vector2.ClampMagnitude(sum, mSpeed);

            Vector2 steer = sum - velocity;
            //if (steer.magnitude > mForce)
            //	steer = Vector2.ClampMagnitude(steer, mForce);
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

        foreach (BoidLtoR b in boids)
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
        Renderer r = skin.GetComponent<Renderer>(); // Should eventually move this as class global for efficiency.
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

        //get world vectors to the edge of the screen
        ////Vector2 maxX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, location.y));
        ////Vector2 maxY = Camera.main.ScreenToWorldPoint(new Vector2(location.x, Camera.main.pixelHeight));
        ////Vector2 minX = Camera.main.ScreenToWorldPoint(new Vector2(0f, location.y));
        ////Vector2 minY = Camera.main.ScreenToWorldPoint(new Vector2(location.x, 0f));

        ////if ((maxX - location).magnitude < distanceFromEdge) //if boids are within the distance to the edge, adjust force so they turn away from it
        ////{
        ////    Debug.Log("Edge");
        ////}
        ////else if ((maxY - location).magnitude < distanceFromEdge)
        ////{
        ////    Debug.Log("Edge");
        ////}
        ////else if ((minX - location).magnitude < distanceFromEdge)
        ////{
        ////    Debug.Log("Edge");
        ////}
        ////else if ((minY - location).magnitude < distanceFromEdge)
        ////{
        ////    Debug.Log("Edge");
        ////}

        ////return new Vector2();
    }

}
