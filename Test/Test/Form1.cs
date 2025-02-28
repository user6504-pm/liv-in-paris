using System;
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
    public partial class Form1 : Form
    {
        public Graphe mongraphe;
        public GrapheRenderer _renderer;

        public Form1()
        {
            InitializeComponent();

            //répondre aux questions (parcours, connexe, cycle ?)
            //+ outils de visualisation d'un graphe (Microsoft System.Drawing)
            string chemin = "Noeuds + Liens - associations.txt";
            mongraphe = ChargerGrapheDepuisFichier(chemin);
            mongraphe.AfficherGraphe();
            mongraphe.ParcoursLargeur(1);
            //mongraphe.EstConnexe(); // <-- à revoir problème de out of range
           // mongraphe.ContientCycleRécu();

    
            _renderer = new GrapheRenderer(mongraphe, panelGraphe.Width, panelGraphe.Height);

            // Activer le redessin
            panelGraphe.Paint += PanelGraphe_Paint;


        }

        private void PanelGraphe_Paint(object sender, PaintEventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.Draw(e.Graphics);
            }
        }

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
            //construire le graphe

            Graphe monGraphe = new Graphe(ListeNoeuds);
            return monGraphe;
        }

        static void AfficherListe(List<Noeud> liste)
        {
            foreach (Noeud item in liste)
            {
                Console.WriteLine(item.Id);
            }
        }

        // Dans Form1.cs
        private void btnParcoursLargeur_Click(object sender, EventArgs e)
        {
            mongraphe.ParcoursLargeur(0); // Démarrer depuis le nœud 0
            panelGraphe.Invalidate();   // Rafraîchir l'affichage
        }

        private void btnParcoursProfondeur_Click(object sender, EventArgs e)
        {
            mongraphe.ParcoursProfondeur(0);
            panelGraphe.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Code exécuté au chargement du formulaire
            MessageBox.Show("Formulaire chargé !");
        }
    }
}
