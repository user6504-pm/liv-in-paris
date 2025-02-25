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
        static void Main(string[] args)
        {
            //lire le fichier soc-karate.mtx
            //instencier dans le graphe (en utilisant les 2 modes)
            //répondre aux questions (parcours, connexe, cycle ?)
            //+ outils de visualisation d'un graphe (Microsoft System.Drawing)


            Graphe monGraphe = new Graphe(6);
            monGraphe.AjouterLien(0, 1);
            monGraphe.AjouterLien(0, 2);
            
            monGraphe.AjouterLien(1, 3);

            monGraphe.ParcoursProfondeur(2);
        }

    }
}
