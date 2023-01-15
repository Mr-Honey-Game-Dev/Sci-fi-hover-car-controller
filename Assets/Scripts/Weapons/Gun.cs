using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponMode mode = WeaponMode.missile;
    Coroutine shootingCR;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed=2;
    [SerializeField] Transform spawningPoint;
    [SerializeField] float RecoilForce = 10;
    [SerializeField] AudioSource ShootAuido;
    [SerializeField] ParticleSystem MuzzleFlash;
    bool fire;
    bool recoilStart;
    // Start is called before the first frame update
    void Start()
    {
        bullet.SetActive(false);
    }

    // Update is called once per frame
   public void GunUpdate(bool firing,float timeGap)
    {
        fire = firing;
        Recoil();
        if (firing && shootingCR == null)
        {
            shootingCR = StartCoroutine(shooting(timeGap));
        }
        else
        {
            if (!firing && shootingCR != null)
            {
                StopCoroutine(shootingCR);
                shootingCR = null;
            }
        }

    }
    IEnumerator shooting(float gap) {
        //Debug.Log(gap);
        yield return new WaitForSeconds(0);
        while (fire) {
            Shoot();
            StartCoroutine(RecoilCR(gap));
            yield return new WaitForSeconds(gap);
        }
    
    }

    private void Shoot()
    {
        Recoil();
        if (MuzzleFlash) { 
            if(!MuzzleFlash.isPlaying)
            MuzzleFlash.Play(); }
        GameObject NewBullet = Instantiate(bullet);
        NewBullet.SetActive(true);
        NewBullet.transform.position = spawningPoint.position;
        ShootAuido.Play();
        NewBullet.GetComponent<Bullet>().startTranform=gameObject.transform;
        NewBullet.GetComponent<Bullet>().speed = bulletSpeed;
    
       // NewBullet.AddComponent<Reference>().Ref = GetComponent<Reference>().Ref;
        Destroy(NewBullet, 3);
    }




    [SerializeField] Transform InitialPos;
    [SerializeField] Transform EndPosition;
    [SerializeField] Transform recoilObject;
    private void Recoil()
    {
        if (recoilStart == true)
        {
            recoilObject.position = Vector3.MoveTowards(recoilObject.position, EndPosition.position, Time.deltaTime * 6);
        }
        else {
            recoilObject.position = Vector3.MoveTowards(recoilObject.position, InitialPos.position, Time.deltaTime * 6);

        }

    }
    IEnumerator RecoilCR(float timeGap) {
        recoilStart = true;
        GetComponent<Reference>().Ref.GetComponent<FlyingCarController>().WeaponRecoil = -transform.forward * RecoilForce/10;
        yield return new WaitForSeconds(timeGap/2);
        GetComponent<Reference>().Ref.GetComponent<FlyingCarController>().WeaponRecoil = Vector3.zero;

        recoilStart = false;
    }
}
