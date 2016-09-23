using UnityEngine;
using System.Collections.Generic;

public class BoidLtoRManager : MonoBehaviour {

    public GameObject boid;
    public int maxBoids = 3;
    public float spawnRate = 1;
    public List<BoidLtoR> flock;

    private float spawnTimer = 0;
    private int currentBoids = 0;

    // Use this for initialization
    void Start()
    {
        flock = new List<BoidLtoR>();
        spawnTimer = Time.time + spawnRate;      
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > spawnTimer && flock.Count < maxBoids)
        {
            Vector3 pos = new Vector3(-0.1f, Random.Range(0.1f, 0.9f), 10f);
            pos = Camera.main.ViewportToWorldPoint(pos);
            GameObject temp = Instantiate(boid, pos, Quaternion.identity, this.transform) as GameObject;
            BoidLtoR b = temp.GetComponent<BoidLtoR>();

            flock.Add(b);
            flock[currentBoids++].boids = flock;

            spawnTimer = Time.time + spawnRate;
        }
    }

    void DeleteBoid(GameObject boid)
    {
        flock.Remove(boid.GetComponent<BoidLtoR>());
        currentBoids--;
        Destroy(boid);

    }
}
