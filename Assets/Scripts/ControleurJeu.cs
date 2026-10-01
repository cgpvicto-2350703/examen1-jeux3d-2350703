using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contrôleur de jeu qui gère l'état global du jeu et fournit un accès centralisé aux contrôles et aux autres systèmes.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    /// <summary>
    /// Instance singleton du contrôleur de jeu.
    /// </summary>
    public static ControleurJeu Instance { get; private set; }

    [field:SerializeField, Tooltip("Référence aux contrôles du joueur.")]
    public PlayerInput Controles { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
