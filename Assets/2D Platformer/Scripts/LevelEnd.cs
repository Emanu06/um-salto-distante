using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // info sobre o collider que entrou
        string colliderName = other.name;
        string colliderTag = other.tag;
        string rootName = other.transform.root != null ? other.transform.root.name : "null";
        string rbOwner = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject.name : "none";

        Debug.Log("LevelEnd Trigger -> colliderName: " + colliderName +
                  ", tag: " + colliderTag +
                  ", root: " + rootName +
                  ", attachedRigidbodyOwner: " + rbOwner);

        // checar tanto o collider quanto o gameObject dono do Rigidbody (caso a tag esteja no pai)
        bool isPlayer = other.CompareTag("Player") ||
                        (other.attachedRigidbody != null && other.attachedRigidbody.gameObject.CompareTag("Player"));

        if (isPlayer)
        {
            Debug.Log("Player detectado — carregando cena 'Game Over'");
            SceneManager.LoadScene("Game Over");
        }
    }
}
