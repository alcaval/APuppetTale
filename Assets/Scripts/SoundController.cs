using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _deathSounds, _mainClips;
    [SerializeField] private AudioClip _audienceClap;
    [SerializeField] private AudioSource _source, _mainMusicSource;

    private void OnEnable() 
    {
        GameObject[] soundControllers = GameObject.FindGameObjectsWithTag("SoundController"); 
        if(soundControllers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetSoundController(this);
        if(globales.reset == 1)
        {
            PlayMainMusic();
        }
    }

    public void PlayClap()
    {
        _source.clip = _audienceClap;
        _source.Play();
    }

    public void PlayDeath()
    {
        _mainMusicSource.Stop();

        int index;
        index = Random.Range(0, _deathSounds.Count);
        _source.clip = _deathSounds[index];      
        _source.Play();
    }

    public void PlayMainMusic()
    {
        Debug.Log("A tocar musiquilla");
        int index = Random.Range(0, _mainClips.Count);
        _mainMusicSource.clip = _mainClips[index];      
        _mainMusicSource.Play();
    }

    public void StopMainAudio()
    {
        StartCoroutine(FadeOutCoroutine(_mainMusicSource));
    }

    private IEnumerator FadeOutCoroutine(AudioSource source)
    {
        for(float i=0; i<2f; i+=Time.deltaTime)
        {
            source.volume = Mathf.Lerp(2f, 0f, i/2f);
            Debug.Log(source.volume);
            yield return null;
        }

        source.Stop();
        source.volume = 1f;
    }
}
