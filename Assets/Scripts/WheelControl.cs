using Unity.VisualScripting;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public Transform wheelModel;

    [HideInInspector] public WheelCollider wheelCollider;
    public TrailRenderer skid;
    public ParticleSystem dust;
    // Create properties for the CarControl script
    // (You should enable/disable these via the 
    // Editor Inspector window)
    public bool steerable;
    public bool motorized;
    public bool isLeftDrive;
    public bool isRightDrive;
    bool isPunctured = false;
    public Rigidbody carBody;
    float slipLongThreshold, slipLatThreshold;
    Vector3 position;
    Quaternion rotation;
    float slipLat, slipLong;
    bool groundHit = false;
    float relativeVelocity;
    float wheelDamage;
    // Start is called before the first frame update
    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        slipLatThreshold = .4f;
        slipLongThreshold = .55f;
    }

    void Puncture(){
        if (isPunctured == true){
            WheelFrictionCurve fFriction = wheelCollider.forwardFriction;
            WheelFrictionCurve sFriction = wheelCollider.sidewaysFriction;
            fFriction.stiffness = .2f;
            sFriction.stiffness = .2f;
            wheelModel.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        Debug.Log(wheelDamage);
        if(groundHit == false){
            if (wheelCollider.GetGroundHit(out WheelHit ishit) == true){
                relativeVelocity = Mathf.Abs(carBody.linearVelocity.y);
            }else{
                relativeVelocity = 0;
            }
            if (relativeVelocity > 3){
                wheelDamage += relativeVelocity/100;
            }
            if (wheelDamage >= 20){
                isPunctured = true;
            }
            
            //Puncture();
            groundHit = true;
        }
        if (groundHit = true && wheelCollider.GetGroundHit(out WheelHit hit) == false){
            groundHit = false;
        }
        wheelCollider.GetGroundHit(out WheelHit wheelData);
            slipLat = wheelData.sidewaysSlip;
            slipLong = wheelData.forwardSlip;
        // Get the Wheel collider's world pose values and
        // use them to set the wheel model's position and rotation
        wheelCollider.GetWorldPose(out position, out rotation);
            if (wheelModel.gameObject != null){
            wheelModel.transform.position = position;
            wheelModel.transform.rotation = rotation;
            }
        if (skid!=null){
            if (Mathf.Abs(slipLat)>slipLatThreshold || Mathf.Abs(slipLong)>slipLongThreshold){
                dust.Play();
                skid.emitting = true;
            }else{
                dust.Stop();
                skid.emitting = false;
            }
        }

    }
}