using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Verticle = "Vertical";

    private float horizontalInput;
    private float verticalInput;

    public bool isBreaking;

    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    float currentBreakForce;
    [SerializeField] float maxSteeringAngle;
    float currentSteeringAngle;

    private PlayerActionController inputActions;

    private void Awake()
    {
        inputActions = new PlayerActionController();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        inputActions.Land.Break.performed += ctx => breakPressed(ctx);
        inputActions.Land.Break.canceled += ctx => breakPressed(ctx);

    }

    // Update is called once per frame
    void Update()
    {

        GetInput();
        
    }
    private void FixedUpdate()
    {

        HandleMotor();
        HandleSteering();
        UpdateWheelRot();
       // transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }

    
    private void GetInput()
    {
        horizontalInput = inputActions.Land.Horizontal.ReadValue<float>();
        verticalInput = inputActions.Land.Vertical.ReadValue<float>();
        
        //isBreaking =
       // if(inputActions.Land.Break.ReadValue<float>())
       // Debug.Log( inputActions.Land.Break.ReadValue<float>());
    }

    private void breakPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) {
            isBreaking = true;
        }
        if (ctx.canceled) {
            isBreaking = false;
        }
    }

    private void HandleMotor()
    {
        rearLeftWheel.motorTorque = verticalInput * motorForce ;
       rearRightWheel.motorTorque = verticalInput * motorForce ;
       frontLeftWheel.motorTorque = verticalInput * motorForce;
        frontRightWheel.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
       // if (isBreaking)
        {
            ApplyBreak();
        }
    }
   
    private void ApplyBreak()
    {
        Debug.Log("applied");
        frontLeftWheel.brakeTorque = currentBreakForce;
        frontRightWheel.brakeTorque = currentBreakForce;
        rearLeftWheel.brakeTorque = currentBreakForce*0.5f;
        rearRightWheel.brakeTorque = currentBreakForce*0.5f;
    }
    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;//Mathf.Lerp(1,2,GetComponent<Rigidbody>().velocity.magnitude*0.01f) ;
        frontLeftWheel.steerAngle = currentSteeringAngle;
        frontRightWheel.steerAngle = currentSteeringAngle;
       // GetComponent<Rigidbody>().MoveRotation(new Quaternion(transform.rotation.x,transform.rotation.y+(currentSteeringAngle*(GetComponent<Rigidbody>().velocity.magnitude/1000)), transform.rotation.z,transform.rotation.z));
    }
    private void UpdateWheelRot()
    {
        UpdateSingleWheelRot(frontRightWheel,frontRightWheelTransform);
        UpdateSingleWheelRot(frontLeftWheel,frontLeftWheelTransform);
        UpdateSingleWheelRot(rearRightWheel,rearRightWheelTransform);
        UpdateSingleWheelRot(rearLeftWheel,rearLeftWheelTransform);
    }

    private void UpdateSingleWheelRot(WheelCollider Wheel, Transform WheelTransform)
    {
        Vector3 pos;
        Quaternion qout;
        Wheel.GetWorldPose(out pos,out qout);
        WheelTransform.position = pos;
        WheelTransform.rotation = qout;
    }
}
