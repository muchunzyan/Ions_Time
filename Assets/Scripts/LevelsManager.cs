using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public List<GameObject> levelsButtons = new List<GameObject>();

    private void Start()
    {
        var numberOfLevelsCompleted = PlayerPrefs.GetInt("NumberOfLevelsCompleted");
        for (var i = 0; i < numberOfLevelsCompleted + 1; i++)
        {
            levelsButtons[i].SetActive(true);
        }

        CheckSound();
    }

    public void OpenMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    
    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    
    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    
    public void OpenLevel5()
    {
        SceneManager.LoadScene("Level5");
    }
    
    private void CheckSound()
    {
        if (PlayerPrefs.GetInt("SoundIsOff") == 1)
        {
            if (FindObjectsOfType(typeof(AudioSource)) is AudioSource[] allAudioSources)
                foreach (var audioSource in allAudioSources)
                {
                    audioSource.mute = true;
                }
        }
    }
}
