using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public WeaponMode mode = WeaponMode.laser;
    [SerializeField] Transform StartTransform;
    LineRenderer lr;
    [SerializeField] Transform RotatingObject;
    bool fire;
    [SerializeField] float RecoilForce = 100;
    [SerializeField] AudioSource LaserAudio;
    [SerializeField] ParticleSystem LaserStartParticle;
    [SerializeField] ParticleSystem LaserEndParticle;

    [SerializeField] GameObject shooter;
    [SerializeField] GameObject shooterWithLights;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shooterWithLights.SetActive(fire);
        shooter.SetActive(!fire);
    }
    public void LaserUpdate(bool Fired)
    {
        if (Fired)
        {
            if (fire == false)
            {
                StartCoroutine(RecoilCR());
                fire = true;
            }
          
            RaycastHit hit;
            if (Physics.Raycast(StartTransform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {


                    if (hit.collider.tag == "Player" && hit.collider.gameObject.GetComponent<Reference>().Ref && hit.collider.gameObject.GetComponent<Reference>().Ref == GetComponent<Reference>().Ref)
                    {
                        LaserOnOff(fire, transform.forward * 5000, false);
                    }

                    else
                    {

                        LaserOnOff(fire, hit.point, true);
                    }

                }

            }
            else {
                LaserOnOff(fire, transform.forward * 5000, false);

            }


        }
        else
        {
            fire = false;
            LaserOnOff(false, Vector3.zero, false);
        }
    }
    void LaserOnOff(bool on, Vector3 endPoint, bool Particles) {
        if (on)
        {
            RotatingObject.transform.Rotate(RotatingObject.transform.up, Time.deltaTime * 2500);
            lr.SetPosition(0, StartTransform.position);

            if (!LaserAudio.isPlaying) { LaserAudio.Play(); }
            if (LaserStartParticle)
            {
                LaserStartParticle.Play();
            }
            lr.SetPosition(1, endPoint);
            if (Particles)
            {
                if (LaserEndParticle)
                {

                    LaserEndParticle.gameObject.SetActive(true);
                    LaserEndParticle.Play();
                    LaserEndParticle.transform.position = endPoint;

                }
            }
            else
            {
                if (LaserEndParticle)
                {
                    LaserEndParticle.Stop();
                    LaserEndParticle.gameObject.SetActive(false);
                }

            }
        }
        else {
            RotatingObject.transform.Rotate(RotatingObject.transform.up, Time.deltaTime * 500);

            if (LaserAudio.isPlaying) { LaserAudio.Stop(); }
            if (LaserStartParticle)
            {
                LaserStartParticle.Stop();
            }
            if (LaserEndParticle) {
                LaserEndParticle.Stop();
                LaserEndParticle.gameObject.SetActive(false);
            }
            lr.SetPosition(0, StartTransform.position);
            lr.SetPosition(1, StartTransform.position);
        }

    }
        IEnumerator RecoilCR()
        {
            GetComponent<Reference>().Ref.GetComponent<FlyingCarController>().LaserRecoil = -(transform.forward) * RecoilForce / 10;
            yield return new WaitForSeconds(0.3f);
            GetComponent<Reference>().Ref.GetComponent<FlyingCarController>().LaserRecoil = -(transform.forward) * 0;


        }
    }

