using System.Collections;
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
        public float raioChecagem = 0.2f;
        public LayerMask chaoLayer;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        private bool estaNoChao;
        private bool podePularDuplo; // Verifica se o segundo pulo est� dispon�vel

        private float originalMovingSpeed;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            originalMovingSpeed = movingSpeed; // guarda a velocidade original para o power-up
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            // Verifica se est� no ch�o
            estaNoChao = Physics2D.OverlapCircle(groundCheck.position, raioChecagem, chaoLayer);

            // Se estiver no ch�o, reseta a permiss�o de pulo duplo
            if (estaNoChao)
                Debug.Log("Est� no ch�o");
                podePularDuplo = true;

            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // anima��o de correr
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // anima��o de idle
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (estaNoChao)
                {
                    Pular();

                }
                else if (podePularDuplo)
                {
                    Pular();
                    podePularDuplo = false; // Gasta o segundo pulo
                }
            }

            if (!isGrounded) animator.SetInteger("playerState", 2); // anima��o de pular

            if (facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }

        void Pular()
        {
            // Aplica velocidade vertical instant�nea
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        // Mostra a �rea de checagem do ch�o no editor
        void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, raioChecagem);
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // informa GameManager que o jogador morreu
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                Debug.Log("Coletou");
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }

        // Coroutine para o power-up de velocidade
        public IEnumerator SpeedBoost(float duration, float multiplier)
        {
            movingSpeed = originalMovingSpeed * multiplier;
            yield return new WaitForSeconds(duration);
            movingSpeed = originalMovingSpeed;
        }
    }
}
