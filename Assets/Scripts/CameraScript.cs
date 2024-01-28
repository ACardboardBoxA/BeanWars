using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 5f;
    public float screenEdgeDistance = 20f;
    public Camera camera;

    public float minXClamp;
    public float maxXClamp;

    private void Update()
    {
        MoveCameraWithMouse();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            speed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            speed /= 2;
        }
    }

    private void MoveCameraWithMouse()
    {
        float mouseX = Input.mousePosition.x;
        Vector3 newPosition = transform.position;

        if (mouseX >= Screen.width - screenEdgeDistance)
        {
            newPosition += Vector3.right * speed * Time.deltaTime;

            //transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        else if (mouseX <= screenEdgeDistance)
        {
            newPosition += Vector3.left * speed * Time.deltaTime;
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minXClamp, maxXClamp);
        transform.position = newPosition;
    }
}
