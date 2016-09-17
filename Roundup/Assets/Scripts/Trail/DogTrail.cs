using UnityEngine;
using System.Collections;

public class DogTrail : MonoBehaviour {

    public float speed = 1;

    private Vector2[] path;

    public void StartRun(Vector2[] p)
    {
        path = p;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        foreach (Vector2 p in path)
        {
            while (Vector2.Distance(p, transform.position) > 1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, p, speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, p, Time.deltaTime);
                yield return null;
            }
        }

        GameObject.Find("Main Camera").SendMessage("DogDoneRunning");
    }
}
