using UnityEngine;
using System.Collections;

public class GoalAreaManager : MonoBehaviour {

    public GameObject flock;
	// Use this for initialization

    void OnTriggerEnter2D(Collider2D col)
    {
        flock.SendMessage("DeleteBoid", col.gameObject);
    }
}
