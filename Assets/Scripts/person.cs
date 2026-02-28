
using UnityEngine;


public class person : MonoBehaviour
{
    public Transform Player;
    public Transform guyTransform;
    public Rigidbody rb;
    Vector3 m_EulerAngleVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    void FixedUpdate()
    {
        guyTransform.LookAt(Player);
    }
}
