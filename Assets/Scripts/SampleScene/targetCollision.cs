using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ScoreManager.instance.AddScore(1);
            Destroy(collision.gameObject); // optionnel : supprimer le projectile
        }
    }
}
