﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Classe spécialisée dans le rendu visuel d'un graphe sur une surface graphique
/// </summary>
public class GrapheRenderer
{
    private Graphe _graphe;
    /// <summary>
    /// Dictionnaire associant chaque ID de noeud à ses coordonnées d'affichage
    /// </summary>
    private Dictionary<int, Point> _positions = new Dictionary<int, Point>();
    /// <summary>
    /// Rayon visuel des noeuds en pixels
    /// </summary>
    private int _nodeRadius = 10;
    /// <summary>
    /// Configuration typographique pour l'affichage des ID de noeuds
    /// </summary>
    private Font _nodeFont = new Font("Arial", 8,FontStyle.Bold); 
    private Brush _textBrush = Brushes.White;
    public GrapheRenderer(Graphe graphe, int width, int height)
    {
        _graphe = graphe;
        CalculateNodePositions(width, height);
    }
    /// <summary>
    /// Calcule un positionnement circulaire équilibré pour les noeuds
    /// </summary>
    private void CalculateNodePositions(int width, int height)
    {
        int margin = 70;
        int centerX = width / 2;
        int centerY = height / 2;
        int radius = (Math.Min(width, height)- margin) / 2;

        double angleStep = 2 * Math.PI / _graphe.NoeudsGraphe.Count;
        double currentAngle = 0;

        foreach (var noeud in _graphe.NoeudsGraphe)
        {
            _positions[noeud.Id] = new Point(
                centerX + (int)(radius * Math.Cos(currentAngle)),
                centerY + (int)(radius * Math.Sin(currentAngle))
            );
            currentAngle += angleStep;
        }
    }

    public void Draw(Graphics g)
    {
        using (var pen = new Pen(Color.DarkGray, 1))
        {
            foreach (var noeud in _graphe.NoeudsGraphe)
            {
                if (_positions.TryGetValue(noeud.Id, out var startPos))
                {
                    foreach (var voisinId in noeud.ListeAdjacence)
                    {
                        if (_positions.TryGetValue(voisinId, out var endPos))
                        {
                            g.DrawLine(pen, startPos, endPos);
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<int, Point> kvp in _positions)
        {
            int id = kvp.Key;
            Point pos = kvp.Value;

            g.FillEllipse(Brushes.LightSteelBlue, pos.X - _nodeRadius, pos.Y - _nodeRadius, 2 * _nodeRadius, 2 * _nodeRadius);
            SizeF textSize = g.MeasureString(id.ToString(), _nodeFont);
            StringFormat format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            g.DrawString(id.ToString(),
                _nodeFont,
                Brushes.DarkSlateBlue,
                new RectangleF(pos.X - _nodeRadius, pos.Y - _nodeRadius, 2 * _nodeRadius, 2 * _nodeRadius),
                format);
        }
    }
}
