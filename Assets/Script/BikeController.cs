using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BikeController : MonoBehaviour
{
    RaycastHit hit;
    float moveInput, steerInput , rayLenght, currentVelocityOffset;
    public TextMeshProUGUI speedText;
    public float maxSpeed, acceleration, steerStrenght, tiltAngle, gravity, bikeTiltIncrement = 0.09f, zTitlAngle = 45, hadleRotVal = 30f, handleRotSpeed = .15f;
    public float minimumSpeed = 100;
    [Range(1,10)]
    public float brakeFactor;

    [HideInInspector] public Vector3 velocity2;
    public GameObject handle;

    public Rigidbody sphereRb, bikeBody;

    public LayerMask drivableLayer;

    //increase speed for speed boost
    private bool isBoosting = false;
    private float originalMaxSpeed;

    //for backflip 
    public float flipSpeed = 5f;
    private float flipProgress = 0f;



    //For shooting
    public Transform cameraTransform;
    public LayerMask enemyLayer;


    // Start is called before the first frame update
    void Start()
    {
        Debug.developerConsoleVisible = true;
        sphereRb.transform.parent = null;
        bikeBody.transform.parent = null;
        sphereRb.drag = 0.1f;  // Lower value makes it slide more
        sphereRb.angularDrag = 1f; // Keeps turning responsive

        rayLenght = sphereRb.GetComponent<SphereCollider>().radius + 6f;
        //cameraTransform = GetComponent<Camera>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
       
        transform.position = sphereRb.transform.position;

        speedText.text = sphereRb.velocity.magnitude.ToString();

        velocity2 = bikeBody.transform.InverseTransformDirection(bikeBody.velocity);
        currentVelocityOffset = velocity2.z / maxSpeed;
       
    }

    private void FixedUpdate()
    {

        
        Movement();
      //  Brake();
    }

    void Movement()
    {
        if (Grounded())
        {
            Acceleration();
            Rotation();
            BikeTilt();
        }
        else
        {
            PerformBackflip();
            Gravity();
        }
    }
    void Acceleration()
    {
        float targetSpeed = maxSpeed; // The speed we want to reach
        float accelerationRate = acceleration * Time.fixedDeltaTime; // Speed up gradually

        // Increase velocity gradually
        float currentSpeed = sphereRb.velocity.magnitude; // Get current speed
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate); // Smooth acceleration

        sphereRb.velocity = transform.forward * newSpeed; // Apply new speed
        Debug.Log(sphereRb.velocity.magnitude);
    }

    void Rotation()
    {
        // Tilt-based steering: The more the player tilts, the sharper the turn
        float turnAmount = steerInput * steerStrenght * Time.fixedDeltaTime;
        transform.Rotate(0, turnAmount, 0, Space.World);

        // Handle rotation to match steering input
        handle.transform.localRotation = Quaternion.Slerp(
            handle.transform.localRotation,
            Quaternion.Euler(handle.transform.localRotation.eulerAngles.x, hadleRotVal * steerInput, handle.transform.localRotation.eulerAngles.z),
            handleRotSpeed
        );
    }
    public void ActivateSpeedBoost(float boostAmount, float duration)
    {
        if (!isBoosting)
        {
            isBoosting = true;
            originalMaxSpeed = maxSpeed;
            maxSpeed += boostAmount; // Temporarily increase max speed

            StartCoroutine(GradualSpeedBoost(boostAmount, duration)); // Apply gradual speed increase
        }
    }

    private IEnumerator GradualSpeedBoost(float boostAmount, float duration)
    {
        float elapsedTime = 0f;
        float initialSpeed = sphereRb.velocity.magnitude;
        float targetSpeed = initialSpeed + boostAmount;

        // Increase speed over half of the duration (1 sec)
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float newSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / (duration / 2));
            sphereRb.velocity = transform.forward * newSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(duration / 2); // Maintain speed for a brief moment

        elapsedTime = 0f;
        // Gradually return speed back to normal over the remaining time
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float newSpeed = Mathf.Lerp(targetSpeed, originalMaxSpeed, elapsedTime / (duration / 2));
            sphereRb.velocity = transform.forward * newSpeed;
            yield return null;
        }

        maxSpeed = originalMaxSpeed;
        isBoosting = false;
    }

    void BikeTilt()
    {
        float xRot = (Quaternion.FromToRotation(bikeBody.transform.up, hit.normal) * bikeBody.transform.rotation).eulerAngles.x;
        float zRot = -zTitlAngle * steerInput; // Tilt the bike based on steering input

        Quaternion targetRot = Quaternion.Slerp(
            bikeBody.transform.rotation,
            Quaternion.Euler(xRot, transform.eulerAngles.y, zRot),
            bikeTiltIncrement
        );

        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);
        bikeBody.MoveRotation(newRotation);
    }
    void PerformBackflip()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           flipProgress += flipSpeed * Time.deltaTime;
            Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x - flipProgress, transform.eulerAngles.y, transform.eulerAngles.z);
            bikeBody.MoveRotation(newRotation);

            if (flipProgress >= 360f)
            {
                flipProgress = 0f; // Reset after one full rotation
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sphereRb.velocity *= brakeFactor / 10;
        }
    }
    bool Grounded()
    {
        if(Physics.Raycast(sphereRb.position,Vector3.down,out hit, rayLenght, drivableLayer))
        {
            return true;
        }
        else { return false; }
    }
    void Gravity()
    {
        sphereRb.AddForce(gravity * Vector3.down, ForceMode.Acceleration);
    }
 
  /*  void Shoot() {

        Debug.Log("Reached shoot method");
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, enemyLayer))
        {

            Debug.Log("Hit!");

        }
    }*/
}
