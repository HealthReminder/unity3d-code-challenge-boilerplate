using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ACameraProperties : MonoBehaviour
{
    internal float minDistance = 3f;
    internal float maxDistance = 30.0f;
    internal float movementSpeed = 5f;

}
public abstract class ACameraData : ACameraProperties
{
    public Transform focus_transform;
}
public abstract class ACameraController : ACameraData
{
    public void MoveVertical(float input, float modifier = 1.0f)
    {
        float dotProduct = Vector3.Dot(transform.forward, focus_transform.up);
        if (dotProduct > -0.97f && dotProduct < -0.8f)
            transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime * input * modifier);
    }
    public void MoveHorizontal(float input, float modifier = 1.0f)
    {
        transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime * input * modifier);
    }
    public void Zoom(float input, float modifier = 1.0f)
    {
        Vector3 newPos = transform.position + (transform.forward * movementSpeed * Time.deltaTime * 200 * input * modifier);
        float distToTarget = Vector3.Distance(newPos, focus_transform.position);
        if (distToTarget > minDistance && distToTarget < maxDistance)
            transform.position = newPos;
    }
}
public class OverheadCamera : ACameraController
{
    public float fastMovementSpeed = 50f;
    void Update()
    {

        if (!focus_transform)
            return;

        //bool fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        //float movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;

        //Depth
        float inputScrollwheel = Input.GetAxis("Mouse ScrollWheel");
        if (inputScrollwheel != 0f)
            Zoom(inputScrollwheel);

        //Left and Right
        float inputHorizontal= Input.GetAxis("Horizontal");
        if (inputHorizontal != 0)
            MoveHorizontal(inputHorizontal);

        //Up and Down
        //float inputVertical = Input.GetAxis("Vertical");
        //if (inputVertical != 0)
        //    MoveVertical(inputVertical);

        transform.LookAt(focus_transform.position);
    }

}
