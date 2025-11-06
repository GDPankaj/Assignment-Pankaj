using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        [SerializeField] Joystick joystick;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        public void SetMoveInput(float x)
        {
            if (x > 0)
            {
                moveInput = 1f;
            }
            else if (x < 0)
            {
                moveInput = -1f;
            }
            else
            {
                moveInput = 0f;
            }
        }

        void Update()
        {
            if (!canShoot)
            {
                timeSinceShoot += Time.deltaTime;
                if(timeSinceShoot > 0.5)
                {
                    canShoot = true;
                }
            }
            /*if(canShoot *//*&& Input.GetMouseButton(0)*//*)
            {
                OnShoot();
            }*/
            moveInput = joystick.Horizontal;
            if (Input.GetButton("Horizontal") || moveInput != 0) 
            {
                //moveInput = Input.GetAxis("Horizontal");
                
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded )
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation

            if(facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if(facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }
        private bool canJump = false;
        public void OnJump()
        {
            if (canJump)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                canJump = false;
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = canJump = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        public LevelManager level;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                level.coins.Remove(other.gameObject);
                level.CheckLevelEnd();
                Destroy(other.gameObject);
            }
        }

        [SerializeField] private GameObject throwable;
        private bool canShoot;
        private float timeSinceShoot = 0;
        public void OnShoot()
        {
            if (canShoot)
            {
                timeSinceShoot = 0;
                canShoot = false;

                GameObject go = Instantiate(throwable, transform.position + (facingRight ? Vector3.right : Vector3.left), Quaternion.identity);
                go.GetComponent<Throwable>().OnSpawn(facingRight ? Vector2.right : Vector2.left, level);
            }
        }
    }
}
