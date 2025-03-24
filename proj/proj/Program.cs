using proj;
using System;
using System.Collections.Generic;

namespace Projet {
    class Program {
        static void Main() {
            string filePath = "soc-karate.mtx"; // Modifier avec le chemin correct
            Graph<string> graph = new Graph<string>();
            GraphLoader graphLoader = new GraphLoader(filePath);

            // Dictionnaire pour stocker les nœuds par ID
            Dictionary<int, Noeud<string>> noeudDict = new Dictionary<int, Noeud<string>>();

            // Ajouter les nœuds en premier
            foreach (var idNoeud in graphLoader.ListeAdjacence.Keys) {
                var noeud = new Noeud<string>(idNoeud, $"Noeud{idNoeud}");
                graph.AjouterNoeud(noeud);
                noeudDict[idNoeud] = noeud;
            }

            // Ajouter les liens après
            foreach (var noeud in graphLoader.ListeAdjacence) {
                foreach (var voisin in noeud.Value) {
                    if (noeud.Key < voisin) // Éviter les doublons dans un graphe non orienté
                    {
                        graph.AjouterLien(noeudDict[noeud.Key], noeudDict[voisin], 1);
                    }
                }
            }

            // Affichage du graphe
            Console.WriteLine("=== AFFICHAGE DU GRAPHE ===");
            graph.AfficherGraphe();
            Console.WriteLine();

            // Parcours en Largeur (BFS)
            Console.WriteLine("=== PARCOURS EN LARGEUR (BFS) ===");
            graph.ParcoursLargeur(noeudDict[1]); // Assurez-vous que 1 existe dans le fichier
            Console.WriteLine();

            // Parcours en Profondeur (DFS)
            Console.WriteLine("=== PARCOURS EN PROFONDEUR (DFS) ===");
            graph.ParcoursProfondeur(noeudDict[1]); // Assurez-vous que 1 existe dans le fichier
            Console.WriteLine();

            // Vérifier si le graphe est connexe
            Console.WriteLine("=== TEST DE CONNEXITÉ ===");
            Console.WriteLine(graph.EstConnexe() ? "Le graphe est connexe." : "Le graphe n'est pas connexe.");
            Console.WriteLine();

            // Vérifier la présence de circuits
            Console.WriteLine("=== TEST DE PRÉSENCE DE CIRCUITS ===");
            Console.WriteLine(graph.ADesCircuits() ? "Le graphe contient des circuits." : "Le graphe ne contient pas de circuits.");
            Console.WriteLine();

            // Afficher les caractéristiques du graphe
            Console.WriteLine("=== CARACTÉRISTIQUES DU GRAPHE ===");
            graph.ObtenirCaracteristiques();
            Console.WriteLine();

            // Générer une représentation graphique du graphe
            Console.WriteLine("=== AFFICHAGE GRAPHIQUE DU GRAPHE ===");
            graph.AfficherGraphique("graphe.png");
            Console.WriteLine("Ouvrez 'graphe.png' pour voir l'affichage graphique.");
        }
    }
}
