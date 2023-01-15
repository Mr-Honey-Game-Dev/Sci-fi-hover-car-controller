using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChecker : MonoBehaviour
{

    public bool inAir;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground") {
            inAir = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            inAir = true;
        }
    }
}
