using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopResumeLevel : MonoBehaviour
{
    [Header("True - Resume ; False - Stop")]
    [SerializeField] private bool mode;

    [SerializeField] private LevelProgression _levelProgression;
    [SerializeField] private Slider speed;

    private void OnTriggerEnter(Collider other) {
        if(mode){
            _levelProgression.SetSpeed(speed.value);
            this.gameObject.SetActive(false);
            this.GetComponentInParent<Transform>().gameObject.GetComponentInChildren<treadmillBehaviour>().enabled = false;
        }else{
            StartCoroutine(waitToStop());
        }
    }

    private IEnumerator waitToStop(){
        yield return new WaitUntil(checkPosition);
        _levelProgression.SetSpeed(0f);
        this.gameObject.SetActive(false);
    }

    private bool checkPosition(){
        return transform.position.x < -8f;
    }
}
