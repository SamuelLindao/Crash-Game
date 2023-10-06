using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BugScript : MonoBehaviour
{
    public LineRenderer render;
    public GameObject coin;
    public GameController controller;
    void Update()
    {
        if (controller.GameStarted)
        {
            Vector2 newPos =  new Vector3(render.lastVertexX, render.lastVertexY) ;
            transform.DOLocalMove(newPos, 1f + Time.deltaTime);
            Vector2 direction = new Vector2(render.lastVertexX, render.lastVertexY) - new Vector2(render.VertexX, render.VertexY);

            float angleInRadians = Mathf.Atan2(direction.y, direction.x);

            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            angleInDegrees = (angleInDegrees + 360) % 360;
            transform.rotation = Quaternion.Euler(0, 0, angleInDegrees - 25);
            coin.transform.localScale = new Vector3(Mathf.Clamp(3f * ((float)transform.localPosition.y / 100f), 1, 3), Mathf.Clamp(3f * ((float)transform.localPosition.y / 100f), 1, 3));
        }
        else
        {
            transform.localPosition = new Vector2(0, 0);

        }
    }
}
