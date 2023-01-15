using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponMode {laser,missile,grapplingGun }
public class Weapons : MonoBehaviour
{
    PlayerActionController InputActions;
    float mouseHorizontal;
    float mousevertical;
    bool fire;
    int WeaponIndex=0;

    [HideInInspector] public WeaponMode mode;
    [Range(1, 6)]
    public float RotSensitivity=3; 
    [Range(3,8)]
    public int TranformationSpeed=3;

    [SerializeField] GameObject laserObj;
    [SerializeField] GameObject missileObj;
    [SerializeField] GameObject[] weapons;
    [Range(1, 15)] [SerializeField] int BulletsinOneSec=2;
    [SerializeField] AudioSource ChangeSound;
    [SerializeField] ParticleSystem WeaponChangeEffect;

    private Vector3[] weaponsScale;
    private Animator[] weaponsAnimator;
    private Gun gun;
    private Laser laser;
  //  private Animator laserAnimator;
   // private Animator missileAnimator;

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

    // Start is called before the first frame update
    void Start()
    {
        mode = WeaponMode.laser;
        InputActions.Land.Fire.performed += ctx => FireAction(ctx);
        InputActions.Land.Fire.canceled += ctx => FireAction(ctx);
        InputActions.Land.ChangeWeapon.performed += ctx => ChangeWeapon(ctx);
        InputActions.Land.ChangeWeapon.canceled += ctx => ChangeWeapon(ctx);
        InitWeapons();
    }
    void InitWeapons()
    {
        weaponsScale = new Vector3[weapons.Length];
        weaponsAnimator = new Animator[weapons.Length];

        for (int i = 0; i < weapons.Length; i++) {
            weaponsScale[i] = weapons[i].transform.localScale;
            weapons[i].transform.localScale = Vector3.zero;

            weaponsAnimator[i] = weapons[i].GetComponent<Animator>();
        }
        gun = missileObj.GetComponent<Gun>();
        laser = laserObj.GetComponent<Laser>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        HandleWeaponRotation();
        HandleMissile();
        HandleScale(TranformationSpeed);
    }
    private void Update()
    {
        mode = weapons[WeaponIndex].GetComponent<Reference>().mode;
        HandleLaser();
    }

    private void HandleScale(int speed)
    {

        for (int i = 0; i < weapons.Length; i++) {
            if (i == WeaponIndex)
            {
                weapons[i].transform.localScale = Vector3.MoveTowards(weapons[i].transform.localScale, weaponsScale[i], Time.deltaTime * speed);
            }
            else { 
                weapons[i].transform.localScale = Vector3.MoveTowards(weapons[i].transform.localScale, Vector3.zero, Time.deltaTime * speed);
            }
        
        }
     
    }

    private void HandleWeaponRotation()
    {
        RototeObject(weaponsAnimator[WeaponIndex], InputActions.Land.MouseLook.ReadValue<Vector2>(), 0.0033f * RotSensitivity);
    }

    void RototeObject(Animator a,Vector2 coordinates,float Multiplier) {
        if (a == null) { return; }

        if (a.GetFloat("Y") <= 1 && a.GetFloat("Y") >= -1)
            a.SetFloat("Y", a.GetFloat("Y") + (coordinates.y * Multiplier));
       else
            a.SetFloat("Y", (1 - a.GetFloat("Y") < 0) ? 1 : -1);

       
        if (a.GetFloat("X") <= 1 && a.GetFloat("X") >= -1)
            a.SetFloat("X", a.GetFloat("X") + (coordinates.x * Multiplier));
        else
            a.SetFloat("X", (1 - a.GetFloat("X") < 0) ? 1 : -1);       
    }
 
    private void HandleMissile()
    {
        if (mode == WeaponMode.missile)
        {

            gun.GunUpdate(fire, 1.0f / (float)BulletsinOneSec);
        }
        else {
            gun.GunUpdate(false, 1.0f / (float)BulletsinOneSec);

        }
    }

    private void HandleLaser()
    {
        if (mode == WeaponMode.laser)
        {
            laser.LaserUpdate(fire);
        }
        else 
        { 
            laser.LaserUpdate(false);
        }
    }
   

    private void ChangeWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled) {
            WeaponIndex = (WeaponIndex + 1) % (weapons.Length);
            ChangeSound.Play();
            if(WeaponChangeEffect)
            WeaponChangeEffect.Play();

        }
    }
    IEnumerator ChangeToMissile() {
        yield return new WaitForSeconds(0f);
        //while (Vector3.Distance(missile.transform.localScale,missileScale) > 0.1f || laser.transform.localScale== Vector3.zero)
        //{
        //    Debug.Log(Vector3.Distance(missile.transform.localScale, missileScale)+ " "+Vector3.Distance(laser.transform.localScale, Vector3.zero));
        //    if (Vector3.Distance(missile.transform.localScale, missileScale) > 0.1f)
        //    {
        //        missile.transform.localScale += Vector3.Lerp(missile.transform.localScale, missileScale, Time.deltaTime);
        //    }
        //    if (Vector3.Distance(laser.transform.localScale, Vector3.zero) > 0.1f)
        //    {

        //        laser.transform.localScale -= Vector3.Lerp(laser.transform.localScale, Vector3.zero, Time.deltaTime);
        //    }
        //    yield return new WaitForSeconds(0.2f);
        //};
    }
    IEnumerator ChangeToLaser()
    {
        yield return new WaitForSeconds(0f);
        //while (Vector3.Distance(laser.transform.localScale,laserScale) != 0 || Vector3.Distance(missile.transform.localScale, Vector3.zero) != 0)
        //{
        //    if (Vector3.Distance(laser.transform.localScale,laserScale) != 0)
        //    {
        //        laser.transform.localScale += Vector3.Lerp(laser.transform.localScale, laserScale, Time.deltaTime);
        //    }
        //    if (Vector3.Distance(missile.transform.localScale, Vector3.zero) != 0)
        //    {

        //        missile.transform.localScale -= Vector3.Lerp(missile.transform.localScale, Vector3.zero, Time.deltaTime);
        //    }
        //    yield return new WaitForSeconds(0.2f);
        //};
    }

    private void FireAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            fire = true;
        }
        else if (ctx.canceled)
        {
            fire = false;
        }
    }

}
