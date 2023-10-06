using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRenderer : Graphic
{
    public Vector2 gridSize;
    public GridRenderer grid;
    public Fade fade;

    public Color colorOne;
    public Color colorTwo;
    public GameObject bugObject;

    public List<Vector2> points;

    public float resolution = 2;
    public float lastVertexX;
    public float lastVertexY;

    public float VertexX;
    public float VertexY;

    float width;
    float height;
    float unitWidth;
    float unitHeight;

    public float thickness = 10f;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

        if(points.Count < 2)
        {
            return;
        }

        float angle = 0;
        

        for(int i =0; i < points.Count;i++)
        {
            Vector2 point = points[i];
            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 45f;
            }
            DrawVerticesForPoint(point, vh, angle, i, points.Count);

        }

        for (int i =0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
     
    }

    public float GetAngle(Vector2 me, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI));
    }
    void DrawVerticesForPoint(Vector2 point, VertexHelper vh, float angle, float i, float count)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = Color.Lerp(colorOne, colorTwo, (float)(i / count));

        vertex.position = Quaternion.Euler(0,0, 70 ) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        lastVertexX = vertex.position.x;
        lastVertexY = vertex.position.y;
        if (i < count - 1)
        {
            VertexX = vertex.position.x;
            VertexY = vertex.position.y;
        }
        //fade.points = points;
    }

    private void Update()
    {
        if(grid != null)
        {
            if(gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }

    }
}
