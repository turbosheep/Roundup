using UnityEngine;
using System.Collections;

public class SimpleTouchController2 : MonoBehaviour {

    //bool frozen = false;
    //List<Vector2> path;
    public GameObject dog;
    private GameObject inactiveDog;

    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);

            if(myTouch.phase == TouchPhase.Began)
            {
                Vector2 path = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                inactiveDog = Instantiate(dog, path, Quaternion.identity) as GameObject;
            }
            else if(myTouch.phase == TouchPhase.Moved || MyTouch.phase == TouchPhase.Stationary)
            {
                 inactiveDog.SendMessage("Move", Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if(myTouch.phase == TouchPhase.Ended)
            {
                DogDoneRunning();
            }
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 path = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inactiveDog = Instantiate(dog, path, Quaternion.identity) as GameObject;
        }
        else if (Input.GetMouseButton(0))
        {
            inactiveDog.SendMessage("Move", Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DogDoneRunning();
        }

#endif
    }

    void DogDoneRunning()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Dog"));
    }
}
