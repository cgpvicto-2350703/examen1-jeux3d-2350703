using UnityEngine;

/// <summary>
/// Détecteur de boule qui déclenche des événements lorsque la boule entre ou sort d'une zone.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DetecteurBoule : MonoBehaviour
{
    /// <summary>
    /// La boule est entrée dans la zone de détection.
    /// </summary>
    public event System.Action BouleEntree;

    /// <summary>
    /// La boule est sortie de la zone de détection.
    /// </summary>
    public event System.Action BouleSortie;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            BouleEntree?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            BouleSortie?.Invoke();
        }
    }
}
