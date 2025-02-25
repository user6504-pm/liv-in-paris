using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Graphe
{
    private Dictionary<int, List<int>> dictionnaireAdjacence;
    private int[,] matriceLiens;
    private int taille;

    public Graphe(int taille)
    {
        this.taille = taille;
        dictionnaireAdjacence = new Dictionary<int, List<int>>();
        matriceLiens = new int[taille, taille];
    }

    public void AjouterLien(int u, int v)
    {
        if (!dictionnaireAdjacence.ContainsKey(u))
        {
            dictionnaireAdjacence[u] = new List<int>();
            dictionnaireAdjacence[u].Add(v);
        }

        if (!dictionnaireAdjacence.ContainsKey(v))
        {
            dictionnaireAdjacence[v] = new List<int>();
            dictionnaireAdjacence[v].Add(u);
        }

        matriceLiens[u, v] = 1;
        matriceLiens[v, u] = 1;
    }

    //Blanc => neutre
    //Jaune => Découvert
    //Rouge => visité
    public void ParcoursLargeur(int depart)
    {
        Queue<int> file = new Queue<int>();
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
        List<int> OrdreTerminé = new List<int>();

        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        file.Enqueue(depart);
        etatSommets[depart] = "Jaune";

        while (file.Count > 0)
        {
            int sommetActuel = file.Dequeue(); 
            etatSommets[sommetActuel] = "Rouge"; 
            OrdreTerminé.Add(sommetActuel);

            Console.WriteLine("Visite du sommet " +sommetActuel+" (Rouge)");

            //Explore les voisins
            foreach (var voisin in dictionnaireAdjacence[sommetActuel])
            {
                if (etatSommets[voisin] == "Blanc")
                {
                    etatSommets[voisin] = "Jaune";
                    file.Enqueue(voisin);
                    Console.WriteLine("Découverte du sommet "+voisin+"(Jaune)");
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

        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        pile.Push(depart);
        etatSommets[depart] = "Jaune";

        while (pile.Count > 0)
        {
            int sommetActuel = pile.Peek();
            bool aVoisinNonVisite = false;

            // Explorer les voisins 
            foreach (var voisin in dictionnaireAdjacence[sommetActuel])
            {
                if (etatSommets[voisin] == "Blanc")
                {
                    etatSommets[voisin] = "Jaune"; 
                    pile.Push(voisin);
                    Console.WriteLine("Découverte du sommet "+voisin+" (Jaune)");
                    aVoisinNonVisite = true;
                    break; 
                }
            }

            if (!aVoisinNonVisite) // Si aucun voisin non visité, on termine ce sommet
            {
                etatSommets[sommetActuel] = "Rouge"; // Marquer comme visité
                OrdreTerminé.Add(sommetActuel);
                Console.WriteLine("Fin du traitement du sommet "+sommetActuel+" (Rouge)");
                pile.Pop();
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

        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        int premierSommet = dictionnaireAdjacence.Keys.First();

        Stack<int> pile = new Stack<int>();
        pile.Push(premierSommet);
        etatSommets[premierSommet] = "Jaune";

        while (pile.Count > 0)
        {
            int sommetActuel = pile.Peek();
            bool aVoisinNonVisite = false;

            // Explore les voisins
            foreach (var voisin in dictionnaireAdjacence[sommetActuel])
            {
                if (etatSommets[voisin] == "Blanc")
                {
                    etatSommets[voisin] = "Jaune"; 
                    pile.Push(voisin);
                }
                Console.WriteLine($"Découverte du sommet {voisin} (Jaune)");
                aVoisinNonVisite = true;
                break; 
            }


            if (!aVoisinNonVisite)
            {
                etatSommets[sommetActuel] = "Rouge"; 
                OrdreTerminé.Add(sommetActuel);
                Console.WriteLine("Fin du traitement du sommet "+sommetActuel+" (Rouge)");
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

    public void ParcoursProfondeurRécu(int depart)
    {
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();
        List<int> OrdreTerminé = new List<int>();

      
        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        Console.WriteLine("Départ de l'exploration en profondeur depuis " +depart);
        ParcoursProfondeurUtil(depart, etatSommets, OrdreTerminé);

        Console.WriteLine("Ordre de marquage terminé (Rouge) : " + string.Join(",", OrdreTerminé));
    }

    private void ParcoursProfondeurUtil(int sommet, Dictionary<int, string> etatSommets, List<int> OrdreTerminé)
    {
        etatSommets[sommet] = "Jaune"; 
        Console.WriteLine("Découverte du sommet " +sommet +"(Jaune)");

        // Explore récursivement les voisins
        foreach (var voisin in dictionnaireAdjacence[sommet])
        {
            if (etatSommets[voisin] == "Blanc")
            {
                ParcoursProfondeurUtil(voisin, etatSommets, OrdreTerminé);
            }
        }

        etatSommets[sommet] = "Rouge";
        OrdreTerminé.Add(sommet);
        Console.WriteLine($"Fin du traitement du sommet {sommet} (Rouge)");
    }

    public bool ContientCycle()
    {
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();

        // Initialise tous les sommets à Blanc 
        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            if (etatSommets[sommet] == "Blanc")
            {
                if (ContientCycleUtil(sommet, -1, etatSommets))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool ContientCycleUtil(int sommet, int parent, Dictionary<int, string> etatSommets)
    {
        etatSommets[sommet] = "Jaune"; // Marquer comme découvert
        Console.WriteLine("Découverte du sommet "+sommet+" (Jaune)");

        foreach (var voisin in dictionnaireAdjacence[sommet])
        {
            if (etatSommets[voisin] == "Blanc")
            {
                if (ContientCycleUtil(voisin, sommet, etatSommets))
                    return true;
            }
            else if (voisin != parent && etatSommets[voisin] == "Jaune")
            {
                // Cycle détecté si on revient sur un sommet déjà découvert autre que le parent
                Console.WriteLine("Cycle détecté via le sommet "+voisin+" !");
                return true;
            }
        }

        etatSommets[sommet] = "Rouge"; // Marquer comme terminé
        Console.WriteLine($"Fin du traitement du sommet {sommet} (Rouge)");
        return false;
    }

    public bool ContientCycleRécu()
    {
        Dictionary<int, string> etatSommets = new Dictionary<int, string>();

        // Initialise tous les sommets en Blanc 
        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            etatSommets[sommet] = "Blanc";
        }

        // Vérifie chaque composante du graphe
        foreach (var sommet in dictionnaireAdjacence.Keys)
        {
            if (etatSommets[sommet] == "Blanc")
            {
                if (ContientCycleRécuUtil(sommet, -1, etatSommets))
                    return true;
            }
        }
        return false;
    }

    private bool ContientCycleRécuUtil(int sommet, int parent, Dictionary<int, string> etatSommets)
    {
        etatSommets[sommet] = "Jaune"; 
        Console.WriteLine("Découverte du sommet "+sommet+" (Jaune)");

        foreach (var voisin in dictionnaireAdjacence[sommet])
        {
            if (etatSommets[voisin] == "Blanc")
            {
                if (ContientCycleRécuUtil(voisin, sommet, etatSommets))
                    return true;
            }
            else if (voisin != parent && etatSommets[voisin] == "Jaune")
            {
                // Cycle détecté si on revient sur un sommet découvert autre que le parent
                Console.WriteLine("Cycle détecté via le sommet "+voisin+" !");
                return true;
            }
        }

        etatSommets[sommet] = "Rouge"; // Marquer comme terminé
        Console.WriteLine("Fin du traitement du sommet "+sommet+" (Rouge)");
        return false;
    }

}


