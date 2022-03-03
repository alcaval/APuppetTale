using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ragdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 oldMouse;
    private Vector3 mouseSpeed;
    public int jumpCount;

    [Header("Fuerza de impulso")]
    [SerializeField] private float longo = 1f;
    [SerializeField] private float normal = 0.8f;
    [SerializeField] private float corto = 0.5f;

    [Header("Thresholds de impulso")]
    [SerializeField] private float _threshLongo = 0.7f;
    [SerializeField] private float _threshNormal = 0.4f;
    [SerializeField] private float _threshCorto = 0.2f;
    // [SerializeField] private float _jumpForce;

    private void OnMouseDown() {
        oldMouse = Input.mousePosition;
        screenPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        offset = Camera.main.WorldToViewportPoint(gameObject.transform.position) - Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    }

    private void OnMouseDrag() {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 curPosition = Camera.main.ScreenToViewportPoint(curScreenPoint) - offset;
    }

    private void OnMouseUp() {
        mouseSpeed = Camera.main.ScreenToViewportPoint(oldMouse - Input.mousePosition);
        Vector3 potensia = mouseSpeed;

        if(Mathf.Abs(mouseSpeed.x) > 0.2 || Mathf.Abs(mouseSpeed.y) > 0.2){
            jumpCount++;
        }

        rb.velocity = -manageJump(mouseSpeed) * 100;
    }

    private Vector3 manageJump(Vector3 potencia){
        float y = 0f;
        float x = 0f;
        
        if(potencia.y < -_threshLongo){
            y = -longo;
            print("longo");
        }else if(potencia.y < -_threshNormal){
            y = -normal;
            print("normal");
        }else if(potencia.y < -_threshCorto){
            y = -corto;
            print("corto");
        }else{
            print("maaals");
        }

        if(potencia.x < -_threshLongo){
            x = -longo;
        }else if(potencia.x < -_threshNormal){
            x = -normal;
        }else if(potencia.x < -_threshCorto){
            x = -corto;
        }

        if(potencia.x > _threshLongo){
            x = longo;
        }else if(potencia.x > _threshNormal){
            x = normal;
        }else if(potencia.x > _threshCorto){
            x = corto;
        }

        return new Vector3(x, y, 0f);
    }
}
