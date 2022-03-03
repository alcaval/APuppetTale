using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treadmillBehaviour : MonoBehaviour
{
    private bool roll = false;
    private Collider player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roll){
            player.GetComponent<Rigidbody>().velocity = Vector3.left * 50f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        roll = true;
        player = other;  
    }

    private void OnTriggerExit(Collider other) {
        roll = false;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void resetTreadmill(){
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}
