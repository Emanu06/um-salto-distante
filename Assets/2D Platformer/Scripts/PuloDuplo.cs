using UnityEngine;

public class DubleJump : MonoBehaviour
{
    public float velocidade = 5f;   // Velocidade horizontal
    public float forcaPulo = 8f;    // For�a do pulo

    // Configura��o para detectar o ch�o
    public Transform checarChao;
    public float raioChecagem = 0.2f;
    public LayerMask chaoLayer;

    // Componentes e controle de pulo
    private Rigidbody2D rb;
    private bool estaNoChao;
    private bool podePularDuplo; // Verifica se o segundo pulo est� dispon�vel

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura movimento lateral
        float movimento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);

        // Verifica se est� no ch�o
        estaNoChao = Physics2D.OverlapCircle(checarChao.position, raioChecagem, chaoLayer);

        // Se estiver no ch�o, reseta a permiss�o de pulo duplo
        if (estaNoChao)
            podePularDuplo = true;

        // Pressionar espa�o executa o pulo ou duplo pulo
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

        // Espelha o sprite de acordo com a dire��o
        if (movimento != 0)
            transform.localScale = new Vector3(Mathf.Sign(movimento), 1f, 1f);
    }

    void Pular()
    {
        // Aplica velocidade vertical instant�nea
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
    }

    // Mostra a �rea de checagem do ch�o no editor
    void OnDrawGizmosSelected()
    {
        if (checarChao == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checarChao.position, raioChecagem);
    }
}