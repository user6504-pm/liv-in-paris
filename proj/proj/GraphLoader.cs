using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj {
    public class GraphLoader {
        public Dictionary<int, List<int>> ListeAdjacence { get; private set; }
        public int[,] MatriceAdjacence { get; private set; }
        public int NombreNoeuds { get; private set; }

        public GraphLoader(string filePath) {
            ListeAdjacence = new Dictionary<int, List<int>>();
            ChargerGraph(filePath);
        }

        private void ChargerGraph(string filePath) {
            var edges = new List<(int, int)>();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            bool isFirstLine = true;

            foreach (string line in lines) {
                if (line.StartsWith("%")) continue; // Ignorer les commentaires
                string[] parts = line.Split();

                if (isFirstLine) {
                    NombreNoeuds = int.Parse(parts[0]);
                    MatriceAdjacence = new int[NombreNoeuds + 1, NombreNoeuds + 1];
                    isFirstLine = false;
                    continue;
                }

                if (parts.Length == 2) {
                    int node1 = int.Parse(parts[0]);
                    int node2 = int.Parse(parts[1]);
                    if (node1 != node2) {
                        edges.Add((node1, node2));
                    }
                }
            }

            // Construire la liste d'adjacence
            foreach (var (node1, node2) in edges) {
                if (!ListeAdjacence.ContainsKey(node1))
                    ListeAdjacence[node1] = new List<int>();
                if (!ListeAdjacence.ContainsKey(node2))
                    ListeAdjacence[node2] = new List<int>();

                ListeAdjacence[node1].Add(node2);
                ListeAdjacence[node2].Add(node1); // Graphe non orienté

                // Construire la matrice d'adjacence
                MatriceAdjacence[node1, node2] = 1;
                MatriceAdjacence[node2, node1] = 1;
            }
        }

        public void AfficherListeAdjacence() {
            Console.WriteLine("Liste d'adjacence :");
            foreach (var noeud in ListeAdjacence) {
                Console.Write(noeud.Key + " -> ");
                Console.WriteLine(string.Join(", ", noeud.Value));
            }
        }

        public void AfficherMatriceAdjacence() {
            Console.WriteLine("Matrice d'adjacence :");
            for (int i = 1; i <= NombreNoeuds; i++) {
                for (int j = 1; j <= NombreNoeuds; j++) {
                    Console.Write(MatriceAdjacence[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
