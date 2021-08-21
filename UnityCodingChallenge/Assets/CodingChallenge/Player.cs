using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
public abstract class APlayerProperties : MonoBehaviour
{
    //This class handles the properties used mostly for debug
    //The variables here should not be changed constantly
    //Movement
    internal float walkSpeed = 0.1f;
    internal float minDistanceToDestination = 0.25f;
    //Rotation
    internal float rotationSpeed = 0.01f;
    internal float minAngleToTarget = 5;

}
public abstract class APlayerData : APlayerProperties
{
    public int Health;
    public int Coins;
}
public abstract class APlayerController: APlayerData
{
    public bool RotateTowardsPoint(Vector3 point)
    {
        if (Vector3.Angle(transform.forward, point) <= minAngleToTarget)
            return true;
        else
        {
            Vector3 targetPostition = new Vector3(point.x, transform.position.y, point.z);
            this.transform.LookAt(targetPostition);
            return false;
        }
    }
    public bool MoveTowardsPoint(Vector3 point, float minDistance)
    {
        if (Vector3.Distance(transform.position, point) <= minDistance)
            return true;
        else
        {
            transform.position += (point - transform.position).normalized * walkSpeed;
            return false;
        }        
    }

    public void GetInformationFromRay(Camera cam,out bool isHit, out Vector3 hitPos, out GameObject hitInteractable)
    {
        isHit = false;
        hitPos = Vector3.zero;
        hitInteractable = null;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            isHit = true;
            hitPos = hit.point;
            if (hit.transform.tag == "Interactable")
                hitInteractable = hit.transform.gameObject;
        }
    }
}
public abstract class APlayerView : APlayerController
{
    public Animator Animator;
    public void SetAnimationIdle(bool isActive)
    {
        Animator.SetBool("IsIdle", isActive);
    }

}
public class Player : APlayerView
{
    public Camera PlayerCamera;
    public Vector3 currentDestination;
    public bool IsInteracting = false;
    public bool IsOnDestination = false;
    public bool IsFacingDestination = false;
    private void Awake()
    {
        currentDestination = transform.position;
    }

    private void Update()
    {
        //Rotate if not facing it yet
        if (!IsFacingDestination)
            IsFacingDestination = RotateTowardsPoint(currentDestination);

        //Move if not there yet and not busy or idle
        if (!IsInteracting)
        {
            if (!IsOnDestination)
                IsOnDestination = MoveTowardsPoint(currentDestination, minDistanceToDestination);
            else 
                SetAnimationIdle(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnPlayerClick();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interactable")
        {
            if (other.GetComponent<Llama>())
            {
                Debug.Log("Player captured a Llama.");
                Llama capturedLlama = other.GetComponent<Llama>();
                capturedLlama.GetCaptured();
            } else if (other.GetComponent<PickableItem>())
            {
                PickableItem collectedItem = other.GetComponent<PickableItem>();
                collectedItem.Collect(out int moneyGain, out int healthGain);
                Coins += moneyGain;
                Health += healthGain;
                Debug.Log($"Player collected an object and gained ${moneyGain}, and {healthGain} HP.");
            }

        }
    }
    void OnPlayerClick()
    {
        //Player is trying to interact with the world
        GetInformationFromRay(PlayerCamera, out bool isValid, out Vector3 mousePos, out GameObject interactingWith);
        if (isValid)
        {
            bool interactionResult = false;
            if (interactingWith != null)
            {
                interactionResult = false;
                Debug.Log($"Player interacted with building with a {interactionResult} result.");
                SetAnimationIdle(true);
            }
            if (!interactionResult)
            {
                //Player could not interact with object
                //That means that it has a new target to go to
                currentDestination = mousePos;
                IsOnDestination = false;
                IsFacingDestination = false;
                SetAnimationIdle(false);

            }
        }

    }

}
