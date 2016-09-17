using UnityEngine;
using System.Collections.Generic;

public class SimpleTouchController1 : MonoBehaviour {

    bool frozen = false;
    List<Vector2> path;
    public GameObject dog;

    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID
        if (Input.touchCount > 0 && !frozen)
        {
            Touch myTouch = Input.GetTouch(0);

            if(myTouch.phase == TouchPhase.Began)
            {
                Camera.main.backgroundColor = Color.red;
                path = new List<Vector2>();
            }
            else if(myTouch.phase == TouchPhase.Moved)
            {
                path.Add(myTouch.position);
            }
            else if(myTouch.phase == TouchPhase.Ended)
            {
                frozen = true;
                Camera.main.backgroundColor = Color.blue;
                GameObject.Instantiate(dog, new Vector3(path[0].x, path[0].y, 0), Quaternion.identity);
                GameObject.Find("Dog").SendMessage("StartRun", path);
            }
        }
#elif UNITY_EDITOR
        if (!frozen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Camera.main.backgroundColor = Color.red;
                path = new List<Vector2>();
            }
            else if (Input.GetMouseButton(0))
            {
                path.Add((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                frozen = true;
                //Camera.main.backgroundColor = Color.blue;
                path.Add((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
                GameObject.Instantiate(dog, new Vector3(path[0].x, path[0].y, 0), Quaternion.identity);
                GameObject.FindGameObjectWithTag("Dog").SendMessage("StartRun", path.ToArray());
            }
        }
#endif
    }

    void DogDoneRunning()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Dog"));
        frozen = false;
        path.Clear();
    }
}
