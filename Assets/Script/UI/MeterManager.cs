using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterManager : MonoBehaviour
{
    public BikeController bikeController;

    public GameObject needle;


    // Starting and end positions for needle (based on z axis)
    private float startPos = 185f, endPos = -11f;
    private float desiredPosition;


    // The speed of the vehicle
    private float vehicleSpeed;


    // Max speed of vehicle
    [SerializeField]private int maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate() {
        vehicleSpeed = bikeController.speedCheck;
        updateNeedle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateNeedle() {
        desiredPosition = startPos - endPos;
        float temp = vehicleSpeed / maxSpeed;

        // Set new needle position every frame
        needle.transform.eulerAngles = new Vector3(0, 0,(startPos - temp * desiredPosition));

    }
}
