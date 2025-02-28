using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;

public class GrapheRenderer
{
    public Graphe _graphe;
    public Dictionary<int, SKPoint> _positions = new Dictionary<int, SKPoint>();

    // Configuration visuelle
    public SKColor _nodeColor = SKColors.SteelBlue;
    public SKColor _edgeColor = SKColors.DimGray;
    public float _nodeRadius = 20;
    public int _width;
    public int _height;

    public GrapheRenderer(Graphe graphe, int width, int height)
    {
        _graphe = graphe;
        _width = width;
        _height = height;
        CalculateNodePositions();
    }

    private void CalculateNodePositions()
    {
        // Positionnement circulaire simple
        int centerX = _width / 2;
        int centerY = _height / 2;
        int radius = Math.Min(_width, _height) / 3;

        double angleStep = 2 * Math.PI / _graphe.NoeudsGraphe.Count;
        double currentAngle = 0;

        foreach (var node in _graphe.NoeudsGraphe)
        {
            _positions[node.Id] = new SKPoint(
                centerX + (float)(radius * Math.Cos(currentAngle)),
                centerY + (float)(radius * Math.Sin(currentAngle))
            );
            currentAngle += angleStep;
        }
    }

    public void Draw(SKCanvas canvas)
    {
        // Dessiner les liens d'abord
        var paintEdge = new SKPaint
        {
            Color = _edgeColor,
            StrokeWidth = 2,
            IsAntialias = true
        };

        foreach (var node in _graphe.NoeudsGraphe)
        {
            if (_positions.TryGetValue(node.Id, out var startPos))
            {
                foreach (var neighborId in node.ListeAdjacence)
                {
                    if (_positions.TryGetValue(neighborId, out var endPos))
                    {
                        canvas.DrawLine(startPos, endPos, paintEdge);
                    }
                }
            }
        }

        // Dessiner les noeuds ensuite
        var paintNode = new SKPaint
        {
            Color = _nodeColor,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 16,
            TextAlign = SKTextAlign.Center,
            IsAntialias = true
        };

        foreach (var (id, pos) in _positions)
        {
            // Cercle du noeud
            canvas.DrawCircle(pos, _nodeRadius, paintNode);

            // Texte ID
            var textBounds = new SKRect();
            textPaint.MeasureText(id.ToString(), ref textBounds);
            canvas.DrawText(
                id.ToString(),
                pos.X,
                pos.Y + textBounds.Height / 2,
                textPaint);
        }
    }
}

