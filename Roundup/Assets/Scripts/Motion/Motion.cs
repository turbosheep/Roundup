using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour
{
    //This class assumes that the attatched object has a child GameObject with a transform
    //The transform and rotation of the object and the child both control the movement of the object

    public float movementSpeed = 3;     //How quickly the object moves
    public float rotationSpeed = 3;     //How quickly the object rotates to face direction
    public float smallRadius = 2;       //Personal space radius
    public float largeRadius = 20;      //Attractive radius
    public float edgeBoarder = 10;      //Amount of space away from the edge of screen that stops movement

    protected Rect screenRect;
    protected Rect playRect;
    protected Transform marker;
    protected bool fleeing;
    protected GameObject[] boidList;

	// Used for initialization
	void Start ()
    {
        tag = "Boid";
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        playRect = new Rect(edgeBoarder, edgeBoarder, Screen.width - (2 * edgeBoarder), Screen.height - (2 * edgeBoarder));
        marker = transform.GetChild(0);
    }

    void Awake ()
    {
        //Get the boidList but remove yourself from the list
        GameObject[] list = GameObject.FindGameObjectsWithTag("Boid");
        boidList = new GameObject[list.Length - 1];
        int index = 0;
        for(int i = 0; i < list.Length; i++)
        {
            Debug.Log("for loop");
            if (boidList[i] != this)
            {
                Debug.Log("expand");
                boidList[index] = list[i];
                index++;
            }
        }
        Debug.Log(boidList.Length);
    }

    // Main AI logic
    void Update ()
    {
        Vector3 mousePos = GetMousePos();
        if(screenRect.Contains(Input.mousePosition))
        {
            if(Input.GetMouseButton(0))
            {
                Attract(mousePos);
            }
            else
            {
                Repel(mousePos);
            }
        }
    }

    //Gets the mouse position
    protected Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        return mousePos;
    }

    //Moves obj away from the Vector3 position
    public void Repel(Vector3 pos)
    {
        RotateMarker(pos);
        RotateObject(pos);
        transform.position -= (marker.up + transform.up) * movementSpeed * Time.deltaTime;
    }

    //Moves obj towards the Vector3 position
    public void Attract(Vector3 pos)
    {
        RotateMarker(pos);
        RotateObject(pos);
        transform.position += (marker.up + transform.up) * movementSpeed * Time.deltaTime;
    }

    //Rotates the marker to the Vector3 pos
    protected void RotateMarker(Vector3 pos)
    {
        Vector3 diff = pos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        marker.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    //Rotates the transform to the Vector3 pos
    protected void RotateObject(Vector3 pos)
    {
        Vector3 diff = pos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion finish = Quaternion.Euler(0f, 0f, rot_z - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, finish, rotationSpeed * Time.deltaTime);
    }
}
