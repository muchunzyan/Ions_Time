using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool hardEnemy;
    public float hp;
    public float runSpeed;
    public float attackRange;
    public float timeBtwShots;
    private float _distToPlayer;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    private bool _isDead;

    public bool mustPatrol = true;
    private bool _canShoot = true;
    private bool _mustFlip;
    public Collider2D flipCollider;

    private Rigidbody2D _rb;
    public Transform platformEndChecker;
    public LayerMask groundLayer;
    private Transform _playerTransform;
    private AudioSource _audioSource;
    public AudioClip shootSound, enemyHurtSound;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.Find("Player").transform;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isDead)
        {
            if (mustPatrol)
            {
                Patrol();
            }

            if (hp <= 0)
            {
                StartCoroutine(Death());
            }

            if (hardEnemy)
            {
                _distToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

                if (_distToPlayer < attackRange)
                {
                    if (_playerTransform.position.x > transform.position.x &&
                        transform.rotation == new Quaternion(0, -1, 0, 0) ||
                        _playerTransform.position.x < transform.position.x &&
                        transform.rotation == new Quaternion(0, 0, 0, 1))
                    {
                        Flip();
                    }

                    mustPatrol = false;
                    _rb.velocity = Vector2.zero;

                    if (_canShoot)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    mustPatrol = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            _mustFlip = !Physics2D.OverlapCircle(platformEndChecker.position, 0.1f, groundLayer);
        }
    }

    private void Patrol()
    {
        if (_mustFlip || flipCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        _rb.velocity = new Vector2(runSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }
    
    private void Flip()
    {
        mustPatrol = false;
        transform.Rotate(0f, 180f, 0f);
        runSpeed *= -1;
        mustPatrol = true;
    }

    private IEnumerator Death()
    {
        _isDead = true;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(0, 500));
        
        transform.position += new Vector3(0, 0, -1);
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private IEnumerator Shoot()
    {
        _canShoot = false;
        
        _audioSource.clip = shootSound;
        _audioSource.Play();
        
        var newBulletPos = shootPoint.position;
        if (shootPoint.rotation == new Quaternion(0f, 0f, 0f, 1f))
        {
            newBulletPos += new Vector3(-0.3f, 0, 0);
        }
        else
        {
            newBulletPos += new Vector3(0.3f, 0, 0);
        }

        var newBullet = Instantiate(bulletPrefab, newBulletPos, Quaternion.identity);
        var bulletController = newBullet.GetComponent<BulletController>();
        bulletController.enemyBullet = true;
        bulletController.speed = 6;
        bulletController.isRight = shootPoint.rotation == new Quaternion(0f, 0f, 0f, 1f);
        newBullet.GetComponent<SpriteRenderer>().color = Color.yellow;
        
        yield return new WaitForSeconds(timeBtwShots);
        _canShoot = true;
    }

    public void HurtSound()
    {
        _audioSource.clip = enemyHurtSound;
        _audioSource.Play();
    }
}
