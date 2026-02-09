using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class CarControl : MonoBehaviour
{
    
    public float rotAngle, steerSpeed;
    public float steeringRangeAtMaxSpeed = 20;
    public float centreOfGravityOffset = -1f;
    public float steeringRange = 45;
    float maxSpeed;
    float forwardSpeed;
    float engineTorque = 2000;
    float motorTorqueGear = 2000;
    float steerOutput;
    float hInput;
    float vInput;
    public float engineRpm;
    [HideInInspector]
    float realSpeed;
    float idleRpm;
    float driveRpm;
    public float driveDia;
    float speedFactor;
    float maxRpm = 7000;
    float currentSteerRange;
    float currentMotorTorque;
    float nonDriveTorque;
    public int gear;
    bool isAccelerating;
    public bool steeringHelp;
    InputAction steerActionKeyboard;
    InputAction steerActionGamepad;
    InputAction accelAction;
    InputAction gearUp;
    InputAction gearDown;
    WheelControl[] wheels;
    public WheelCollider driveWheelRight;
    public WheelCollider driveWheelLeft;
    Rigidbody rigidBody;
    public GlobalSettings gs;
    public TMP_Text gearText;
    public TMP_Text speedText;
    public TMP_Text rpmText;
    public Image rpmNeedle;
    // Start is called before the first frame update
    void Start()
    {
        
        gearUp = InputSystem.actions.FindAction("GearUp");
        gearDown = InputSystem.actions.FindAction("GearDown");
        accelAction = InputSystem.actions.FindAction("Accel");
        steerActionKeyboard = InputSystem.actions.FindAction("SteerKeyboard");
        steerActionGamepad = InputSystem.actions.FindAction("SteerGamepad");
        rigidBody = GetComponent<Rigidbody>();
        FindFirstObjectByType<EngineSoundManager>().Play("EngineNoise");
        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        idleRpm = 1000;
        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();
        steerSpeed = 10;
        
        Shift();
    }

    // Update is called once per frame
    void Shift(){
        if (gear>=0){
            gearText.text = gear.ToString() + " Gear";
        }else{
            gearText.text = "R" + " Gear";
        }
        
        switch (gear)
        {
            case 1:
                motorTorqueGear = 5500;
                nonDriveTorque = 2;
                maxSpeed = 19f;
                break;
            case 2:
                motorTorqueGear = 5000;
                nonDriveTorque = 2;
                maxSpeed = 25.0342f;
                break;
            case 3:
                motorTorqueGear = 4750;
                nonDriveTorque = 2;
                maxSpeed = 35.7632f;
                break;
            case 4:
                motorTorqueGear = 4250;
                nonDriveTorque = 2;
                maxSpeed = 48.2803f;
                break;
            case 5:
                motorTorqueGear = 3500;
                nonDriveTorque = 2;
                maxSpeed = 62.1386f;
                break;
            case 6:
                motorTorqueGear = 2500;
                nonDriveTorque = 2;
                maxSpeed = 75.1027f;
                break;
            case -1:
                motorTorqueGear = -5500;
                nonDriveTorque = -2;
                maxSpeed = -15;
                break;
            default:
                motorTorqueGear = .001f;
                nonDriveTorque = .002f;
                maxSpeed = 0;
                break;
        }
    }
    void Update()
    {
        engineTorque = motorTorqueGear*Mathf.Abs(Mathf.Clamp((Mathf.Sqrt(2f*(((Mathf.Abs(engineRpm-1000))/10000)))+0.1f-Mathf.Pow(((Mathf.Abs(engineRpm-1000))/10000), 2f)), .01f, 10));
        driveRpm = (driveWheelLeft.rpm+driveWheelRight.rpm)/2;
        rpmNeedle.transform.rotation = Quaternion.Euler(rpmNeedle.transform.rotation.x, rpmNeedle.transform.rotation.y, engineRpm/100);
        vInput = accelAction.ReadValue<float>();
        if (gearUp.WasPressedThisFrame() && gear != 6)
        {
            gear +=1;
            Shift();
        }
        if (gearDown.WasPressedThisFrame() && gear != -1)
        {
            gear -=1;
            Shift();
        }
        //IsGamepad
        if (Mathf.Abs(steerActionGamepad.ReadValue<float>())>0){
            hInput = steerActionGamepad.ReadValue<float>();
            gs.isGamepad = true;
        }else {
            hInput = steerActionKeyboard.ReadValue<float>();
            gs.isGamepad = false;
        }

        if(gs.isGamepad){
            //Gamepad Steering
            steerOutput = hInput;
        }else{
            //smooth steering
            if (Mathf.Sign(hInput) != Mathf.Sign(steerOutput)){
                steerOutput -= Mathf.Sign(steerOutput) * Time.deltaTime * steerSpeed; 
            }
            if (Mathf.Abs(hInput)>= 0 && Mathf.Abs(steerOutput) <= 1){
                steerOutput += hInput * steerSpeed * Time.deltaTime;
            }
            if(Mathf.Abs(hInput) < steerOutput && hInput == 0){
                steerOutput -= Mathf.Sign(steerOutput) * Time.deltaTime * steerSpeed; 
            }
        }
        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        realSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        forwardSpeed = driveRpm*Mathf.PI*driveDia/60;
        
        //Speedometer
        speedText.text = Mathf.RoundToInt(forwardSpeed*2.23694f).ToString();
        // Calculate how close the car is to top speed
        // as a number from zero to one
        speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        currentMotorTorque = Mathf.Lerp(engineTorque, 0,speedFactor);

        // Check whether the car is going
        isAccelerating = 0 < vInput;
        //Calculates & displays fake engine rpm
        if (gear == 0){
            if(gs.isGamepad){
                 if (vInput > 0 && engineRpm <= maxRpm){
                    engineRpm = vInput*10000;
                }else if(engineRpm >= idleRpm){
                    engineRpm -= engineRpm/30;
                }
            }else{
                if (vInput > 0 && engineRpm <= maxRpm){
                    engineRpm += vInput*300;
                }else if(engineRpm >= idleRpm){
                    engineRpm -= engineRpm/30;
                }
            }
            engineRpm = Mathf.RoundToInt(engineRpm);
        }else{
            engineRpm = Mathf.RoundToInt((Mathf.Lerp(0, maxRpm/10000,speedFactor)*10000)+ idleRpm);
        }
        
        rpmText.text = engineRpm + " rpm";
        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, Mathf.Abs(realSpeed)/30);
        
        if (steeringHelp)
        {
            foreach (var wheel in wheels)
             {
                if (wheel.steerable)
                {
                    wheel.wheelCollider.steerAngle = steerOutput * currentSteerRange;
                }
            }
        }
        else
        {
              foreach (var wheel in wheels)
            {
                 if (wheel.steerable)
                {
                    wheel.wheelCollider.steerAngle = steerOutput * 30;
                }
            }
        }
        foreach (var wheel in wheels)
        {   
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (isAccelerating)
            { 
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.wheelCollider.motorTorque = vInput * currentMotorTorque;
                }
            }
            else
            {
                if (wheel.motorized){
                    Debug.Log(wheel.wheelCollider.brakeTorque);
                    wheel.wheelCollider.motorTorque = 0;
                }
                
            }
            // If the user is trying to go in the opposite direction
            // apply brakes to all wheels
            
        
        }
       
    }
}