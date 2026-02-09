using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BrakesDiff : MonoBehaviour
{
    WheelControl[] wheels;
    public WheelCollider driveWheelRight;
    public WheelCollider driveWheelLeft;
    float rightLock;
    float leftLock;
    float brakesOutput;
    float handbrakeOutput;
    float brakeTorque = 2500;
    float handbrakeTorque = 3000;
    public float brakeBalance = .8f;
    float brakeBalanceRear; 
    InputAction brakes;
    InputAction handbrake;
    public TMP_Text rpmLeft;
    public TMP_Text rpmRight;
    float vInput;
    InputAction accelAction;
    bool isAccelerating;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brakes = InputSystem.actions.FindAction("Brakes");
        handbrake = InputSystem.actions.FindAction("Handbrake");
        accelAction = InputSystem.actions.FindAction("Accel");
        wheels = GetComponentsInChildren<WheelControl>();
        brakeBalanceRear = 1-brakeBalance;
    }

    // Update is called once per frame
    void Update()
    {
        brakesOutput = brakes.ReadValue<float>();
        handbrakeOutput = handbrake.ReadValue<float>();
        vInput = accelAction.ReadValue<float>();
        isAccelerating = 0 < vInput;
        
        
        if(Mathf.Abs(driveWheelLeft.rpm)-50>Mathf.Abs(driveWheelRight.rpm)){ 
            leftLock = (Mathf.Abs(driveWheelLeft.rpm)-Mathf.Abs(driveWheelRight.rpm))*100;
        }else{
            leftLock = 0;
        }
        if(Mathf.Abs(driveWheelLeft.rpm)+50<Mathf.Abs(driveWheelRight.rpm)){
            rightLock = (Mathf.Abs(driveWheelRight.rpm)-Mathf.Abs(driveWheelLeft.rpm))*100;
        }else{
            rightLock = 0;
        }
        //gets rpm average of drive wheels
        
        foreach (var wheel in wheels)
        {   
            if (wheel.isLeftDrive){
                wheel.wheelCollider.brakeTorque = Mathf.Clamp(((brakesOutput*brakeTorque*brakeBalance) + leftLock)+(Mathf.Abs(vInput-1)*400), 0, 7000);
                rpmLeft.text = wheel.wheelCollider.brakeTorque.ToString(); 
            }
            if (wheel.isRightDrive){
                wheel.wheelCollider.brakeTorque = Mathf.Clamp(((brakesOutput*brakeTorque*brakeBalance) + rightLock)+(Mathf.Abs(vInput-1)*400), 0, 7000); 
                rpmRight.text = wheel.wheelCollider.brakeTorque.ToString();
            }
            if (!wheel.motorized){
                if (Mathf.Abs(wheel.wheelCollider.rpm) <= 1 && isAccelerating)
                {
                    wheel.wheelCollider.motorTorque = 100;
                }
                if (handbrakeOutput < 1){
                wheel.wheelCollider.brakeTorque = brakesOutput* brakeBalanceRear * brakeTorque;
                }else{
                    wheel.wheelCollider.brakeTorque = handbrakeOutput * handbrakeTorque;
                }
            }
        }
    }
}
