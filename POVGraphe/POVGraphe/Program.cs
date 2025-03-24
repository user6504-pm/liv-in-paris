using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Program
{
    class Program
    {
        static void Main(string[] args){
            //répondre aux questions (parcours, connexe, cycle ?)
            //+ outils de visualisation d'un graphe (Microsoft System.Drawing)
            string chemin = "Noeuds + Liens - associations.txt";
            Graphe mongraphe = ChargerGrapheDepuisFichier(chemin);
            mongraphe.AfficherGraphe();
            // mongraphe.ParcoursLargeur(1);
            mongraphe.ParcoursProfondeur(1);
            //    mongraphe.EstConnexe(); // <-- à revoir problème de out of range
            //mongraphe.ContientCycleRécu();
        }
        static Graphe ChargerGrapheDepuisFichier(string cheminFichier) {
            Dictionary<int,Noeud> dictionnaireNoeuds = new Dictionary<int, Noeud>();
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

        static void AfficherListe(List<Noeud> liste){
            foreach (Noeud item in liste)
                Console.WriteLine(item.Id);
        }

    }
}
