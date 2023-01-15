using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlyingCarController))]
public class ParticleAndSoundManager : MonoBehaviour
{
    FlyingCarController controller;

  //  [SerializeField] AudioClip MotorAudio;
   // [SerializeField] AudioClip FansAudio;
    //[SerializeField] AudioClip MotorWithNitroAudio;
    //[SerializeField] AudioClip FanAudioSideWays;

    [SerializeField] ParticleSystem Thruster;
    [SerializeField] ParticleSystem ThrusterWithNitro;

    [SerializeField] AudioSource MotorAudioS;
    [SerializeField] AudioSource FansAudioS;
    [SerializeField] AudioSource MotorWithNitroAudioS;
    [SerializeField] AudioSource FanAudioSideWaysS;
    // Start is called before the first frame update
    void Start()
    {
      /*  MotorAudioS = gameObject.AddComponent<AudioSource>();
        MotorAudioS.clip = MotorAudio;
        FansAudioS = gameObject.AddComponent<AudioSource>();
        FansAudioS.clip = FansAudio;
        MotorWithNitroAudioS = gameObject.AddComponent<AudioSource>();
        MotorWithNitroAudioS.clip = MotorWithNitroAudio;
        FanAudioSideWaysS = gameObject.AddComponent<AudioSource>();
        FanAudioSideWaysS.clip = FanAudioSideWays;*/
        controller = GetComponent<FlyingCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.horizontalInput == 0)
        {
            if (!FansAudioS.isPlaying)
                FansAudioS.Play();
            if (FanAudioSideWaysS.isPlaying)
                FanAudioSideWaysS.Stop();
        }
        else {
            if (FansAudioS.isPlaying)
                FansAudioS.Stop();
            if (!FanAudioSideWaysS.isPlaying)
                FanAudioSideWaysS.Play();
        }

        if (controller.verticalInput != 0)
        {
            if(!Thruster.isPlaying && controller.verticalInput >= 0)
            Thruster.Play();
            if (controller.nitro) {
                if (!MotorWithNitroAudioS.isPlaying && controller.verticalInput >= 0) { MotorWithNitroAudioS.Play(); }
                if (MotorAudioS.isPlaying) { MotorAudioS.Stop(); }
                if (!ThrusterWithNitro.isPlaying && controller.verticalInput >= 0) { ThrusterWithNitro.Play(); }
            }
            else {
                if (MotorWithNitroAudioS.isPlaying) { MotorWithNitroAudioS.Stop(); }
                if (!MotorAudioS.isPlaying) { MotorAudioS.Play(); }
                if (ThrusterWithNitro.isPlaying) { ThrusterWithNitro.Stop(); }
            }

        }
        else {
            if(MotorAudioS.isPlaying) MotorAudioS.Stop();
            if (MotorWithNitroAudioS.isPlaying) MotorWithNitroAudioS.Stop();
            if (Thruster.isPlaying) Thruster.Stop();
            if (ThrusterWithNitro.isPlaying) ThrusterWithNitro.Stop();
        }
    }
}
