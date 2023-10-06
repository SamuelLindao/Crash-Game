using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : Graphic
{
    public Vector2 gridSize;
    public GridRenderer grid;
  //  public LineRenderer render;

    public Color colorOne;
    public Color colorTwo;
    public GameObject bugObject;

    public List<Vector2> points = new List<Vector2>();

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
        
        if(points.Count<2)
        {
            return;
        }

        for (int i = 0; i < points.Count ; i++)
        {
            //DrawBase(vh, 100,100,0,0);
            if (i > 0)
            {
                DrawBase(vh, points[i].x, points[i].y, points[i -1 ].x, points[i - 1].y, i - 1);
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (i > 0)
            {
                DrawTriangles(vh, i - 1);
            }
        }

    }
    void DrawBase(VertexHelper vh, float x, float y, float initX, float initY, int index)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3[] vertices = new Vector3[3];
        if (index == 0)
        {
            vertex.position = new Vector3(initX, initY);
            vh.AddVert(vertex);
        }

        vertex.position = new Vector3(unitWidth * x + 0.3f, 0);
        vh.AddVert(vertex);

       
        vertex.position = new Vector3(unitWidth * x + 0.3f, unitHeight * y);
        vh.AddVert(vertex);



    }
    public void DrawTriangles(VertexHelper vh, int index)
    {

        if (index == 0)
        {
            vh.AddTriangle(0, 1, 2);
        }
        else if (index == 1)
        {
            vh.AddTriangle(index + 0, index + 3, index + 2);
            vh.AddTriangle(index + 0, index + 1, index + 3);

        }
        else
        {
            if (index % 2 == 0)
            {
                vh.AddTriangle(index + 2, index + 4, index + 1);
                vh.AddTriangle(index + 1, index + 3, index + 4);
            }
            else
            {
                vh.AddTriangle(index + 3, index + 5, index + 2);
                vh.AddTriangle(index + 2, index + 4, index + 5);
            }
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

        vertex.position = Quaternion.Euler(45, 45, 90) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);


        vertex.position = new Vector3(thickness / 2, points[0].y);
        vertex.position += new Vector3(unitWidth * point.x, points[0].y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2, points[0].y);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * points[0].y);
        vh.AddVert(vertex);

        lastVertexX = vertex.position.x;
        lastVertexY = vertex.position.y;
        if (i < count - 1)
        {
            VertexX = vertex.position.x;
            VertexY = vertex.position.y;
        }
    }
    private void Update()
    {

        if (grid != null)
        {
            if (gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }
    }
}
