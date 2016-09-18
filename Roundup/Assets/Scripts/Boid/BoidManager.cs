using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour
{

	public GameObject boid;
	public int numBoids = 3;

	public Boid[] flock;

	public Vector2 flockCenter;
	public Vector2 flockVelocity;

	// Use this for initialization
	void Awake()
	{
		flock = new Boid[numBoids];
		for (int i = 0; i < numBoids; i++)
		{
			Vector2 pos = new Vector2(Random.Range(-10f, 10f), Random.Range(-5.0f, 5.0f));
			//GameObject temp = Instantiate(boid, pos, Quaternion.identity, this.transform) as GameObject;
			GameObject temp = Instantiate(boid, new Vector2(), Quaternion.identity, this.transform) as GameObject;
			Boid b = temp.GetComponent<Boid>();
			flock[i] = b;
		}
		foreach (Boid b in flock)
		{
			b.boids = flock;
		}
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 c = Vector3.zero;
		Vector2 v = Vector2.zero;

		foreach (Boid boid in flock)
		{
			c += boid.transform.localPosition;
			v += boid.GetComponent<Rigidbody2D>().velocity;
		}

		flockCenter = c / numBoids;
		flockVelocity = v / numBoids;

	}
}
