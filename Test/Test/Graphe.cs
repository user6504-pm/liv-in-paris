using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
public class Graphe
{
    public List<Noeud> NoeudsGraphe;
    public int[,] matriceAdjacence;
    public int taille;
    public Graphe(List<Noeud> noeuds)
    {
        this.NoeudsGraphe = noeuds;
        this.taille = noeuds.Count + 1;
        this.matriceAdjacence = new int[taille, taille];

        //construction de la matrice d'adjacence
        foreach (var noeud in NoeudsGraphe)
        {
            foreach (var voisinId in noeud.ListeAdjacence)
            {
                // Trouver l'indice du voisin dans la liste des noeuds
                int sourceIndex = noeud.Id;
                int cibleIndex = voisinId;

                // Remplir la matrice
                matriceAdjacence[sourceIndex, cibleIndex] = 1;
                matriceAdjacence[cibleIndex, sourceIndex] = 1; // Si le graphe est non orienté, sinon commente cette ligne.
            }
        }
    }
    public void AfficherListeNoeuds()
    {
        // Affiche la liste des noeuds avec leur Id et ListeAdjacence
        Console.WriteLine("Liste des Noeuds:");
        foreach (var noeud in NoeudsGraphe)
        {
            Console.WriteLine($"Noeud ID: {noeud.Id}, Voisins: [{string.Join(", ", noeud.ListeAdjacence)}]");
        }
    }

    public void AfficherMatriceAdjacence()
    {
        // Afficher la matrice d'adjacence
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
        // Afficher la taille du graphe
        Console.WriteLine($"\nTaille du graphe: {taille}");
    }

    public void AfficherGraphe()
    {
        // Appel des méthodes définies précédemment
        AfficherListeNoeuds();
        AfficherMatriceAdjacence();
        AfficherTailleGraphe();
    }
    public void AjouterLien(Lien lien)
    {

        if (lien.Source.Id < taille && lien.Cible.Id < taille)
        {
            // Ajout à la matrice d'adjacence
            matriceAdjacence[lien.Source.Id, lien.Cible.Id] = 1;
            matriceAdjacence[lien.Cible.Id, lien.Source.Id] = 1;

            // Ajout à la liste d'adjacence des deux nœuds
            lien.Source.ListeAdjacence.Add(lien.Cible.Id);
            lien.Cible.ListeAdjacence.Add(lien.Source.Id);
        }
        else
        {
            Console.WriteLine("Erreur : l'un des nœuds n'existe pas dans le graphe.");
        }
    }

    //Blanc => neutre
    //Jaune => Découvert
    //Rouge => visité
    public void ParcoursLargeur(int depart)
    {
        Queue<int> file = new Queue<int>();
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
        //initialisé le dictionary avec les noeuds
        List<int> OrdreTerminé = new List<int>();

        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";                      //ici là
        }

        file.Enqueue(depart);
        etatSommets[depart] = "Jaune";

        while (file.Count > 0)
        {
            int sommetActuel = file.Dequeue();
            etatSommets[sommetActuel] = "Rouge";
            OrdreTerminé.Add(sommetActuel);


            //Explore les voisins
            foreach (var noeud in NoeudsGraphe)
            {
                if (noeud.Id == sommetActuel)
                {
                    foreach (var voisin in noeud.ListeAdjacence)
                    {
                        //chéquer si ça marche avec le type de 'voisin'
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

            // Explorer les voisins 
            foreach (var noeud in NoeudsGraphe)
            {
                if (noeud.Id == sommetActuel)
                {
                    foreach (var voisin in noeud.ListeAdjacence)
                    {
                        //chéquer si ça marche avec le type de 'voisin'
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

            // Explore les voisins
            foreach (var voisin in NoeudsGraphe[sommetActuel].ListeAdjacence)
            {
                //chéquer si ça marche avec le type de 'voisin'
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
                Console.WriteLine("Fin du traitement du sommet " + sommetActuel + " (Rouge)");
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

    public bool ContientCycleRécu()
    {
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();

        // Initialise tous les sommets en Blanc 
        foreach (var noeud in NoeudsGraphe)
        {
            etatSommets[noeud.Id] = "Blanc";
        }

        // Vérifie chaque composante du graphe
        foreach (var noeud in NoeudsGraphe)
        {
            if (etatSommets[noeud.Id] == "Blanc" && ContientCycleRécuUtil(noeud.Id, etatSommets, -1))
            {
                return true;
            }
        }
        return false;
    }

    public bool ContientCycleRécuUtil(int sommet, Dictionary<int, string> etatSommets, int parent)
    {
        etatSommets[sommet] = "Jaune";

        foreach (var voisin in NoeudsGraphe[sommet].ListeAdjacence)
        {
            if (etatSommets[voisin] == "Blanc")
            {
                if (ContientCycleRécuUtil(voisin, etatSommets, sommet))
                    return true;
            }
            else if (voisin != parent && etatSommets[voisin] == "Jaune")
            {
                // Cycle détecté si on revient sur un sommet découvert autre que le parent
                Console.WriteLine("Cycle détecté via le sommet " + voisin + " !");
                return true;
            }
        }

        etatSommets[sommet] = "Rouge"; // Marquer comme terminé
        Console.WriteLine("Fin du traitement du sommet " + sommet + " (Rouge)");
        return false;
    }
}
