using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    public List<Transform> movePositions = new List<Transform>();
    private bool _isDead;
    private Vector3 _targetPos;
    public float moveSpeed;
    public float hp;
    private Rigidbody2D _rb;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject deathScreen;
    public Image bossBar;
    private AudioSource _audioSource;
    public AudioClip enemyHurtSound, shootSound;
    public PlayerController playerController;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        
        _targetPos = transform.position;
        StartCoroutine(MoveToPositionAndShoot());
    }

    private void Update()
    {
        if (!_isDead)
        {
            if (hp <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                bossBar.fillAmount = hp / 100;
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator MoveToPositionAndShoot()
    {
        while (!_isDead)
        {
            _targetPos = movePositions[Random.Range(0, movePositions.Count)].position;
            yield return new WaitForSeconds(1);
            
            _audioSource.clip = shootSound;
            _audioSource.Play();
            var newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            var bulletController = newBullet.GetComponent<BulletController>();
            bulletController.enemyBullet = true;
            bulletController.speed = 6;
            bulletController.isRight = false;
            newBullet.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private IEnumerator Death()
    {
        _isDead = true;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().showDeathScreen = false;

        _rb.gravityScale = 1;
        _rb.AddForce(new Vector2(0, 500));
        
        transform.position += new Vector3(0, 0, -1);
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        
        playerController.canMove = false;
        
        yield return new WaitForSeconds(4);
        
        deathScreen.transform.GetChild(1).gameObject.SetActive(true);
        deathScreen.SetActive(true);
        
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
        
        Destroy(gameObject);
    }

    public void HurtSound()
    {
        _audioSource.clip = enemyHurtSound;
        _audioSource.Play();
    }
}
