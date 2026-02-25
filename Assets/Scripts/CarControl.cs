using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using Unity.Cinemachine;

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
    float maxRpm = 8000;
    float currentSteerRange;
    float currentMotorTorque;
    float nonDriveTorque;
    public int gear;
    bool isAccelerating;
    bool firstPerson = false;
    bool lightsOn = false;
    public bool steeringHelp;
    InputAction steerActionKeyboard;
    InputAction steerActionGamepad;
    InputAction accelAction;
    InputAction gearUp;
    InputAction changeCamera;
    InputAction gearDown;
    InputAction lightToggle;
    WheelControl[] wheels;
    public GameObject[] lights;
    public CinemachineCamera cinemachineCamera;
    public WheelCollider driveWheelRight;
    public WheelCollider driveWheelLeft;
    public Transform steeringWheel;
    public Transform popups;
    Rigidbody rigidBody;
    public GlobalSettings gs;
    public TMP_Text gearText;
    public TMP_Text speedText;
    public TMP_Text rpmText;
    public Image rpmNeedle;
    public RectMask2D rpmGague;
    public float powerCurve;
    float clutchRPM;
    // Start is called before the first frame update
    void Start()
    {
        
        gearUp = InputSystem.actions.FindAction("GearUp");
        gearDown = InputSystem.actions.FindAction("GearDown");
        accelAction = InputSystem.actions.FindAction("Accel");
        steerActionKeyboard = InputSystem.actions.FindAction("SteerKeyboard");
        steerActionGamepad = InputSystem.actions.FindAction("SteerGamepad");
        changeCamera = InputSystem.actions.FindAction("SwitchCamera");
        lightToggle = InputSystem.actions.FindAction("Lights");
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
        if (gear>=1){
            gearText.text = gear.ToString();
        }else if(gear==-1){
            gearText.text = "R";
        }
        else if (gear == 0)
        {
            gearText.text= "N";
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
        if(lightToggle.WasPressedThisFrame() && lightsOn)
        {
            lights[0].SetActive(false);
            
            popups.localRotation = Quaternion.Euler(-86.427f, 216.72f, -219.097f);
            lightsOn = false;
            
        }else if(lightToggle.WasPressedThisFrame())
        {
            lights[0].SetActive(true);
            popups.localRotation = Quaternion.Euler(-51.098f,446.105f,-448.608f);
            lightsOn = true;
        }
        if (changeCamera.WasPressedThisFrame() && !firstPerson)
        {
            cinemachineCamera.gameObject.SetActive(false);
            firstPerson = true;
        } else if(changeCamera.WasPressedThisFrame() && firstPerson)
        {
            cinemachineCamera.gameObject.SetActive(true);
            firstPerson = false;
        }
        
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(60,100, realSpeed/90);
        if (gear != 0)
        {
            powerCurve = Mathf.Abs(Mathf.Clamp((Mathf.Sqrt(2f*(((Mathf.Abs(engineRpm+clutchRPM-1000))/10000)))+0.1f-Mathf.Pow(((Mathf.Abs(engineRpm+clutchRPM-1000))/10000), 2f)), .01f, 10));
            engineTorque = motorTorqueGear*powerCurve;
        }
        else
        {
            engineTorque = motorTorqueGear;
        }
        
        driveRpm = (driveWheelLeft.rpm+driveWheelRight.rpm)/2;
        steeringWheel.localRotation = Quaternion.Euler(steerOutput*250, steeringWheel.rotation.y, steeringWheel.rotation.z);

        //rpmNeedle.transform.rotation = Quaternion.Euler(rpmNeedle.transform.rotation.x, rpmNeedle.transform.rotation.y, (-engineRpm/100)+90);
        rpmGague.padding = new Vector4(0,0,Mathf.Lerp(520,30,engineRpm/8000),0);
        Debug.Log(engineRpm);
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
        speedText.text = Mathf.RoundToInt(Mathf.Abs(forwardSpeed*2.23694f)).ToString();
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
                    engineRpm = (vInput*1000000)*Time.deltaTime;
                }else if(engineRpm >= idleRpm){
                    engineRpm -= (engineRpm/.3f)*Time.deltaTime;
                }
            }else{
                if (vInput > 0 && engineRpm <= maxRpm){
                    engineRpm += (vInput*30000)*Time.deltaTime;
                }else if(engineRpm >= idleRpm){
                    engineRpm -= (engineRpm/.3f)*Time.deltaTime;
                }
            }
            engineRpm = Mathf.RoundToInt(engineRpm);
            clutchRPM = engineRpm; 
        }else{
            if (clutchRPM > 300)
            {
                clutchRPM -= engineRpm*100*Time.deltaTime;
            }
            else
            {
                clutchRPM = 1;
            }

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