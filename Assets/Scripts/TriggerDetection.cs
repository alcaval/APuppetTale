using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public bool dead = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Dead");
            dead = true;
        }
    }

    // private void OnTriggerExit(Collider other) {
    //     if(other.tag == "Prop"){
    //         //print("PropFound");
    //         Destroy(other.gameObject);
    //     }
    // }
}
