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
			Vector3 pos = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 10f);
            pos = Camera.main.ViewportToWorldPoint(pos);
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
