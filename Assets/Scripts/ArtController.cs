using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ArtController : MonoBehaviour
{
    public List<Sprite> artsSprites = new List<Sprite>();
    
    private void Start()
    {
        var randArtIndex = Random.Range(0, artsSprites.Count - 1);
        GetComponent<SpriteRenderer>().sprite = artsSprites[randArtIndex];
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerController>().PlayPickItemSound();
            
            var currentSceneName = SceneManager.GetActiveScene().name;
            var levelNumber = int.Parse(currentSceneName[currentSceneName.Length-1].ToString());
            
            if (PlayerPrefs.GetInt("NumberOfLevelsCompleted") < levelNumber)
                PlayerPrefs.SetInt("NumberOfLevelsCompleted", levelNumber);
            
            SceneManager.LoadScene("Levels");
            
            Destroy(gameObject);
        }
    }
}
