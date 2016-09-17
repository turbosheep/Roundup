using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour {

    public GameObject boid;
    public int numBoids = 3;

    public GameObject[] flock;

    public Vector2 flockCenter;
    public Vector2 flockVelocity;

	// Use this for initialization
	void Start () {
        GameObject parent = new GameObject();
        parent.name = "flock";
        flock = new GameObject[numBoids];
	    for(int i = 0; i < numBoids; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-10f, 10f), Random.Range(-5.0f, 5.0f));
            GameObject b = Instantiate(boid, pos, Quaternion.identity, parent.transform) as GameObject;
            flock[i] = b;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 c = Vector3.zero;
        Vector2 v = Vector2.zero;

        foreach(GameObject boid in flock)
        {
            c += boid.transform.localPosition;
            v += boid.GetComponent<Rigidbody2D>().velocity;
        }

        flockCenter = c / numBoids;
        flockVelocity = v / numBoids;
	
	}
}
