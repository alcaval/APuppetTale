using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolinoBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * _speed * Time.deltaTime, Space.Self);
    }
}
