using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class APlayerProperties : MonoBehaviour
{
    //This class handles the properties used mostly for debug
    //The variables here should not be changed constantly
    //Movement
    internal float walkSpeed = 0.05f;
    internal float minDistanceToDestination = 0.25f;
    //Rotation
    internal float rotationSpeed = 0.01f;
    internal float minAngleToTarget = 5;

}
public abstract class APlayerData : APlayerProperties
{
    public int Health;
    public int Coins;
    public Inventory Inventory;

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
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
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
                if(Vector3.Distance(transform.position, hit.point) <= 4)
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
    public bool hasInput = false;
    public Vector3 currentDestination;
    public bool IsInteracting = false;
    public bool IsOnDestination = false;
    public bool IsFacingDestination = false;
    private void Awake()
    {

        currentDestination = transform.position;
        UpdateLocalPlayer();
    }
    public void UpdateLocalPlayer()
    {
        PersistentData.UpdateLocalPlayer(out Coins, out Health, out Dictionary<ItemType, int> countList, out Dictionary<ItemType, string> pathList);
        if (countList == null)
            countList = new Dictionary<ItemType, int>();
        if (pathList == null)
            pathList = new Dictionary<ItemType, string>();
        Inventory = new Inventory(countList, pathList);


    }
    private void Update()
    {
        if (!hasInput)
            return;
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Updating inventory with item count of: " + Inventory.itemToCount.Keys.Count);
            PersistentData.UpdatePersistentPlayer(Coins, Health, Inventory);
            PersistentData.Debug();
            SceneManager.LoadScene(1);//Loads shop to test persistent data
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Updating inventory with item count of: " + Inventory.itemToCount.Keys.Count);
            PersistentData.UpdatePersistentPlayer(Coins, Health, Inventory);
            PersistentData.Debug();
            SceneManager.LoadScene(2);//Loads shop to test persistent data
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interactable")
        {
            if (other.GetComponent<Llama>())
            {
                Llama capturedLlama = other.GetComponent<Llama>();
                capturedLlama.GetCaptured();
                Debug.Log("Player captured a Llama.");
            }
            else if (other.GetComponent<PickableItem>())
            {
                PickableItem collectedItem = other.GetComponent<PickableItem>();
                collectedItem.Collect(out int moneyGain, out int healthGain, out string prefabPath);
                Coins += moneyGain;
                Health += healthGain;
                Inventory.AddItem(collectedItem, prefabPath);
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
                PersistentData.UpdatePersistentPlayer(Coins, Health, Inventory);
                if (interactingWith.GetComponent<Building>())
                    interactingWith.GetComponent<Building>().Enter();
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
