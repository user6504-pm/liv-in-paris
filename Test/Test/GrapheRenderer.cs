using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GrapheRenderer
{
    private Graphe _graphe;
    private Dictionary<int, Point> _positions = new Dictionary<int, Point>();
    private int _nodeRadius = 10;
    private Font _nodeFont = new Font("Arial", 8,FontStyle.Bold); // Police plus petite
    private Brush _textBrush = Brushes.White;
    public GrapheRenderer(Graphe graphe, int width, int height)
    {
        _graphe = graphe;
        CalculateNodePositions(width, height);
    }

    private void CalculateNodePositions(int width, int height)
    {
        // Positionnement circulaire
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
        // Dessiner les liens
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

        // Dessiner les nœuds (partie corrigée)
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
