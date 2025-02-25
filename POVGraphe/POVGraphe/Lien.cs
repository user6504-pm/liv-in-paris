using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Lien
{
    public Noeud Source;
    public Noeud Cible;
    public Lien(Noeud source, Noeud cible)
    {
        Source = source;
        Cible = cible;
    }
}

