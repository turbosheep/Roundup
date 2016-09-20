using UnityEngine;
using System.Collections;

public class DummyMotion : MonoBehaviour
{
    public float speed = 3;
    protected Rect screenRect;

    // Used for initialization
    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    //Moves obj away from the mouse
    public void Repel(Transform obj)
    {
        if (screenRect.Contains(Input.mousePosition))
        {
            Rotate(obj);
            obj.position -= obj.up * speed * Time.deltaTime;
        }
    }

    //Moves obj towards the mouse
    public void Attract(Transform obj)
    {
        if (screenRect.Contains(Input.mousePosition))
        {
            Rotate(obj);
            obj.position += obj.up * speed * Time.deltaTime;
        }
    }

    //Gets the mouse position
    protected Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        return mousePos;
    }

    //Rotates the transform to the mouse point
    protected void Rotate(Transform obj)
    {
        Vector3 mousePos = GetMousePos();
        Vector3 diff = mousePos - obj.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
