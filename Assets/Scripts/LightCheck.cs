using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheck : MonoBehaviour
{
    private bool _enabled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < 0.1){
            GetComponent<Light>().enabled = true;
        }
        else if(transform.position.z < -7.5){
            GetComponent<Light>().enabled = true;
        }
    }
}
