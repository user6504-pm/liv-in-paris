﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace proj {
    public class Graph<T> {
        private List<Noeud<T>> noeuds;
        private List<Lien<T>> liens;

        public Graph() {
            noeuds = new List<Noeud<T>>();
            liens = new List<Lien<T>>();
        }

        public void AjouterNoeud(Noeud<T> noeud) {
            noeuds.Add(noeud);
        }

        public void AjouterLien(Noeud<T> noeud1, Noeud<T> noeud2, int poids) {
            if (noeuds.Contains(noeud1) && noeuds.Contains(noeud2)) {
                liens.Add(new Lien<T>(noeud1, noeud2, poids));
            }
            else {
                throw new Exception("Les nœuds doivent être présents dans le graphe avant d'ajouter un lien.");
            }
        }

        public void AfficherGraphe() {
            Console.WriteLine("Noeuds du graphe:");
            foreach (var noeud in noeuds) {
                Console.WriteLine(noeud);
            }

            Console.WriteLine("\nLiens du graphe:");
            foreach (var lien in liens) {
                Console.WriteLine(lien);
            }
        }

        public void ParcoursLargeur(Noeud<T> depart) {
            if (!noeuds.Contains(depart)) {
                Console.WriteLine("Le nœud de départ n'existe pas dans le graphe.");
                return;
            }

            HashSet<Noeud<T>> visite = new HashSet<Noeud<T>>();
            Queue<Noeud<T>> file = new Queue<Noeud<T>>();

            file.Enqueue(depart);
            visite.Add(depart);

            Console.WriteLine("Parcours en Largeur (BFS) :");

            while (file.Count > 0) {
                var noeudActuel = file.Dequeue();
                Console.Write(noeudActuel.Donnees + " ");

                foreach (var lien in liens) {
                    if (lien.Noeud1.Equals(noeudActuel) && !visite.Contains(lien.Noeud2)) {
                        file.Enqueue(lien.Noeud2);
                        visite.Add(lien.Noeud2);
                    }
                    else if (lien.Noeud2.Equals(noeudActuel) && !visite.Contains(lien.Noeud1)) {
                        file.Enqueue(lien.Noeud1);
                        visite.Add(lien.Noeud1);
                    }
                }
            }
            Console.WriteLine();
        }

        public void ParcoursProfondeur(Noeud<T> depart) {
            if (!noeuds.Contains(depart)) {
                Console.WriteLine("Le nœud de départ n'existe pas dans le graphe.");
                return;
            }

            HashSet<Noeud<T>> visite = new HashSet<Noeud<T>>();
            Stack<Noeud<T>> pile = new Stack<Noeud<T>>();

            pile.Push(depart);
            visite.Add(depart);

            Console.WriteLine("Parcours en Profondeur (DFS) :");

            while (pile.Count > 0) {
                var noeudActuel = pile.Pop();
                Console.Write(noeudActuel.Donnees + " ");

                foreach (var lien in liens) {
                    if (lien.Noeud1.Equals(noeudActuel) && !visite.Contains(lien.Noeud2)) {
                        pile.Push(lien.Noeud2);
                        visite.Add(lien.Noeud2);
                    }
                    else if (lien.Noeud2.Equals(noeudActuel) && !visite.Contains(lien.Noeud1)) {
                        pile.Push(lien.Noeud1);
                        visite.Add(lien.Noeud1);
                    }
                }
            }
            Console.WriteLine();
        }

        public bool EstConnexe() {
            if (noeuds.Count == 0) return true;

            var visite = new HashSet<Noeud<T>>();
            var pile = new Stack<Noeud<T>>();

            var premier = noeuds[0];
            pile.Push(premier);
            visite.Add(premier);

            while (pile.Count > 0) {
                var noeud = pile.Pop();

                foreach (var lien in liens) {
                    Noeud<T> voisin = null;
                    if (lien.Noeud1.Equals(noeud)) voisin = lien.Noeud2;
                    else if (lien.Noeud2.Equals(noeud)) voisin = lien.Noeud1;

                    if (voisin != null && !visite.Contains(voisin)) {
                        pile.Push(voisin);
                        visite.Add(voisin);
                    }
                }
            }

            return visite.Count == noeuds.Count;
        }

        public bool ADesCircuits() {
            var visite = new HashSet<Noeud<T>>();

            foreach (var noeud in noeuds) {
                if (!visite.Contains(noeud)) {
                    if (DetecterCycleDFS(noeud, null, visite))
                        return true;
                }
            }
            return false;
        }

        private bool DetecterCycleDFS(Noeud<T> noeud, Noeud<T> parent, HashSet<Noeud<T>> visite) {
            visite.Add(noeud);

            foreach (var lien in liens) {
                Noeud<T> voisin = null;
                if (lien.Noeud1.Equals(noeud)) voisin = lien.Noeud2;
                else if (lien.Noeud2.Equals(noeud)) voisin = lien.Noeud1;

                if (voisin != null) {
                    if (!visite.Contains(voisin)) {
                        if (DetecterCycleDFS(voisin, noeud, visite))
                            return true;
                    }
                    else if (!voisin.Equals(parent)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ObtenirCaracteristiques() {
            int nombreNoeuds = noeuds.Count;
            int nombreLiens = liens.Count;

            Console.WriteLine("===== Caractéristiques du Graphe =====");
            Console.WriteLine($"Nombre de nœuds : {nombreNoeuds}");
            Console.WriteLine($"Nombre de liens : {nombreLiens}");

            double densite = (nombreNoeuds > 1) ? (2.0 * nombreLiens) / (nombreNoeuds * (nombreNoeuds - 1)) : 0;
            Console.WriteLine($"Densité du graphe : {densite:F4}");
            Console.WriteLine($"Le graphe est connexe ? : {(EstConnexe() ? "Oui" : "Non")}");
            Console.WriteLine($"Le graphe contient des circuits ? : {(ADesCircuits() ? "Oui" : "Non")}");

            var degres = noeuds.ToDictionary(n => n, n => 0);
            foreach (var lien in liens) {
                degres[lien.Noeud1]++;
                degres[lien.Noeud2]++;
            }

            int degreMin = degres.Values.Min();
            int degreMax = degres.Values.Max();
            double degreMoyen = degres.Values.Average();

            Console.WriteLine($"Degré minimum : {degreMin}");
            Console.WriteLine($"Degré maximum : {degreMax}");
            Console.WriteLine($"Degré moyen : {degreMoyen:F2}");
        }

        public void AfficherGraphique(string cheminFichier = "graph.png", int tailleImage = 600) {
            if (noeuds.Count == 0) {
                Console.WriteLine("Le graphe est vide, impossible de l'afficher.");
                return;
            }

            int rayon = tailleImage / 3;
            int centreX = tailleImage / 2;
            int centreY = tailleImage / 2;
            Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();

            using (var surface = SKSurface.Create(new SKImageInfo(tailleImage, tailleImage))) {
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                using (var paintNoeud = new SKPaint { Color = SKColors.Blue, Style = SKPaintStyle.Fill })
                using (var paintLien = new SKPaint { Color = SKColors.Gray, StrokeWidth = 2 })
                using (var paintTexte = new SKPaint { Color = SKColors.Black, TextSize = 18, IsAntialias = true }) {

                    double angleStep = 2 * Math.PI / noeuds.Count;
                    for (int i = 0; i < noeuds.Count; i++) {
                        double angle = i * angleStep;
                        int x = centreX + (int)(rayon * Math.Cos(angle));
                        int y = centreY + (int)(rayon * Math.Sin(angle));
                        positions[noeuds[i]] = new SKPoint(x, y);
                    }

                    foreach (var lien in liens) {
                        var p1 = positions[lien.Noeud1];
                        var p2 = positions[lien.Noeud2];
                        canvas.DrawLine(p1, p2, paintLien);
                    }

                    foreach (var noeud in noeuds) {
                        var pos = positions[noeud];
                        canvas.DrawCircle(pos, 15, paintNoeud);
                        canvas.DrawText(noeud.Donnees.ToString(), pos.X - 10, pos.Y - 20, paintTexte);
                    }
                }

                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(cheminFichier)) {
                    data.SaveTo(stream);
                }
            }

            Console.WriteLine($"Graphique enregistré sous : {cheminFichier}");
        }
    }
}
