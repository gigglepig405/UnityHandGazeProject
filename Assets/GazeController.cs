using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    public Transform gazeIndicator;
    public Camera mainCamera;

    private bool wasHitLastFrame = false;

    void Start()
    {

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            Debug.Log("Main Camera found: " + mainCamera.name);
        }
        else
        {
            Debug.LogError("Main Camera not found! Please assign a camera to the mainCamera field.");
        }


        if (gazeIndicator != null)
        {
            gazeIndicator.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Gaze Indicator is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (mainCamera == null || gazeIndicator == null)
        {
            Debug.LogWarning("Main Camera or Gaze Indicator is missing. Skipping gaze detection.");
            return;
        }


        Debug.Log("Camera Forward Direction: " + mainCamera.transform.forward);


        Ray gazeRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(gazeRay, out hit))
        {

            Debug.Log("Raycast hit: " + hit.collider.name);
            gazeIndicator.position = hit.point;
            gazeIndicator.gameObject.SetActive(true);
            wasHitLastFrame = true;
        }
        else
        {

            if (wasHitLastFrame)
            {
                Debug.Log("Raycast did not hit anything.");
                wasHitLastFrame = false;
            }
            gazeIndicator.gameObject.SetActive(false);
        }
    }
}


