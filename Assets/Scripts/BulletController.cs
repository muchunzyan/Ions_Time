using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletController : MonoBehaviour
{
    public List<Sprite> phrases = new List<Sprite>();
    public List<string> freeToFireTags = new List<string>();
    public float speed = 20f;
    public float power;
    public bool isRight;
    public GameObject bulletDestroyParticle;
    public bool enemyBullet;
    private string _hitTargetTag;

    private Rigidbody2D _rb;
    private GameManager _gameManager;
    private Animator _animator;
    private static readonly int BossBullet = Animator.StringToHash("bossBullet");
    private static readonly int SimpleBullet = Animator.StringToHash("simpleBullet");

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _gameManager = GameObject.Find("Scripts").GetComponent<GameManager>();

        if (enemyBullet)
        {
            freeToFireTags.Remove("Player");

            if (_gameManager.bossLevel)
            {
                freeToFireTags.Add("Boss");
                freeToFireTags.Add("Bullet");
                _animator.SetTrigger(BossBullet);
            }
            else
            {
                freeToFireTags.Add("Enemy");
                _animator.SetTrigger(SimpleBullet);
            }

            _hitTargetTag = "Player";
        }
        else
        {
            _animator.SetTrigger(SimpleBullet);
            _hitTargetTag = _gameManager.bossLevel ? "Boss" : "Enemy";
        }
        
        var phraseIndex = Random.Range(0, phrases.Count - 1);
        GetComponent<SpriteRenderer>().sprite = phrases[phraseIndex];

        if (isRight)
        {
            _rb.velocity = transform.right * speed;
        }
        else
        {
            _rb.velocity = -transform.right * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_hitTargetTag))
        {
            if (enemyBullet)
            {
                other.gameObject.GetComponent<PlayerController>().isDead = true;
            }
            else
            {
                if (other.CompareTag("Enemy"))
                {
                    var enemyController = other.GetComponent<EnemyController>();
                    enemyController.hp -= power;
                    enemyController.HurtSound();
                    
                }
                else if (other.CompareTag("Boss"))
                {
                    var bossController = other.GetComponent<BossController>();
                    bossController.hp -= power;
                    bossController.HurtSound();
                }
            }
        }

        if (!freeToFireTags.Contains(other.tag))
        {
            var position = transform.position;
            float xPos;

            if (enemyBullet && _gameManager.bossLevel)
            {
                if (isRight)
                {
                    xPos = position.x + 2f;
                }
                else
                {
                    xPos = position.x - 2f;
                }
            }
            else
            {
                if (isRight)
                {
                    xPos = position.x + 0.55f;
                }
                else
                {
                    xPos = position.x - 0.55f;
                }
            }

            var particleSpawnPosition = new Vector3(xPos, position.y, position.z - 1f);
            var newParticle = Instantiate(bulletDestroyParticle, particleSpawnPosition, Quaternion.identity);
            
            if (enemyBullet)
            {
                var mainModule = newParticle.GetComponent<ParticleSystem>().main;

                if (_gameManager.bossLevel)
                {
                    mainModule.startColor = Color.red;
                }
                else
                {
                    mainModule.startColor = Color.yellow;
                }
            }

            Destroy(gameObject);
        }
    }
}
