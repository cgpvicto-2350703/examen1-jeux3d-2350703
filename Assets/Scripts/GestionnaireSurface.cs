using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la création et la destruction des surfaces dans la scène afin de suivre le déplacement de la boule. 
/// </summary>
public class GestionnaireSurface : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab des surfaces qui peuvent être initialisées à répétition.")]
    private Surface[] prefabSurface;

    [SerializeField, Tooltip("Position initiale en Z du premier plancher.")]
    private float positionInitialeZ;

    [SerializeField, Tooltip("Nombre de surfaces à créer au démarrage. Est aussi le nombre de surfaces maintenues dans la scène.")]
    private int nombreSurfaces = 5;

    [Header("Paramètres de décalage et d'inclinaison")]
    [SerializeField, Tooltip("Distance horizontale entre les surfaces en unité Unity (mètres)")]
    private float longueurSurface = 10.0f;

    [SerializeField, Tooltip("Décalage en Y entre l'élévation des surfaces (mètres)")]
    private float decalageElevation = -1.0f;

    [SerializeField, Tooltip("Inclinaison en degrés de la surface (0 = horizontale, 90 = verticale)")]
    private float inclinaisonSurface = -5.0f;

    [Header("Paramètres des objets générés sur les surfaces")]
    [SerializeField, Tooltip("Liste des objets collectables qui peuvent être ajoutés aux surfaces.")]
    private GameObject[] collectables;

    [SerializeField, Tooltip("Probabilité d'apparition d'un collectable sur une surface (0.0 = jamais, 1.0 = toujours)"), Range(0.0f, 1.0f)]
    private float probabiliteCollectable = 0.2f;

    // Emplacement de la dernière surface créée en Z
    private float derniereSurfacePositionZ = 0.0f;

    // Emplacement de la dernière surface créée en Y
    private float derniereSurfacePositionY = 0.0f;

    // Liste des surfaces actuellement dans la scène. La queue permet de les supprimer en ordre d'ajout.
    private Queue<Surface> surfaces;

    // Nombre de surfaces instanciées 
    public int SurfacesParcourues { get; private set; }

    private void Awake()
    {
        surfaces = new Queue<Surface>();
        SurfacesParcourues = 0;
    }

    private void Start()
    {
        // Crée la première surface à la position initiale, puis les autres surfaces sont créées derrière elle.
        derniereSurfacePositionZ = positionInitialeZ - longueurSurface;
        derniereSurfacePositionY = 0.0f;

        for (int i = 0; i < nombreSurfaces; i++)
        {
            AjouterSurface();
        }
    }

    /// <summary>
    /// Ajoute une nouvelle surface à la scène, derrière la dernière surface créée.
    /// </summary>
    private void AjouterSurface()
    {
        Surface surfaceAInstancier = SurfacesParcourues < 5 ? prefabSurface[0] : prefabSurface[Random.Range(0, prefabSurface.Length)];
        Surface nouvelleSurface = Instantiate(surfaceAInstancier, transform);
        surfaces.Enqueue(nouvelleSurface);

        // Positionne la nouvelle surface derrière la dernière surface créée.
        float y = derniereSurfacePositionY + decalageElevation;
        float z = derniereSurfacePositionZ + longueurSurface;
        nouvelleSurface.transform.position = new Vector3(0.0f, y, z);
        derniereSurfacePositionZ = z;
        derniereSurfacePositionY = y;
        nouvelleSurface.transform.Rotate(Vector3.forward, inclinaisonSurface);

        AjouterCollectables(nouvelleSurface);

        // Écoute les événements de la surface pour ajouter une nouvelle surface lorsque la boule entre et détruire la surface lorsque la boule sort.
        nouvelleSurface.Detecteur.BouleEntree += AjouterSurface;
        nouvelleSurface.Detecteur.BouleSortie += DetruireSurface;
    }

    /// <summary>
    /// Gère l'ajout de collectables sur la surface donnée. La probabilité d'apparition d'un collectable est déterminée par la variable "probabiliteCollectable".
    /// </summary>
    /// <param name="surface">La surface sur laquelle ajouter les collectables.</param>
    private void AjouterCollectables(Surface surface)
    {
        if(Random.value < probabiliteCollectable && collectables.Length > 0)
        {
            GameObject collectablePrefab = collectables[Random.Range(0, collectables.Length)];
            surface.AjouterObjet(collectablePrefab);
        }
    }

    /// <summary>
    /// Détruit la surface la plus ancienne de la scène lorsque la boule sort de la surface.
    /// </summary>
    private void DetruireSurface()
    {
        Surface surface = surfaces.Dequeue();

        // Retire les événements pour éviter les fuites de mémoire.
        surface.Detecteur.BouleEntree -= AjouterSurface;
        surface.Detecteur.BouleSortie -= DetruireSurface;

        Destroy(surface.gameObject);
        SurfacesParcourues++;
    }
}
