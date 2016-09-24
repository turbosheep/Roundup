using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleEnter : MonoBehaviour
{
    public List<Collider> TriggerList;

    void Update()
    {
        Debug.Log("Fishy: " + TriggerList.Count);
    }

    void onCollisionEnter(Collider other)
    {
        Debug.Log("Enter");
        TriggerList.Add(other);
    }

    void onCollisionExit(Collider other)
    {
        Debug.Log("Exit");
        TriggerList.Remove(other);
    }
}
