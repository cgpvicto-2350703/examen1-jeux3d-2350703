using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Affiche les champs d'information dans l'interface
/// </summary>
public class InformationInterface : MonoBehaviour
{
    [SerializeField, Tooltip("Référence au texte affichant le nombre de surfaces parcourues.")]
    TextMeshProUGUI texteNombreSurfacesParcourues;

    [SerializeField, Tooltip("Référence au texte affichant le temps écoulé.")]
    TextMeshProUGUI texteTempsEcoule;

    [SerializeField, Tooltip("Référence au texte affichant la texteVitesse.")]
    TextMeshProUGUI texteVitesse;

    [SerializeField, Tooltip("Référence au gestionnaire de surfaces.")]
    private GestionnaireSurface gestionnaireSurface;

    [SerializeField, Tooltip("Référence à la boule.")]
    private Boule boule;

    // / Temps écoulé depuis le début du jeu
    private float tempsEcoule;

    private void Start()
    {
        tempsEcoule = 0f;
        ControleurJeu.Instance.Controles.actions.FindAction("Commencer").performed += CommencerJeu;
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null)
            return;

        PlayerInput controles = ControleurJeu.Instance.Controles;

        if (controles == null)
            return;

        controles.actions.FindAction("Commencer").performed -= CommencerJeu;
    }

    /// <summary>
    /// Commence le compteur de temps de jeu lorsque l'action "Commencer" est déclenchée.
    /// </summary>
    /// <param name="contexte"></param>
    private void CommencerJeu(InputAction.CallbackContext contexte)
    {
        ControleurJeu.Instance.Controles.actions.FindAction("Commencer").performed -= CommencerJeu;
        tempsEcoule = 0f;
        StartCoroutine(CompterTemps());
    }

    /// <summary>
    /// Compteur de temps qui s'incrémente chaque frame tant que le jeu est en cours.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompterTemps()
    {
        while (true)
        {
            tempsEcoule += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        texteTempsEcoule.text = $"{tempsEcoule:F2}";
    
        texteVitesse.text = $"{boule.Velocite.magnitude:F2}";
        texteNombreSurfacesParcourues.text = gestionnaireSurface.SurfacesParcourues.ToString();
    }
}
