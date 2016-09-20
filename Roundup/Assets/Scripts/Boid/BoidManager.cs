using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour
{

	public GameObject boid;
	public int numBoids = 3;

	public Boid[] flock;

	// Use this for initialization
	void Start()
	{
		flock = new Boid[numBoids];
		for (int i = 0; i < numBoids; i++)
		{
			Vector2 pos = new Vector2(Random.Range(-10f, 10f), Random.Range(-5.0f, 5.0f));
			GameObject temp = Instantiate(boid, pos, Quaternion.identity, this.transform) as GameObject;
			//GameObject temp = Instantiate(boid, new Vector2(), Quaternion.identity, this.transform) as GameObject;
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
	}
}
