using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FlyingCarController))]
public class CameraShake : MonoBehaviour
{
    FlyingCarController FlyingCarController;
   [SerializeField] CinemachineVirtualCamera VirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        FlyingCarController = GetComponent<FlyingCarController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FlyingCarController.nitro && FlyingCarController.verticalInput!=0)
        {
            
            {
                VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 10;
            }
        }
        else {

            VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        }
    }
}
