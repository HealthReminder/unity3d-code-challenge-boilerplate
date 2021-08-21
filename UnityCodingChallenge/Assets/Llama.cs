using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class ALlamaProperties : MonoBehaviour
{
    [Header("Properties")]
    //This class handles the properties necessary for the functionality of the script
    //Its different than variables like health and age, that changes dinamically
    //The variables here should not be changed constantly
    //Components 

    //Data Generation
    internal Vector2 ageMinMax = new Vector2(0, 30);
    internal Vector2 healthMinMax = new Vector2(50, 100);
    internal Vector2 boundariesMinMax = new Vector2(-18, 18);
    //Color
    internal float maxColorOffset = 0.3f; //How much color varies between Llamas
    //Movement
    internal float walkSpeed = 0.05f;
    internal float minDistanceToTarget = 0.1f;
    //Rotation
    internal float rotationSpeed = 0.05f;
    internal float minAngleToTarget = 5;
    //Scale
    internal Vector2 minMaxScale = new Vector2(0.5f, 1.5f);

    public AnimationCurve LlamaJumpCurve;
}
public abstract class ALlamaData : ALlamaProperties
{
    [Header("Data")]
    public int PoolId; //This is the ID sent to the pool when deactivation is in order
    public int Health;
    public int Age;
    public InventoryItem Diet;
    public bool GotCaptured = false;
    public int DropQuantity;
}
[System.Serializable]public class IntEvent : UnityEvent<int>
{
}
public abstract class ALlamaEvents : ALlamaData
{
    //This class is responsible for the events
    //Because of the inverse dependency principle
    //The class does not do anything itself, but is set by the pool manager
    [HideInInspector] public IntEvent OnCaptured;                             //Events are set by the LlamaManager beforehand
}
public abstract class ALlamaController : ALlamaEvents
{
    //This class contains all the code for the functionality of the Llamas
    //That includes stuff like movement 
    float jumpProgress = 0;
    public bool JumpTowardsPoint(Vector3 point)
    {
        Vector3 newPos = transform.position;

        //Jump (Only works on flat surfaces in this early implementation as it is not a priority)
        newPos.y = 1 + LlamaJumpCurve.Evaluate(jumpProgress);
        jumpProgress += Time.deltaTime;
        if (jumpProgress >= 1)
            jumpProgress = 0;

        //Movement
        float distanceFromTarget = Vector3.Distance(transform.position, point);
        if (distanceFromTarget > minDistanceToTarget) {
            newPos += (point - transform.position).normalized * walkSpeed;
            transform.position = newPos;

            return false;
        } else
        {
            transform.position = newPos;
            return true;
        }
    }
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

    [ContextMenu("Capture Llama")] public void GetCaptured()
    {
        OnCaptured.Invoke(PoolId);
        OnCaptured = new IntEvent();

    }

}
public abstract class ALlamaView : ALlamaController
{
    [Header("View")]

    //This class handles everything related to the 'appearance'
    //That includes changing colors and having references to related components
    public Renderer Renderer;
    internal MaterialPropertyBlock propertyBlock;
    public void ChangeColor(Color newColor)
    {
        propertyBlock.SetColor("_BaseColor", newColor);
        Renderer.SetPropertyBlock(propertyBlock);
    }
    [ContextMenu("Change To Random Color")] public void ChangeColorRandom()
    {
        float r, g, b;
        r = Random.Range(0.0f, 1.0f) + 0.1f;
        g = Random.Range(0.0f, 1.0f) + 0.1f;
        b = Random.Range(0.0f, 1.0f) + 0.1f;
        Color newColor = Color.white - new Color(r, g, b) * maxColorOffset;
        ChangeColor(newColor);
    }
    public void ChangeScale()
    {
        float interpolatedAge = Mathf.InverseLerp(ageMinMax.x, ageMinMax.y, Age);
        float scaleChange = Mathf.Lerp(minMaxScale.x, minMaxScale.y, interpolatedAge);
        transform.localScale = Vector3.one * scaleChange;
    }
  
}
//[ExecuteInEditMode] //Remove dashes to see how Llamas would be spawned without playing the scene // Warning: Llamas might flotate
public class Llama : ALlamaView
{
    [Header("States")]

    public bool IsOnDestination = true;
    public bool IsFacingDestination = false;
    Vector3 currentDestination;

    void OnEnable()
    {
        Reset();
    }

    [ContextMenu("Reset")] public void Reset()
    {
        //Randomize variables and reset states
        //This is called during pooling and upon scene load
        propertyBlock = new MaterialPropertyBlock();
        Health = Random.Range((int)healthMinMax.x, (int)healthMinMax.y + 1);
        Age = Random.Range((int)ageMinMax.x, (int)ageMinMax.y + 1);
        Diet = (InventoryItem)Random.Range(0, 4);
        DropQuantity = Random.Range(3,6);
        ChangeColorRandom();
        ChangeScale();
        GotCaptured = false;
        IsOnDestination = true;
    }
    
    void Update()
    {
        if (IsOnDestination)
        {
            IsOnDestination = false;
            IsFacingDestination = false;
            currentDestination = new Vector3(Random.Range(boundariesMinMax.x, boundariesMinMax.y), 1.0f, Random.Range(boundariesMinMax.x, boundariesMinMax.y)); //Llama movement is restricted between these coords
        }

        IsOnDestination = JumpTowardsPoint(currentDestination);
        if (!IsFacingDestination)
        {
            IsFacingDestination = RotateTowardsPoint(currentDestination);
        }

    }
}
