using UnityEngine;
using System.Collections;

public class BoidMouseManager : MonoBehaviour
{
    public GameObject boid;
    public int numBoids = 3;

    public BoidMouseAI[] flock;

    // Use this for initialization
    void Start()
    {
        flock = new BoidMouseAI[numBoids];
        for (int i = 0; i < numBoids; i++)
        {
            Vector3 pos = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 10f);
            pos = Camera.main.ViewportToWorldPoint(pos);
            GameObject temp = Instantiate(boid, pos, Quaternion.identity, this.transform) as GameObject;
            //GameObject temp = Instantiate(boid, new Vector2(), Quaternion.identity, this.transform) as GameObject;
            BoidMouseAI b = temp.GetComponent<BoidMouseAI>();
            flock[i] = b;
        }
        foreach (BoidMouseAI b in flock)
        {
            b.boids = flock;
        }
    }
}
