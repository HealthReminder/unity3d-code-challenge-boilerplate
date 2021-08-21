using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ALlamaProperties : MonoBehaviour
{
    [Header("Properties")]
    //This class handles the properties used mostly for debug
    //The variables here should not be changed constantly

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
    public int Health;
    public int Age;
    public InventoryItem Diet;
    public bool GotCaptured = false;
    public GameObject DropPrefab;
    public int DropQuantity;
}
public abstract class ALlamaController : ALlamaData
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
        for (int i = 0; i < DropQuantity; i++)
        {
            Vector3 _randomPosition = transform.position + new Vector3(Random.Range(0.1f, 0.5f), 0, Random.Range(0.1f, 0.5f));
            Vector3 _randomRotation = new Vector3(0, Random.Range(0.0f, 180.0f), 0);
            Instantiate(DropPrefab, transform.position + _randomPosition, Quaternion.Euler(_randomRotation), null);
        }
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

    // Start is called before the first frame update
    void OnEnable()
    {
        Reset();
    }

    [ContextMenu("Reset")] public void Reset()
    {
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
    // Update is called once per frame
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
