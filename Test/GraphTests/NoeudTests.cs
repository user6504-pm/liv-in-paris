using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace AssociationGraphe
{
    [TestClass]
    public sealed class NoeudTests
    {
        [TestMethod]
        public void AjouterVoisin_DoitAjouterIDAListeAdjacence()
        {
            var noeudA = new Noeud(1);
            var noeudB = new Noeud(2);

            noeudA.AjouterVoisin(noeudB);

            Assert.IsTrue(noeudA.ListeAdjacence.Contains(2));
            Assert.AreEqual(1, noeudA.ListeAdjacence.Count);
        }
    }
}

