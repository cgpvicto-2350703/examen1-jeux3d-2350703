using UnityEngine;

/// <summary>
/// Reinitialise la scène lorsqu'un objet entre en collision avec le détecteur de vide.
/// </summary>
public class DetecteurVide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            // Reinitialise la scène en rechargeant la scène actuelle
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
