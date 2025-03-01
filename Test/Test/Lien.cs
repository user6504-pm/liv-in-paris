using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Représente une connexion explicite entre deux noeuds
/// </summary>
public class Lien
{
    /// <summary>
    /// Noeud d'origine de la connexion
    /// </summary>
    public Noeud Source;
    /// <summary>
    /// Noeud de destination de la connexion
    /// </summary>
    public Noeud Cible;
    /// <summary>
    /// Crée une relation explicite entre deux noeuds existants
    /// </summary>
    public Lien(Noeud source, Noeud cible)
    {
        this.Source = source;
        this.Cible = cible;
    }
}
