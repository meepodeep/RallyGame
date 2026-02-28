using UnityEngine;
using UnityEngine.InputSystem;
public class cameraControl : MonoBehaviour
{
    InputAction lookAction;
    Vector2 look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        look = lookAction.ReadValue<Vector2>();
        Debug.Log(look);
        if (look.x == 1)
        {
            transform.localRotation = Quaternion.Euler(-20,90,0);
        }
        if (look.x == -1f)
        {
            transform.localRotation = Quaternion.Euler(-20,-90,0);
        }
        if (look.y == -1f)
        {
            transform.localRotation = Quaternion.Euler(-10,180,0);
        }
        if (look.y == 0 && look.x == 0)
        {
            transform.localRotation = Quaternion.Euler(0,0,0);
        }
    }
}
