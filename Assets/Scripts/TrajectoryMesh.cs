using System.Collections.Generic;
using UnityEngine;

public class TrajectoryMesh : MonoBehaviour
{
    private int POINT_COUNT = 15;
    private float MAX_RADIUS = 1;
    private float MIN_RADIUS = 0.4f;
    private int NUMBER_HEMISPHERES = 5;
    private float MAX_RADIUS_BALL = 15;
    private float MIN_RADIUS_BALL = 5;
    private List<Vector3> positionBalls = new List<Vector3>();
    List<GameObject> balls = new List<GameObject>();
    public GameObject _containerBalls;
    private int BALLS_COUNT = 15;
    private System.Random rand = new System.Random();
    enum directionArc
    {
        horizontal,
        vertical,
    }
    void Start()
    {
        float stepRadius = (MAX_RADIUS - MIN_RADIUS) / NUMBER_HEMISPHERES;
        for (int i = 0; i < NUMBER_HEMISPHERES; i++)
        {
            List<List<Vector3>> meshPoints = GetMeshPoints(new Vector3(0, 0, 0), MAX_RADIUS - stepRadius * i);
            RenderMesh(meshPoints);
            Debug.Log(positionBalls.Count);
        }
        for (int i = 0; i < BALLS_COUNT; i++)
        {
            AddBall();
        }
    }

    List<List<Vector3>> GetMeshPoints(Vector3 pos, float radius)
    {
        List<List<Vector3>> meshPoints = new List<List<Vector3>>();
        for (int i = 0; i < POINT_COUNT; i++)
        {
            float alpha = Mathf.Deg2Rad * (i * 180f / (POINT_COUNT - 1));
            float offset = radius * Mathf.Cos(alpha);
            float newRadius = radius * Mathf.Sin(alpha);
            List<Vector3> points = GetArcPoint(newRadius, new Vector3(pos.x, pos.y + (radius - newRadius), pos.z + offset), directionArc.horizontal);
            meshPoints.Add(points);
        }
        for (int i = 0; i < POINT_COUNT; i++)
        {
            meshPoints.Add(new List<Vector3>());
            positionBalls.InsertRange(positionBalls.Count, meshPoints[i]);
        }
        for (int i = 0; i < POINT_COUNT; i++)
        {
            for (int j = 0; j < POINT_COUNT; j++)
            {
                meshPoints[POINT_COUNT + i].Add(meshPoints[j][i]);
            }
        }
        return meshPoints;
    }

    private void AddBall()
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        float radius = GetRandomFloat(MIN_RADIUS_BALL, MAX_RADIUS_BALL);
        ball.transform.localScale = new Vector3(radius, radius, radius);
        Debug.Log(positionBalls.Count);
        int index = rand.Next(0, positionBalls.Count);
        Vector3 pos = positionBalls[index];
        positionBalls.RemoveAt(index);
        ball.name = "Ball-" + balls.Count;
        if (balls.Count == 0)
        {
            ball.AddComponent<TargeringBall>();
        }
        ball.transform.position = new Vector3(pos.x * 100, pos.y * 100, pos.z * 100);
        ball.transform.parent = _containerBalls.transform;
        balls.Add(ball);
    }
    private List<Vector3> GetArcPoint(float radius, Vector3 position, directionArc direction)
    {
        List<Vector3> points = new List<Vector3>();
        float alpha = 0;
        if (direction == directionArc.vertical)
        {
            alpha = Mathf.Deg2Rad * 90;
        }
        for (int j = 0; j < POINT_COUNT; j++)
        {
            float beta = Mathf.Deg2Rad * (j * 180f / (POINT_COUNT - 1));
            float x = radius * Mathf.Cos(beta) * Mathf.Cos(alpha) + position.x;
            float z = radius * Mathf.Cos(beta) * Mathf.Sin(alpha) + position.z;
            float y = radius - radius * Mathf.Sin(beta) + position.y;
            Vector3 pos = new Vector3(x, y, z);
            points.Add(pos);
        }
        return points;
    }

    private void RenderMesh(List<List<Vector3>> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            GameObject line = new GameObject();
            line.transform.parent = this.gameObject.transform;
            line.transform.localScale = new Vector3(1, 1, 1);
            line.name = "Arc-" + i.ToString();
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.positionCount = points[i].Count;
            lr.SetPositions(points[i].ToArray());
            lr.startWidth = 0.4f;
            lr.endWidth = 0.4f;
            lr.startColor = new Color(255, 0, 0);
            lr.endColor = new Color(255, 0, 0);
            lr.useWorldSpace = false;
        }
    }

    public float GetRandomFloat(float minimum, float maximum)
    {
        return (float)rand.NextDouble() * (maximum - minimum) + minimum;
    }
}
