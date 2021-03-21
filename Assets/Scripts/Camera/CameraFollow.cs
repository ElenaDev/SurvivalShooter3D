using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5;//esta variable la vamos a usar para suavizar el movimiento de la cámara

    Vector3 offset;//el offset es la distancia inicial que hay entre cámara y player
    void Start()
    {
        offset = transform.position - target.position;//Vector3 entre la cámara y el personaje
    }
    void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
