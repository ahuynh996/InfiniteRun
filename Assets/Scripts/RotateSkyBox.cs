using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    public float rotateSpeed = 1.5f; 

    void Update () 
    {
        RenderSettings.skybox.SetFloat ("_Rotation", Time.time * rotateSpeed);
    }
}
