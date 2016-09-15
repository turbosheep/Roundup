using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {

    private Rigidbody2D body;
    private RaycastHit2D hit;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            //hit = Physics2D.Raycast(Input.mousePosition, this.transform.position);
            Vector2 v = Input.mousePosition - this.transform.position;

            body.AddForce(v);
            Debug.Log(body.velocity);
        }
	}
}
