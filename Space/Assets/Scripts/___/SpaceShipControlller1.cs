using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour
{
    public ConfigurableJoint leftEngineJoint;
    public ConfigurableJoint rightEngineJoint;
    public Rigidbody shipRigidbody;
    public Transform leftEngineTransform;
    public Transform rightEngineTransform;
    public float rotationSpeed = 50f;
    public float maxThrustForce = 1000f;
    public float thrustAcceleration = 500f;
    public float thrustDeceleration = 300f;
    private float currentThrust = 0f;
    private float leftRotationInput = 0f;
    private float rightRotationInput = 0f;
    private bool applyThrust = false;
    private InputActionMap actionMap;

    private void OnEnable()
    {
        actionMap = new InputActionMap("SpaceshipControls");

        var leftRotate = actionMap.AddAction("RotateLeftEngine");
        leftRotate.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/q");
        leftRotate.performed += ctx => leftRotationInput = ctx.ReadValue<float>();
        leftRotate.canceled += ctx => leftRotationInput = 0f;

        var rightRotate = actionMap.AddAction("RotateRightEngine");
        rightRotate.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/d")
            .With("Positive", "<Keyboard>/e");
        rightRotate.performed += ctx => rightRotationInput = ctx.ReadValue<float>();
        rightRotate.canceled += ctx => rightRotationInput = 0f;

        var thrustAction = actionMap.AddAction("Thrust", binding: "<Keyboard>/space");
        thrustAction.performed += ctx => applyThrust = true;
        thrustAction.canceled += ctx => applyThrust = false;

        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }

    private void FixedUpdate()
    {
        // Rotation
        if (leftRotationInput != 0f)
        {
            Quaternion leftTarget = leftEngineJoint.targetRotation * Quaternion.Euler(leftRotationInput * rotationSpeed * Time.fixedDeltaTime, 0f, 0f);
            leftEngineJoint.targetRotation = leftTarget;
        }

        if (rightRotationInput != 0f)
        {
            Quaternion rightTarget = rightEngineJoint.targetRotation * Quaternion.Euler(rightRotationInput * rotationSpeed * Time.fixedDeltaTime, 0f, 0f);
            rightEngineJoint.targetRotation = rightTarget;
        }

        leftEngineJoint.angularXDrive = new JointDrive { positionSpring = 2000f, positionDamper = 1000f, maximumForce = 5000 };
        rightEngineJoint.angularXDrive = new JointDrive { positionSpring = 2000f, positionDamper = 1000f, maximumForce = 5000 };

        // Thrust ramp
        if (applyThrust) currentThrust = Mathf.Min(currentThrust + thrustAcceleration * Time.fixedDeltaTime, maxThrustForce);
        else currentThrust = Mathf.Max(currentThrust - thrustDeceleration * Time.fixedDeltaTime, 0f);

        // Apply thrust to both
        if (currentThrust > 0f)
        {
            Vector3 leftForce = leftEngineTransform.up * currentThrust;
            shipRigidbody.AddForceAtPosition(leftForce, leftEngineTransform.position);

            Vector3 rightForce = rightEngineTransform.up * currentThrust;
            shipRigidbody.AddForceAtPosition(rightForce, rightEngineTransform.position);
        }
    }
}