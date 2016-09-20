using UnityEngine;
using System.Collections;

public class DummyMotionAI : MonoBehaviour
{
	//If your mouse is on the screen, it will repel the object
    //If you press the left mouse button it will attact the object
	void Update ()
    {
        if(Input.GetMouseButton(0))
        {
            GetComponent<DummyMotion>().Attract(transform);
        }
        else
        {
            GetComponent<DummyMotion>().Repel(transform);
        }
	
	}
}
