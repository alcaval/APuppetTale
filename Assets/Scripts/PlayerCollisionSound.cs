using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionSound : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private List<AudioClip> _audiosCollision;
    [SerializeField] private AudioSource _source;
    private int lastPlayedSound = -1;

    private void OnTriggerEnter(Collider other) 
    {
        if(_rb.velocity.y > 0.001f || _source.isPlaying || (other.tag != "Floor" && other.tag != "Prop")) return;

        int index;
        do
        {
            index = Random.Range(0, _audiosCollision.Count);
        }while(index == lastPlayedSound);
        
        lastPlayedSound = index;
        _source.clip = _audiosCollision[index];      
        _source.Play();  
    }
}
