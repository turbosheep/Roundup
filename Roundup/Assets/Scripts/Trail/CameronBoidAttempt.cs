using UnityEngine;
using System.Collections;

public class CameronBoidAttempt : MonoBehaviour {

    public float StartRepelRange = 1;
    public float RatioSpeedMultiplier = 0.5f;
    public float EdgeTol = 0;

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
                float speed = 1 + (RatioSpeedMultiplier / dist);
                Vector2 direction = transform.position - dog.transform.position;
                
                Vector3 edge = Camera.main.WorldToViewportPoint(transform.position);
                if (edge.x < (0.0 + EdgeTol) || edge.x > (1.0 - EdgeTol))
                {
                    direction.x = -direction.x;
                }
                if (edge.y < (0.0 + EdgeTol) || edge.y > (1.0 - EdgeTol))
                {
                    direction.y = -direction.y;
                }

                direction = direction.normalized;
                transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + direction, speed * Time.deltaTime);

            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, Vector2.zero, 1 * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, Vector2.zero, 1 * Time.deltaTime);
        }
    }
}
