using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStartBehaviour : MonoBehaviour
{

    [SerializeField] private Light _luzBrillanteCortinas;
    [SerializeField] private Light _focoJugador;
    [SerializeField] private Light _luzTenueCortinas;


    [SerializeField] private CurtainOpenTest _cortinaDerecha;
    [SerializeField] private CurtainOpenTest _cortinaIzquierda;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerPos;
    private Vector3 _playerStartPosition;
    [SerializeField] private GameObject _nivel;


    private void Start() 
    {
        _playerStartPosition = _player.transform.position;
    }

    public IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _luzBrillanteCortinas.enabled = false;
        _luzTenueCortinas.enabled = true;
        StartCoroutine(_cortinaDerecha.CurtainOpen());
        yield return _cortinaIzquierda.CurtainOpen();
        _player.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _focoJugador.enabled = true;
    }

    // TODO Cerrar las cortinas, apagar la luz, poner al player en posicion original y desactivarlo?
    public IEnumerator ResetGameCoroutine(){
        StartCoroutine(_cortinaDerecha.CurtainClose());
        yield return _cortinaIzquierda.CurtainClose();
        _nivel.transform.position = Vector3.zero;
        yield return new WaitForSeconds(1f);
        StartCoroutine(_cortinaDerecha.CurtainOpen());
        yield return _cortinaIzquierda.CurtainOpen();
        _playerPos.GetComponent<Rigidbody>().velocity = Vector3.zero; // ! No funciona, mantiene la inercia
        _playerPos.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        _player.gameObject.SetActive(false);
        _playerPos.transform.position = _playerStartPosition;
        _player.gameObject.SetActive(true);
        yield return null; 
    }
}
