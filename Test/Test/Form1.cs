﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Test
{
    /// <summary>
    /// Formulaire principal permettant la visualisation interactive d'un graphe
    /// et la construction de celui-ci
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Instance du graphe chargé en mémoire
        /// Contient l'ensemble des noeuds et leurs relations
        /// </summary>
        public Graphe mongraphe;
        /// <summary>
        /// Moteur de rendu graphique chargé de dessiner le graphe
        /// Gère le positionnement des éléments et leur apparence
        /// </summary>
        public GrapheRenderer _renderer;

        public Form1()
        {
            InitializeComponent();

            string chemin = "Noeuds + Liens - associations.txt";
            mongraphe = ChargerGrapheDepuisFichier(chemin);
            mongraphe.AfficherGraphe();
            mongraphe.ParcoursLargeur(1);
            mongraphe.EstConnexe();
            mongraphe.ContientCycle();
    
            _renderer = new GrapheRenderer(mongraphe, panelGraphe.Width, panelGraphe.Height);

            panelGraphe.Paint += PanelGraphe_Paint;
        }

        private void PanelGraphe_Paint(object sender, PaintEventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.Draw(e.Graphics);
            }
        }
        /// <summary>
        /// Charge un graphe à partir d'un fichier texte structuré
        /// Format attendu : "id_source id_cible" par ligne
        /// </summary>
        private Graphe ChargerGrapheDepuisFichier(string cheminFichier)
        {
            Dictionary<int, Noeud> dictionnaireNoeuds = new Dictionary<int, Noeud>();
            List<Noeud> ListeNoeuds = new List<Noeud>();

            foreach (var ligne in File.ReadLines(cheminFichier))
            {
                string[] parties = ligne.Split(' ');
                int sourceId = int.Parse(parties[0]);
                int cibleId = int.Parse(parties[1]);

                if (!dictionnaireNoeuds.ContainsKey(sourceId))
                {
                    dictionnaireNoeuds[sourceId] = new Noeud(sourceId);
                    ListeNoeuds.Add(dictionnaireNoeuds[sourceId]);
                }

                if (!dictionnaireNoeuds.ContainsKey(cibleId))
                {
                    dictionnaireNoeuds[cibleId] = new Noeud(cibleId);
                    ListeNoeuds.Add(dictionnaireNoeuds[cibleId]);
                }

                dictionnaireNoeuds[sourceId].AjouterVoisin(dictionnaireNoeuds[cibleId]);
                dictionnaireNoeuds[cibleId].AjouterVoisin(dictionnaireNoeuds[sourceId]);

            }
            AfficherListe(ListeNoeuds);
            Graphe monGraphe = new Graphe(ListeNoeuds);
            return monGraphe;
        }

        static void AfficherListe(List<Noeud> liste)
        {
            string ligne = "";
            foreach (Noeud item in liste)
            {
                ligne += item.Id + ",";
            }
            Console.WriteLine("Noeuds du Graphe : "+ligne);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Formulaire chargé !");
        }
    }
}
