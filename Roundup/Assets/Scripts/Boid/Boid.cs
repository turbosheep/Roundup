using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{

	public float separation = 1f;
	public float neighborRadius = 5.0f;

	public float separationWeight = 1f;
	public float alignmentWeight = 1f;
	public float cohesionWeight = 1f;

	private Vector2 location;
	private Vector2 velocity;
	private Vector2 acceleration;

	public Boid[] boids { get; set; }

	public float mForce = 1f;
	public float mSpeed = 1f;

	// Use this for initialization
	void Start()
	{
		velocity = new Vector2();
		acceleration = new Vector2();
		location = this.gameObject.transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (boids != null)
		{
			flock();
			velocity += acceleration;
			velocity = Vector2.ClampMagnitude(velocity, mSpeed);
			location += velocity * Time.deltaTime;
			acceleration = Vector2.zero;
			this.gameObject.transform.position = location;
		}
	}

	private void flock()
	{

		//find the three components of the flock
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
		Vector2 desired = location - target;
		desired.Normalize();
		if (desired.magnitude > mSpeed)
			desired = Vector2.ClampMagnitude(desired, mSpeed);

		//get the seering (desired location - velocity), capped for force
		Vector2 steer = desired - velocity;
		if (steer.magnitude > mForce)
			steer = Vector2.ClampMagnitude(steer, mForce);

		//return the steering vector
		return steer;
	}

	private Vector2 Separate()
	{
		Vector2 steer = new Vector2();
		int count = 0;

		foreach (Boid b in boids)
		{
			Vector2 dist = location - b.location;
			float d = dist.magnitude;
			if (d > 0 && d < separation)
			{
				dist.Normalize();
				dist /= d;
				steer += dist;
				count++;
			}
			if (count > 0)
			{
				steer /= (float)count;
			}
			if (steer.magnitude > 0)
			{
				steer.Normalize();
				steer *= mSpeed;
				steer = steer - velocity;
				if (steer.magnitude > mForce)
					steer = Vector2.ClampMagnitude(steer, mForce);
				return steer;
			}
		}
		return new Vector2();
	}

	private Vector2 Align()
	{
		Vector2 sum = new Vector2();
		int count = 0;
		foreach (Boid b in boids)
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
			sum = Vector2.ClampMagnitude(sum, mSpeed);

			Vector2 steer = sum - velocity;
			if (steer.magnitude > mForce)
				steer = Vector2.ClampMagnitude(steer, mForce);
			return steer;
		}
		return new Vector2();
	}

	private Vector2 Cohesion()
	{
		Vector2 sum = new Vector2();
		int count = 0;

		foreach (Boid b in boids)
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

	private Vector2 StayOnscreen()
	{
		Vector2 maxX = new Vector2(Camera.main.pixelWidth, 0f);
		Vector2 maxY = new Vector2(0, Camera.main.pixelHeight);

		if (Camera.main.ScreenToWorldPoint(maxX).x < (location + velocity).x)
			;

		
		return new Vector2();
	}

}
