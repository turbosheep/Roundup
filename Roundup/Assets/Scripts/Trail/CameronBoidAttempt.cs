using UnityEngine;
using System.Collections;

public class CameronBoidAttempt : MonoBehaviour {


    public float StartRepelRange = 1;
    public float RatioSpeedMultiplier = 0.5f;


    private GameObject dog;
    private Vector3 startPos;

	// Use this for initialization
	private void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	private void Update () {

        dog = GameObject.FindGameObjectWithTag("Dog");

        if (dog)
        {
            float dist = Vector2.Distance(transform.position, dog.transform.position);

            if(StartRepelRange - dist > 0)
            {
                float speed = 1 + (dist / RatioSpeedMultiplier);
                Vector2 direction = transform.position - dog.transform.position;
                direction = direction.normalized;

                transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + direction, speed * Time.deltaTime);

            }

        }
	}
}
