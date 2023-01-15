using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FlyingCarController : MonoBehaviour
{
    PlayerActionController InputActions;
    Rigidbody rg;

    [HideInInspector] public float horizontalInput;
    public float verticalInput;

  
    public bool isBreaking;
    [HideInInspector]public bool nitro;

    [SerializeField] float acceleration=2;
    [SerializeField] float rotationSpeed = 2;
    [SerializeField] float maxVelocity;
    [SerializeField] float breakForce;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] int gravityMultiplier=1;
    [SerializeField] float PropellarSpeed=20f;
    [HideInInspector] public Vector3 WeaponRecoil;
    [HideInInspector] public Vector3 LaserRecoil;

    float currentBreakForce;
    float currentSteeringAngle;

    [SerializeField] GameObject rotatingobject;
    [SerializeField] Animator[] animators;
    [SerializeField] Transform[] Propellars;
    [SerializeField] Animator BodyAnimator;
  

    [SerializeField] AirChecker airChecker;
    [SerializeField] GroundChecker groundChecker;





    // Start is called before the first frame update
    private void Awake()
    {
        InputActions = new PlayerActionController();

    }
    private void OnEnable()
    {
        InputActions.Enable();
    }
    private void OnDisable()
    {
        InputActions.Disable();
    }

    void Start()
    {
         rg = GetComponent<Rigidbody>();
        InputActions.Land.Break.performed += ctx => breakPressed(ctx);
        InputActions.Land.Break.canceled += ctx => breakPressed(ctx); 
        InputActions.Land.Nitro.performed += ctx => nitroPressed(ctx);
        InputActions.Land.Nitro.canceled += ctx => nitroPressed(ctx);

    }

  

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

   
    private void FixedUpdate()
    {
        HandleVelocity();
        ApplyBreaks();
        HandleRotations();
    }
    private void GetInput()
    {
        horizontalInput = InputActions.Land.Horizontal.ReadValue<float>();
        verticalInput = InputActions.Land.Vertical.ReadValue<float>();
       
    }
    Vector3 Zvel;//= (Vector3.Lerp(transform.forward,transform.forward* maxVelocity * verticalInput*power, Time.deltaTime * acceleration*power));
    Vector3 Xvel;
    private void HandleVelocity()
    {
        float rightVelDivider= rg.velocity.z > 0 ? 100 : 3000;
        int reverseMult = (verticalInput  >= 0 ) ? 1 : -1;
        float power = nitro && verticalInput >= 0 ? 1.5f : 1;

        Vector3 Yvel= Vector3.zero;
        if (airChecker.inAir)
            Yvel = new Vector3(0, Mathf.Lerp(rg.velocity.y, -gravityMultiplier, Time.deltaTime), 0);          
        else if (!groundChecker.aboveGround)
             Yvel = new Vector3(0, Mathf.Lerp(rg.velocity.y, gravityMultiplier, Time.deltaTime ), 0);

        if (horizontalInput != 0) {
            WeaponRecoil -= new Vector3(0, WeaponRecoil.y, 0);
        }
       //=  ( Vector3.Lerp(transform.right , (transform.right* (maxVelocity * horizontalInput *reverseMult) / rightVelDivider), Time.deltaTime * acceleration));
        Zvel = Vector3.MoveTowards(Zvel, transform.forward * maxVelocity * verticalInput*power , Time.deltaTime * acceleration );
        Xvel = Vector3.MoveTowards(Xvel, (transform.right * (maxVelocity * horizontalInput * reverseMult) / rightVelDivider), Time.deltaTime* acceleration);
        rg.velocity = Zvel+Yvel + Xvel + WeaponRecoil + LaserRecoil;
       
        if (WeaponRecoil.z != 0 || WeaponRecoil.y != 0 || WeaponRecoil.z != 0) {
            WeaponRecoil = Vector3.zero;
        }
        

    }
    private void ApplyBreaks()
    {
       // throw new NotImplementedException();
    }

    private void HandleRotations()
    {
        // transform.rotation = new Quaternion(transform.rotation.x,, transform.rotation.z, transform.rotation.w);
        if (horizontalInput != 0)
        {
            transform.Rotate(transform.up, Mathf.Lerp(transform.rotation.y, transform.rotation.y + maxSteeringAngle * horizontalInput, Time.deltaTime * rotationSpeed));


            
           // transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(30 * horizontalInput, 0, 30 * verticalInput, transform.rotation.w), Time.deltaTime);
        }
      /* rotatingobject.transform.localRotation = new Quaternion(
            Mathf.Lerp(rotatingobject.transform.localRotation.x,30*horizontalInput,Time.deltaTime),
            0,
            Mathf.Lerp(rotatingobject.transform.localRotation.z,30 *verticalInput, Time.deltaTime),
            rotatingobject.transform.localRotation.w
            );*/
        BodyAnimator.SetFloat("HorizontalInput", Mathf.Lerp(BodyAnimator.GetFloat("HorizontalInput"), horizontalInput, Time.deltaTime * 4));
        BodyAnimator.SetFloat("VerticalInput", Mathf.Lerp(BodyAnimator.GetFloat("VerticalInput"), verticalInput/2, Time.deltaTime * 4));

        foreach (Animator a in animators) {
           // a.gameObject.transform.localRotation=new Quaternion(a.gameObject.transform.localRotation.x, a.gameObject.transform.localRotation.y+Time.deltaTime*10, a.gameObject.transform.localRotation.z, a.gameObject.transform.localRotation.w);
            a.SetFloat("HorizontalInput",Mathf.Lerp(  a.GetFloat("HorizontalInput"),horizontalInput,Time.deltaTime*2));

            a.SetFloat("VerticalInput", Mathf.Lerp(a.GetFloat("VerticalInput"),verticalInput, Time.deltaTime*2));
        } 
        foreach (Transform p in Propellars) {
          //  p.localRotation=new Quaternion(p.localRotation.x, p.localRotation.y+Time.deltaTime*10, p.localRotation.z, p.localRotation.w);
            p.Rotate(transform.up,PropellarSpeed*(rg.velocity.sqrMagnitude/maxVelocity)*2 + 10f);
            //p.SetFloat("HorizontalInput",Mathf.Lerp(  a.GetFloat("HorizontalInput"),horizontalInput,Time.deltaTime*2));

            //a.SetFloat("VerticalInput", Mathf.Lerp(a.GetFloat("VerticalInput"),verticalInput, Time.deltaTime*2));
        }

    }

   
    private void breakPressed(InputAction.CallbackContext ctx)
    {

       
    }
    private void nitroPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            nitro = true;
        }
        if (ctx.canceled)
        {
            nitro = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground") {
            
            transform.position += new Vector3(0, Time.deltaTime * 5, 0);
        }
    }
   
}
