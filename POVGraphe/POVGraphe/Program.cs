using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphe monGraphe = new Graphe(6);
            monGraphe.AjouterLien(0, 1);
            monGraphe.AjouterLien(0, 2);
            monGraphe.AjouterLien(1, 3);

            monGraphe.ParcoursProfondeur(2);
        }

    }
}
