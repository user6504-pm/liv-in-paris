using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Test;

namespace AssociationGraphe
{
    [TestClass]
    public class GrapheTests
    {
        [TestMethod]
        public void ConstructeurDeuxNoeudsConnectesMatriceAdjacenceCorrecte()
        {
            var noeud1 = new Noeud(1);
            var noeud2 = new Noeud(2);
            noeud1.AjouterVoisin(noeud2);
            noeud2.AjouterVoisin(noeud1);
            var noeuds = new List<Noeud> { noeud1, noeud2 };

            var graphe = new Graphe(noeuds);

            Assert.AreEqual(1, graphe.matriceAdjacence[1, 2], "Liaison 1->2 manquante");
            Assert.AreEqual(1, graphe.matriceAdjacence[2, 1], "Liaison 2->1 manquante (graphe non orienté)");
            Assert.AreEqual(0, graphe.matriceAdjacence[1, 1], "Boucle non autorisée");
        }
        public void ParcoursLargeurRetourneOrdreCorrect()
        {
            var noeud0 = new Noeud(0);
            var noeud1 = new Noeud(1);
            var noeud2 = new Noeud(2);
            noeud0.AjouterVoisin(noeud1);
            noeud1.AjouterVoisin(noeud0);
            noeud1.AjouterVoisin(noeud2);
            noeud2.AjouterVoisin(noeud1);
            var graphe = new Graphe(new List<Noeud> { noeud0, noeud1, noeud2 });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 
                graphe.ParcoursLargeur(0);
                var result = sw.ToString().Trim();

                StringAssert.Contains(result, "0,1,2", "Ordre BFS incorrect");
            }
        }
        public void EstConnexeDeuxComposantsRetourneFalse()
        {
            var noeud0 = new Noeud(0);
            var noeud1 = new Noeud(1);
            var noeud2 = new Noeud(2);
            var noeud3 = new Noeud(3);
            noeud0.AjouterVoisin(noeud1);
            noeud1.AjouterVoisin(noeud0);
            noeud2.AjouterVoisin(noeud3);
            noeud3.AjouterVoisin(noeud2);
            var graphe = new Graphe(new List<Noeud> { noeud0, noeud1, noeud2, noeud3 });
        
            bool result = graphe.EstConnexe();
        
            Assert.IsFalse(result, "Le graphe ne devrait pas être connexe");
        }
    }
}
