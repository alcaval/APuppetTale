using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private float _speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime, Space.World);
    }

    public void resetLevel(){
        transform.position = Vector3.zero;
    }

    public void SetSpeed(float newSpeed) => _speed = newSpeed;
    public float GetSpeed() => _speed;
}
