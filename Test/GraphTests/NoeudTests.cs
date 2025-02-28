using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test; // Remplace par le namespace de ton projet

    [TestClass]
public sealed class NoeudTests
{
    [TestMethod]
    public void AjouterVoisin_DoitAjouterIDAListeAdjacence()
    {
        // Arrange (Préparation)
        var noeudA = new Noeud(1);
        var noeudB = new Noeud(2);

        // Act (Action)
        noeudA.AjouterVoisin(noeudB);

        // Assert (Vérification)
        Assert.IsTrue(noeudA.ListeAdjacence.Contains(2));
        Assert.AreEqual(1, noeudA.ListeAdjacence.Count);
    }
}

