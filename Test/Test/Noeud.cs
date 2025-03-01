using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Représente un noeud du graphe avec ses connexions
/// </summary>
public class Noeud
{
    /// <summary>
    /// Identifiant numérique unique du noeud
    /// Servant également d'index dans la matrice d'adjacence
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Liste des id des noeuds directement connectés
    /// </summary>
    public List<int> ListeAdjacence { get; set; }

    public Noeud(int id)
    {
        Id = id;
        ListeAdjacence = new List<int>();
    }
    /// <summary>
    /// Établit une relation de voisinage avec un autre noeud
    /// Garantit l'unicité des connexions et maintient la cohérence bidirectionnelle
    /// </summary>
    public void AjouterVoisin(Noeud voisin)
    {
        if (!ListeAdjacence.Contains(voisin.Id))
        {
            ListeAdjacence.Add(voisin.Id);
        }
    }
}

