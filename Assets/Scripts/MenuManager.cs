using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text soundBtnText;
    
    private void Start()
    {
        CheckSound();
    }

    public void OpenLevelsScene()
    {
        SceneManager.LoadScene("Levels");
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
