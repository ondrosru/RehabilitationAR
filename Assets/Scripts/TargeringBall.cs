using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargeringBall : MonoBehaviour
{
    public float angleBetween = 0.0f;
    public Transform target;

    void Start()
    {
        target = gameObject.transform;
    }
    void Update()
    {
        Vector3 targetDir = target.position - Camera.main.transform.position;
        angleBetween = Vector3.Angle(transform.forward, targetDir);
        Debug.Log(angleBetween.ToString());
    }
}
