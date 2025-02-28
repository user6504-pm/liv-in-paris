using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Noeud
{
    public int Id { get; set; }
    public List<int> ListeAdjacence { get; set; }

    public Noeud(int id)
    {
        Id = id;
        ListeAdjacence = new List<int>();
    }

    public void AjouterVoisin(Noeud voisin)
    {
        if (!ListeAdjacence.Contains(voisin.Id))
        {
            ListeAdjacence.Add(voisin.Id);
        }
    }
}

