using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Graphe
{
    /// <summary>
    /// Collection complète des noeuds constituant le graphe
    /// </summary>
    public List<Noeud> NoeudsGraphe;
    /// <summary>
    /// Matrice carrée représentant les connexions entre noeuds
    /// - 0 : Pas de connexion
    /// - 1 : Connexion existante
    /// La matrice est symétrique pour les graphes non orientés
    /// </summary>
    public int[,] matriceAdjacence;
    /// <summary>
    /// Taille théorique maximale de la matrice d'adjacence
    /// Déterminée par l'ID maximum des noeuds + 1
    /// </summary>
    public int taille;
    public Graphe(List<Noeud> noeuds)
    {
        this.NoeudsGraphe = noeuds;
        this.taille = noeuds.Count + 1;
        this.matriceAdjacence = new int[taille, taille];

        // construction de la matrice d'adjacence
        foreach (var noeud in NoeudsGraphe)
        {
            foreach (var voisinId in noeud.ListeAdjacence)
            {
                int sourceIndex = noeud.Id;
                int cibleIndex = voisinId;

                matriceAdjacence[sourceIndex, cibleIndex] = 1;
                matriceAdjacence[cibleIndex, sourceIndex] = 1; 
            }
        }
    }
    public void AfficherListeNoeuds()
    {
        Console.WriteLine("Liste des Noeuds:");
        foreach (var noeud in NoeudsGraphe)
        {
            Console.WriteLine($"Noeud ID: {noeud.Id}, Voisins: [{string.Join(", ", noeud.ListeAdjacence)}]");
        }
    }

    public void AfficherMatriceAdjacence()
    {
        Console.WriteLine("\nMatrice d'Adjacence:");
        for (int i = 0; i < taille; i++)
        {
            for (int j = 0; j < taille; j++)
            {
                Console.Write(matriceAdjacence[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public void AfficherTailleGraphe()
    {
        Console.WriteLine($"\nTaille du graphe: {taille}");
    }

    public void AfficherGraphe()
    {
        AfficherListeNoeuds();
        AfficherMatriceAdjacence();
        AfficherTailleGraphe();
    }
    public void AjouterLien(Lien lien)
    {

        if (lien.Source.Id < taille && lien.Cible.Id < taille)
        {
            matriceAdjacence[lien.Source.Id, lien.Cible.Id] = 1;
            matriceAdjacence[lien.Cible.Id, lien.Source.Id] = 1;

            lien.Source.ListeAdjacence.Add(lien.Cible.Id);
            lien.Cible.ListeAdjacence.Add(lien.Source.Id);
        }
        else
        {
            Console.WriteLine("Erreur : l'un des nœuds n'existe pas dans le graphe.");
        }
    }

    /// <summary>
    /// Exécute un parcours en largeur (BFS) à partir d'un noeud spécifié
    /// Utilise un système de coloration :
    /// - Blanc : Non découvert
    /// - Jaune : Découvert mais non traité
    /// - Rouge : Traité
    /// </summary>
    public void ParcoursLargeur(int depart)
    {
        Queue<int> file = new Queue<int>();
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
       
        List<int> OrdreTerminé = new List<int>();

        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";                      
        }

        file.Enqueue(depart);
        etatSommets[depart] = "Jaune";

        while (file.Count > 0)
        {
            int sommetActuel = file.Dequeue();
            etatSommets[sommetActuel] = "Rouge";
            OrdreTerminé.Add(sommetActuel);


          
            foreach (var noeud in NoeudsGraphe)
            {
                if (noeud.Id == sommetActuel)
                {
                    foreach (var voisin in noeud.ListeAdjacence)
                    {
                        if (etatSommets[voisin] == "Blanc")
                        {
                            etatSommets[voisin] = "Jaune";
                            file.Enqueue(voisin);
                        }
                    }
                }
            }
        }
        string ligne = "";
        foreach (var sommet in OrdreTerminé)
        {
            ligne += sommet.ToString() + ",";
        }

        Console.WriteLine("Fin du parcours en largeur.");
        Console.WriteLine(ligne);
    }
    /// <summary>
    /// Exécute un parcours en profondeur (DFS) à partir d'un noeud spécifié
    /// Utilise le même système de coloration
    /// </summary>
    public void ParcoursProfondeur(int depart)
    {
        Stack<int> pile = new Stack<int>();
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
        List<int> OrdreTerminé = new List<int>();

        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";
        }

        pile.Push(depart);
        etatSommets[depart] = "Jaune";

        while (pile.Count > 0)
        {
            int sommetActuel = pile.Pop();
            etatSommets[sommetActuel] = "Rouge";
            OrdreTerminé.Add(sommetActuel);

            foreach (var noeud in NoeudsGraphe)
            {
                if (noeud.Id == sommetActuel)
                {
                    foreach (var voisin in noeud.ListeAdjacence)
                    {
                        if (etatSommets[voisin] == "Blanc")
                        {
                            etatSommets[voisin] = "Jaune";
                            pile.Push(voisin);
                        }
                    }
                }
            }
        }

        string ligne = "";
        foreach (var sommet in OrdreTerminé)
        {
            ligne += sommet.ToString() + ",";
        }
        Console.WriteLine("Fin du parcours en profondeur.");
        Console.WriteLine(ligne);
    }

    /// <summary>
    /// Détermine si le graphe est connexe (tous les noeuds accessibles depuis n'importe quel noeud)
    /// Implémentation basée sur un parcours en profondeur itératif
    /// Vérifie l'absence de noeuds non découverts après parcours
    /// </summary>
    public bool EstConnexe()
    {

        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
        List<int> OrdreTerminé = new List<int>();

        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";
        }

        int premierSommet = NoeudsGraphe[0].Id;

        Stack<int> pile = new Stack<int>();
        pile.Push(premierSommet);
        etatSommets[premierSommet] = "Jaune";

        while (pile.Count > 0)
        {
            int sommetActuel = pile.Peek();
            bool aVoisinNonVisite = false;

            Noeud noeudActuel = NoeudsGraphe.FirstOrDefault(n => n.Id == sommetActuel);
            if (noeudActuel == null) continue; 
            foreach (var voisin in noeudActuel.ListeAdjacence)
            {
                if (etatSommets[voisin] == "Blanc" && etatSommets[voisin] == "Blanc")
                {
                    etatSommets[voisin] = "Jaune";
                    pile.Push(voisin);
                    aVoisinNonVisite = true;
                }
            }

            if (!aVoisinNonVisite)
            {
                etatSommets[sommetActuel] = "Rouge";
                OrdreTerminé.Add(sommetActuel);
                pile.Pop();
            }
        }
        bool connexe = !etatSommets.Values.Contains("Blanc");

        Console.WriteLine("Ordre de marquage terminé (Rouge) : " + string.Join(",", OrdreTerminé));

        if (connexe)
        {
            Console.WriteLine("Le graphe est connexe.");
        }
        else
        {
            Console.WriteLine("Le graphe n'est PAS connexe.");
        }
        return connexe;
    }
    /// <summary>
    /// Vérifie la présence d'au moins un cycle dans le graphe en utilisant une approche récursive
    /// </summary>
    public bool ContientCycle()
    {
        // Dictionnaire pour suivre l'état de chaque nœud :
        // "Blanc" = Non visité, "Jaune" = En cours de visite, "Rouge" = Visité
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();

        // Initialisation de tous les nœuds en "Blanc"
        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";
        }

        // Parcours de tous les nœuds pour détecter les composantes connexes
        foreach (var noeud in NoeudsGraphe)
        {
            // Lance le DFS seulement si le nœud n'a pas été visité
            if (etatSommets[noeud.Id] == "Blanc" && ContientCycleAuxiliaire(noeud.Id, etatSommets, -1))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Méthode récursive auxiliaire pour la détection de cycles
    /// </summary>
    public bool ContientCycleAuxiliaire(int sommet, Dictionary<int, string> etatSommets, int parent)
    {
        // Marque le nœud courant comme étant en cours de traitement
        etatSommets[sommet] = "Jaune";
        // Explore tous les voisins du nœud actuel
        foreach (var voisin in NoeudsGraphe[sommet].ListeAdjacence)
        {
            // Cas 1 : Voisin non exploré -> Exploration récursive
            if (etatSommets[voisin] == "Blanc")
            {
                if (ContientCycleAuxiliaire(voisin, etatSommets, sommet))
                {
                    return true;
                }
            }
            // Cas 2 : Voisin déjà en cours d'exploration (Jaune) et différent du parent 
            // -> Cycle détecté !
            else if (voisin != parent && etatSommets[voisin] == "Jaune")
            {
                Console.WriteLine("Cycle détecté via le sommet " + voisin + " !");
                return true;
            }
        }
        // Fin du traitement : marque le nœud comme complètement visité
        etatSommets[sommet] = "Rouge"; 
        Console.WriteLine("Fin du traitement du sommet " + sommet + " (Rouge)");
        return false;
    }
}
