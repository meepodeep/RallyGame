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
    float ABS;
    float absUnlocker = -3000;
    float brakeTorque = 3500;
    float handbrakeTorque = 3000;
    public bool absEnable = true;
    public float brakeBalance = .8f;
    float brakeBalanceRear; 
    InputAction brakes;
    InputAction handbrake;
    public TMP_Text rpmLeft;
    public TMP_Text rpmRight;
    float vInput;
    InputAction accelAction;
    bool isAccelerating;
    public CarControl cc;
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
            //ABS
            if (absEnable &&Mathf.Abs(wheel.slipLong) > .4f)
            {
                ABS = (1-(brakesOutput/1.2f));
            }
            else
            {
                ABS = 1;

            }
            //DIFF
            if (wheel.isLeftDrive){
                wheel.wheelCollider.brakeTorque = Mathf.Clamp(((brakesOutput*brakeTorque*brakeBalance) + leftLock)+(Mathf.Abs(vInput-1)*(400*(1f/Mathf.Sqrt(cc.gear+2)))), 0, 7000)*ABS;
                rpmLeft.text = wheel.wheelCollider.brakeTorque.ToString(); 
            }
            if (wheel.isRightDrive){
                wheel.wheelCollider.brakeTorque = Mathf.Clamp(((brakesOutput*brakeTorque*brakeBalance) + rightLock)+(Mathf.Abs(vInput-1)*(400*(1f/Mathf.Sqrt(cc.gear+2)))), 0, 7000)*ABS; 
                rpmRight.text = wheel.wheelCollider.brakeTorque.ToString();
            }
            //HANDBRAKE AND UNLOCKING WHEELS BC UNITY DUMB
            if (!wheel.motorized){
                if (Mathf.Abs(wheel.wheelCollider.rpm) <= 1 && isAccelerating)
                {
                    wheel.wheelCollider.motorTorque = 100;
                }
                if (handbrakeOutput < 1){
                wheel.wheelCollider.brakeTorque = brakesOutput* brakeBalanceRear * brakeTorque*ABS;
                }
            }else{
                //HANDBRAKE 2
                if (handbrakeOutput>0){
                    wheel.wheelCollider.brakeTorque = handbrakeOutput * handbrakeTorque;
                }
            }
        }
    }
}
