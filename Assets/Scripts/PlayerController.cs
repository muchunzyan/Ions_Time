using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _mRigidbody2D;

    public float runSpeed = 40f;
    public float jumpForce = 500f;
    
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public bool canMove;

    public float horizontalMove;
    private bool _jump;
    private bool _facingRight = true;
    private bool _canPlayStepSound = true;

    private Vector3 _myVelocity;

    public GameObject deathScreen, pauseBtn;

    public bool isDead, showDeathScreen;

    public List<AudioClip> playerHurtSounds = new List<AudioClip>();
    public AudioClip stepSound, shootSound, pickItemSound;
    
    private AudioSource _audioSource;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _mRigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canMove)
        {
            if (isDead)
            {
                StartCoroutine(Death());
            }
            
            horizontalMove = Input.GetAxisRaw("Horizontal");
            _animator.SetFloat(Speed, Mathf.Abs(horizontalMove));

            if (Mathf.Abs(_mRigidbody2D.velocity.y) < 0.01f && !_jump)
            {
                _animator.SetBool(IsJumping, false);

                if (Input.GetButtonDown("Jump"))
                {
                    _jump = true;
                    _animator.SetBool(IsJumping, true);
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (GetComponentInChildren<ShootingPointController>().canShoot)
                {
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
                    newBullet.GetComponent<BulletController>().isRight =
                        shootPoint.rotation == new Quaternion(0f, 0f, 0f, 1f);

                    _audioSource.clip = shootSound;
                    _audioSource.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontalMove) > 0 && Mathf.Abs(_mRigidbody2D.velocity.y) < 0.01f && _canPlayStepSound && canMove)
        {
            StartCoroutine(PlayStepSound());
        }
        
        var rbVelocity = _mRigidbody2D.velocity;
        Vector3 targetVelocity = new Vector2(horizontalMove * runSpeed, rbVelocity.y);
        _mRigidbody2D.velocity = Vector3.SmoothDamp(rbVelocity, targetVelocity, ref _myVelocity, 0.05f);


        if (_jump)
        {
            _mRigidbody2D.AddForce(new Vector2(0, jumpForce));
            _jump = false;
        }

        if (horizontalMove > 0 && !_facingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0 && _facingRight)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            if (canMove)
                StartCoroutine(Death());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Boss"))
        {
            if (canMove)
                StartCoroutine(Death());
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public void StopPlayer()
    {
        canMove = false;
        horizontalMove = 0;
        _animator.SetFloat(Speed, 0);
    }

    public void StartPlayer()
    {
        canMove = true;
    }

    private IEnumerator Death()
    {
        canMove = false;
        pauseBtn.SetActive(false);

        _audioSource.clip = playerHurtSounds[Random.Range(0, playerHurtSounds.Count)];
        _audioSource.Play();

        _mRigidbody2D.velocity = Vector2.zero;
        _mRigidbody2D.AddForce(new Vector2(0, 500));
        
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        
        yield return new WaitForSeconds(0.6f);

        if (showDeathScreen)
        {
            deathScreen.transform.GetChild(2).gameObject.SetActive(true);
            deathScreen.SetActive(true);
        }

        yield return new WaitForSeconds(2.5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PlayStepSound()
    {
        _canPlayStepSound = false;
        
        _audioSource.clip = stepSound;
        _audioSource.Play();

        yield return new WaitForSeconds(0.2f);
        _canPlayStepSound = true;
    }

    public void PlayPickItemSound()
    {
        _audioSource.clip = pickItemSound;
        _audioSource.Play();
    }
}
