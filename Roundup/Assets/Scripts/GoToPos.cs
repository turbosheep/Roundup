using UnityEngine;
using System.Collections;

public class GoToPos : MonoBehaviour
{
    public Vector3 destination = Vector3.zero;
    public float speed = 5;
    public bool finished = true;
    public float walkTime = 2;

    protected float time;

	void Update ()
    {
        time -= Time.deltaTime;
        if(!finished)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            if(time < 0)
            {
                finished = true;
                GetComponent<BoidMouseAI>().enabled = true;
                GetComponent<BoidMouseAI>().UpdateLocation();
            }
        }
	}

    public void Restart(Vector3 pos)
    {
        destination = pos;
        time = walkTime;
        finished = false;
    }
}
