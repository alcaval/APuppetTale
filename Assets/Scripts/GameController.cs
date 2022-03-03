using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Cortinas")]
    [SerializeField] private CurtainOpenTest _curtains;

    [Header("UI")]
    [SerializeField] private Canvas _startMenu;
    [SerializeField] private Canvas _pauseMenu;
    [SerializeField] private Canvas _tutorialMenu;
    [SerializeField] private Button _startLevelButton;

    [Header("Nivel")]
    // [SerializeField] private TriggerDetection _deadTrigger;
    [SerializeField] private LevelProgression _levelProgression;
    [SerializeField] private GameObject limites;
    [SerializeField] private SoundController _soundController;
    [SerializeField] private Transform _meta;
    [SerializeField] private GameObject _margenIzquierdoVictoria;

    [Header("Player")]
    [SerializeField] private GameObject _playerRagdoll;
    [SerializeField] private GameObject _playerPos;
    [SerializeField] private ragdollController _ragdollController;
    [SerializeField] private Rigidbody _playerRigidbody;


    [Header("Luces")]
    [SerializeField] private Light _luzBrillanteCortinas;
    [SerializeField] private Light _focoJugador;
    [SerializeField] private Light _luzTenueCortinas;

    private Vector3 _playerStartPosition;
    private Coroutine _resetCoroutine;
    private Coroutine _victoryCoroutine;

    private bool _started = false;
    private bool _levelStarted = false;
    private bool _paused = false;
    private float levelSpeed = 4f;

    private void Start()
    {
        // PlayerPrefs.SetInt("Reset", 0);
        _playerStartPosition = _playerRagdoll.transform.position;
        if (globales.reset != 0)
        {
            StartCoroutine(setupAfterReset());
        }

        if (_soundController == null)
        {
            Debug.Log("El sound controller es null");
            _soundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        }
    }

    public IEnumerator setupAfterReset()
    {

        _startMenu.gameObject.SetActive(false);

        _margenIzquierdoVictoria.SetActive(false);
        limites.gameObject.SetActive(false);

        _luzBrillanteCortinas.enabled = false;
        _luzBrillanteCortinas.intensity = 1f;
        _luzTenueCortinas.enabled = true;
        yield return _curtains.CurtainOpen(0.2f);
        _playerRagdoll.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _focoJugador.enabled = true;

        yield return new WaitForSeconds(1f);

        _levelProgression.SetSpeed(PlayerPrefs.GetFloat("levelSpeed"));
        _resetCoroutine = null;
        _levelStarted = true;
        globales.reset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Tutorial
        if (_started)
        {
            if (_ragdollController.jumpCount > 3)
            {
                _startLevelButton.gameObject.SetActive(true);
            }
        }

        //Start Level
        if (_levelStarted)
        {
            if (_playerPos.transform.position.x >= _meta.position.x && _victoryCoroutine == null)
            {
                _victoryCoroutine = StartCoroutine(VictoryCoroutine());
            }
        }

        //Check if dead
        if (_playerPos.transform.position.x < -16f && _resetCoroutine == null)
        {
            _resetCoroutine = StartCoroutine(ResetGameCoroutine());
        }

        //Pausa
        if (Input.GetKeyDown(KeyCode.Escape) && (_started || _levelStarted) && !_curtains.getAnimation())
        {
            if (_paused)
            {
                ContinueGame();
            }
            else
            {
                StartCoroutine(pauseGame());
            }
        }
    }

    public void StartTutorial()
    {
        levelSpeed = _startMenu.GetComponentInChildren<Slider>().value;
        PlayerPrefs.SetFloat("levelSpeed", levelSpeed);
        _startMenu.gameObject.SetActive(false);
        StartCoroutine(StartTutorialCoroutine());
    }

    public void StartLevel()
    {
        _started = false;
        _tutorialMenu.gameObject.SetActive(false);
        limites.gameObject.SetActive(false);
        StartCoroutine(ResetGameCoroutine());
    }

    private IEnumerator pauseGame()
    {
        yield return _curtains.CurtainClose(1.5f);
        // yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        _pauseMenu.gameObject.SetActive(true);
        _paused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame() => StartCoroutine(continueGameCoroutine());

    private IEnumerator continueGameCoroutine()
    {
        Time.timeScale = 1;
        print("uwu");
        _pauseMenu.gameObject.SetActive(false);
        yield return _curtains.CurtainOpen(1.5f);
        _paused = false;
    }

    public IEnumerator StartTutorialCoroutine()
    {
        _soundController.PlayClap();

        // yield return new WaitForSeconds(2f);
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            _luzBrillanteCortinas.intensity = Mathf.Lerp(1f, 0f, i / 1f);
            yield return null;
        }

        _luzBrillanteCortinas.enabled = false;
        _luzBrillanteCortinas.intensity = 1f;
        _luzTenueCortinas.enabled = true;
        yield return _curtains.CurtainOpen(0.2f);
        _playerRagdoll.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _focoJugador.enabled = true;
        _tutorialMenu.gameObject.SetActive(true);
        _started = true;
    }

    public IEnumerator ResetGameCoroutine()
    {
        _focoJugador.gameObject.SetActive(false);
        if (_playerPos.transform.position.x < -16f)
        {
            _soundController.PlayDeath();
        }

        _levelProgression.SetSpeed(0f);

        yield return _curtains.CurtainClose(1.5f);
        _playerRagdoll.gameObject.SetActive(false);
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(2f);

        globales.reset = 1;
        //PlayerPrefs.Save();
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        
        yield return null;
    }

    public IEnumerator VictoryCoroutine()
    {
        print("victoria");
        _margenIzquierdoVictoria.SetActive(true);

        for (float i = 0; i <= 5f; i += Time.deltaTime)
        {
            _levelProgression.SetSpeed(Mathf.Lerp(PlayerPrefs.GetFloat("levelSpeed"), 0, i / 5f));
            yield return null;
        }

        _levelProgression.SetSpeed(0);

        _soundController.PlayClap();
        yield return new WaitForSeconds(5f);
        _soundController.StopMainAudio();
        yield return _curtains.CurtainClose(1.5f);

        _luzBrillanteCortinas.enabled = true;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            _luzBrillanteCortinas.intensity = Mathf.Lerp(0f, 1f, i / 1f);
            _focoJugador.intensity = Mathf.Lerp(1f, 0f, i / 1f);
            yield return null;
        }
        _luzBrillanteCortinas.intensity = 1;
        _focoJugador.enabled = false;

        yield return new WaitForSeconds(2f);

        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void SetSoundController(SoundController soundController)
    {
        if (_soundController != null)
        {
            Debug.Log("Se intenta reescribir");
            return;
        }
        _soundController = soundController;
    }
}
