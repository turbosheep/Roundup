using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour
{
    //This class assumes that the attatched object has a child GameObject with a transform
    //The transform and rotation of the object and the child both control the movement of the object

    public float movementSpeed = 3;     //How quickly the object moves
    public float rotationSpeed = 3;     //How quickly the object rotates to face direction
    public float sightRadius = 3;       //How close the mouse can get before it flees.

    protected Rect screenRect;
    protected Transform marker;

	// Used for initialization
	void Start ()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        marker = transform.GetChild(0);
    }

    // Main AI logic
    public void DoRepel ()
    {   
        Vector3 mousePos = GetMousePos();
        if(screenRect.Contains(Input.mousePosition))
        {
            Repel(mousePos);
        }
    }

    //Gets the mouse position
    public Vector3 GetMousePos()
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
    public void RotateMarker(Vector3 pos)
    {
        Vector3 diff = pos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        marker.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    //Rotates the transform to the Vector3 pos
    public void RotateObject(Vector3 pos)
    {
        Vector3 diff = pos - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion finish = Quaternion.Euler(0f, 0f, rot_z - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, finish, rotationSpeed * Time.deltaTime);
    }

    //Gets the screen rect
    public Rect GetRect()
    {
        return screenRect;
    }

    public Transform GetMarker()
    {
        return marker;
    }
}
