using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
public abstract class APlayerProperties : MonoBehaviour
{
    internal float walkSpeed = 0.1f;
    internal float minDistanceToTarget = 0.25f;
}
public abstract class APlayerData : APlayerProperties
{
    public int Health;
    public int Coins;
}
public abstract class APlayerController: APlayerData
{
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
public abstract class APlayerAnimations : APlayerController
{
    public Animator Animator;
    public void SetAnimationIdle(bool isActive)
    {
        Animator.SetBool("IsIdle", isActive);
    }

}
public class Player : APlayerAnimations
{
    public Camera PlayerCamera;
    public Vector3 currentTarget;
    public bool IsInteracting = false;
    public bool IsOnTarget = false;
    private void Awake()
    {
        currentTarget = transform.position;
    }
    private void Update()
    {
        if (!IsInteracting)
            if(!IsOnTarget)
                IsOnTarget = MoveTowardsPoint(currentTarget, minDistanceToTarget);
        
        if(IsOnTarget)
            SetAnimationIdle(true);


        if (Input.GetMouseButtonDown(0))
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
                    currentTarget = mousePos;
                    IsOnTarget = false;
                    SetAnimationIdle(false);

                }
            }

        }
    }
 
}
