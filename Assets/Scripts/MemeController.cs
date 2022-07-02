using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MemeController : MonoBehaviour
{
    public List<Sprite> memesSprites = new List<Sprite>();
    
    private void Start()
    {
        var randMemeIndex = Random.Range(0, memesSprites.Count - 1);
        GetComponent<SpriteRenderer>().sprite = memesSprites[randMemeIndex];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Scripts").GetComponent<GameManager>().MemeCollected();

            col.GetComponent<PlayerController>().PlayPickItemSound();

            Destroy(gameObject);
        }
    }
}
