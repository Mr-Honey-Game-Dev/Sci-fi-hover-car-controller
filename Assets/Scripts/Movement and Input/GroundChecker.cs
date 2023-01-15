using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool aboveGround;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
           aboveGround = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            aboveGround = true;
        }
    }
}
