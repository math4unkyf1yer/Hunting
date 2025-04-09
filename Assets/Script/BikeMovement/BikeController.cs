using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BikeController : MonoBehaviour
{
    RaycastHit hit;
    float moveInput, steerInput , rayLenght,rayLenghtUp, currentVelocityOffset;
    private TextMeshProUGUI speedText;
    public float maxSpeed, acceleration, steerStrenght, tiltAngle, gravity, bikeTiltIncrement = 0.09f, zTitlAngle = 45, hadleRotVal = 30f, handleRotSpeed = .15f;
    [Range(1,10)]
    public float brakeFactor;
    public float speedCheck;//use for if the bike blows up;

    //Spawn
    public GameObject spawnPosition;
    private Transform SaveSpawnPoint;
    public GameObject crashEffect;

    [HideInInspector] public Vector3 velocity2;
    public GameObject handle;

    public Rigidbody sphereRb, bikeBody, player;

    public LayerMask drivableLayer;

    //increase speed for speed boost
    private bool isBoosting = false;
    private float originalMaxSpeed;

    //for backflip 
    public float flipSpeed = 5f;
    private float flipProgress = 0f;

    public bool cantCrash;

    //script
    private CameraSwitch cameraScript;
    private SpawnBikeBack spawnScript;
    private Shoot shootScript;



    // Start is called before the first frame update
    void Start()
    {
        cantCrash = true;
        Debug.developerConsoleVisible = true;
        sphereRb.transform.parent = null;
        bikeBody.transform.parent = null;
        sphereRb.drag = 0.1f;  // Lower value makes it slide more
        sphereRb.angularDrag = 1f; // Keeps turning responsive

        cameraScript = gameObject.GetComponent<CameraSwitch>();
        spawnScript = GameObject.Find("SaveSpawnPoint").GetComponent<SpawnBikeBack>();
        SaveSpawnPoint = GameObject.Find("SaveSpawnPoint").GetComponent<Transform>();
        speedText = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>();
        shootScript = GameObject.Find("Ak47Holder").GetComponent<Shoot>();
        rayLenght = sphereRb.GetComponent<SphereCollider>().radius + 4f;
        StartCoroutine(Immortal());
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
       
        transform.position = sphereRb.transform.position;

        speedCheck = sphereRb.velocity.magnitude;
        speedText.text = speedCheck.ToString();

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
        float currentSpeed = sphereRb.velocity.magnitude; // Get current speed
        float targetSpeed = maxSpeed; // Default target speed
        float accelerationRate = acceleration * Time.fixedDeltaTime; // Acceleration factor

        if (moveInput < 0) // Pressing 'S' or down arrow
        {
            targetSpeed = 0; // Decelerate to stop
            accelerationRate *= brakeFactor; // Apply brake factor
        }

        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate); // Smooth transition

        if (OnSlope())
        {
            // Add force in the forward direction to push the bike uphill
            sphereRb.AddForce(transform.forward * acceleration * 10f, ForceMode.Acceleration);
        }
        else
        {
            sphereRb.velocity = transform.forward * newSpeed;
        }

        if (newSpeed <= 40f && cantCrash == false) // If stopped, trigger game over
        {
            //falling over 
            Stalling();
        }
    }
    bool OnSlope()
    {
        if (Grounded())
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > 5f && slopeAngle < 50f; // tweak range for what's considered a ramp
        }
        return false;
    }

    void Stalling()
    {
        // Stop movement
        sphereRb.velocity = Vector3.zero;
        sphereRb.angularVelocity = Vector3.zero;

        // Optional: Disable controls while falling
        moveInput = 0;
        steerInput = 0;
        SaveSpawnPoint.position = spawnPosition.transform.position;
        SaveSpawnPoint.rotation = spawnPosition.transform.rotation;
        bikeBody.gameObject.SetActive(false);
        StartCoroutine(SpawnBack());
    }
    void DestroyBike()
    {
        if(cantCrash == false)
        {
            // Stop movement
            crashEffect.SetActive(true);
            bikeBody.gameObject.SetActive(false);
            StartCoroutine(SpawnBack());
        }
    }
    IEnumerator SpawnBack()
    {
        yield return new WaitForSeconds(2f);
        // Reset position and rotation
        cantCrash = true;
        bikeBody.transform.position = SaveSpawnPoint.position;
        bikeBody.transform.rotation = SaveSpawnPoint.rotation;

        sphereRb.transform.position = SaveSpawnPoint.position;
        sphereRb.transform.rotation = SaveSpawnPoint.rotation;

        // Reset velocity
        sphereRb.velocity = Vector3.zero;
        sphereRb.angularVelocity = Vector3.zero;

        bikeBody.velocity = Vector3.zero;
        bikeBody.angularVelocity = Vector3.zero;

        // Reset any rotation on the handle if needed
        handle.transform.localRotation = Quaternion.identity;

        // Reactivate bike visuals & physics
        bikeBody.gameObject.SetActive(true);
        crashEffect.SetActive(false);

        // Make bike invincible for a few seconds again
        StartCoroutine(Immortal());
    }
    IEnumerator Immortal()
    {
        yield return new WaitForSeconds(7f);
        cantCrash = false;
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

    void PerformBackflip()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           flipProgress += flipSpeed * Time.deltaTime;
            Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x - flipProgress, transform.eulerAngles.y, transform.eulerAngles.z);
            bikeBody.MoveRotation(newRotation);

            if (flipProgress >= 360f)
            {
                shootScript.IncreaseAmmo(1);
                flipProgress = 0f; // Reset after one full rotation
            }
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

    public void CheckCollision()
    {
        if(speedCheck >= 160)
        {
            SaveSpawnPoint.position = spawnPosition.transform.position;
            SaveSpawnPoint.rotation = spawnPosition.transform.rotation;
            DestroyBike();
        }
    }
    public void FlyCrash()
    {
        SaveSpawnPoint.position = spawnPosition.transform.position;
        SaveSpawnPoint.rotation = spawnPosition.transform.rotation;
        DestroyBike();
    }

}
