using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _numberOfMemesToCollect;
    private int _numberOfCollectedMemes;

    public GameObject artPrefab;
    public Transform artSpawnPoint;
    public Text memesText;
    public CinemachineVirtualCamera cMvCam1;
    public GameObject collectTheArtText;
    public GameObject pauseScreen, pauseBtn;
    private PlayerController _playerController;
    private bool _menuIsRead;
    public bool bossLevel;
    public GameObject boss;
    public GameObject bossHealthBar;
    public Text soundBtnText;

    private void Start()
    {
        CheckSound();
        
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (!bossLevel)
        {
            _numberOfMemesToCollect = 3;
            memesText.text = $"Memes:{_numberOfCollectedMemes}/{_numberOfMemesToCollect}";
        }
        else
        {
            StartCoroutine(BossAppear());
        }
    }

    public void MemeCollected()
    {
        _numberOfCollectedMemes += 1;
        memesText.text = $"Memes:{_numberOfCollectedMemes}/{_numberOfMemesToCollect}";
        
        if (_numberOfCollectedMemes >= _numberOfMemesToCollect)
        {
            StartCoroutine(ShowArt());
        }
    }

    private IEnumerator ShowArt()
    {
        _playerController.StopPlayer();
        
        cMvCam1.Priority *= -1;
        yield return new WaitForSeconds(2.5f);
        Instantiate(artPrefab, artSpawnPoint.position + new Vector3(0, 0, 1), Quaternion.identity);
        yield return new WaitForSeconds(1.8f);
        collectTheArtText.SetActive(true);
        cMvCam1.Priority *= -1;
        
        yield return new WaitForSeconds(2);
        _playerController.StartPlayer();
    }

    public void Pause()
    {
        _playerController.StopPlayer();
        
        pauseBtn.SetActive(false);
        pauseScreen.SetActive(true);
        
        Time.timeScale = 0;
    }

    public void Play()
    {
        _playerController.StartPlayer();
        
        pauseScreen.SetActive(false);
        pauseBtn.SetActive(true);
        
        Time.timeScale = 1;
    }

    public void OnMenuBtnClick()
    {
        if (!_menuIsRead)
        {
            GameObject.Find("MenuBtn").transform.GetChild(1).GetComponent<Text>().color = Color.red;
            _menuIsRead = true;
        }
        else
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
    }

    private IEnumerator BossAppear()
    {
        yield return new WaitForSeconds(1);
        _playerController.StopPlayer();
        yield return new WaitForSeconds(1);
        
        cMvCam1.Priority *= -1;
        yield return new WaitForSeconds(2);
        
        boss.SetActive(true);
        bossHealthBar.SetActive(true);
        
        yield return new WaitForSeconds(4);
        cMvCam1.Priority *= -1;
        
        yield return new WaitForSeconds(2);
        _playerController.StartPlayer();
    }
    
    private void CheckSound()
    {
        soundBtnText.text = PlayerPrefs.GetInt("SoundIsOff") == 0 ? "Sound:On" : "Sound:Off";

        if (PlayerPrefs.GetInt("SoundIsOff") == 1)
        {
            if (FindObjectsOfType(typeof(AudioSource)) is AudioSource[] allAudioSources)
                foreach (var audioSource in allAudioSources)
                {
                    audioSource.mute = true;
                }
        }
    }
    
    public void ChangeSound()
    {
        PlayerPrefs.SetInt("SoundIsOff", PlayerPrefs.GetInt("SoundIsOff") == 0 ? 1 : 0);
        soundBtnText.text = PlayerPrefs.GetInt("SoundIsOff") == 0 ? "Sound:On" : "Sound:Off";
        
        if (PlayerPrefs.GetInt("SoundIsOff") == 1)
        {
            if (FindObjectsOfType(typeof(AudioSource)) is AudioSource[] allAudioSources)
                foreach (var audioSource in allAudioSources)
                {
                    audioSource.mute = true;
                }
        }
        else
        {
            if (FindObjectsOfType(typeof(AudioSource)) is AudioSource[] allAudioSources)
                foreach (var audioSource in allAudioSources)
                {
                    audioSource.mute = false;
                }
        }
    }
}
