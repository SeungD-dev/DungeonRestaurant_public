using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public Camera _camera;
    public Canvas canvas;

    private void Awake()
    {
        _camera = Camera.main;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = _camera;
    }

}
