using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to change the position of the camera in relation to the player of the game.
public class CameraController : MonoBehaviour
{

    // Target to follow, as well as the speed and position in relation to the map
    public GameObject followTarget;
    private Vector3 fTposition;
    public float cameraMoveSpeed = 3.5f;

    // Zoom elements for aesthetic, as well as the camera component
    public float zoomMax, zoomMin, zoomDivisor, zoomCut, zoomTime;
    private Camera theCamera;
    public float threshold_min;

    // Bounds used to ensure the camera does not go out of the bounds of the map
    public BoxCollider2D bounds;
    public Vector3 minBounds, maxBounds;
    public float halfWidth, halfHeight;

    private bool zooming = false;

    // Finds a target to follow, and sets the bounds to where the camera can/cannot move.
    void Start()
    {
        transform.position = followTarget.transform.position;
        theCamera = GetComponent<Camera>();
        SetBounds();
    }

    // Update is called once per frame
    void Update()
    {

        // Moves the camera component towards the player using deltaTime

        fTposition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);

        transform.position = Vector3.Lerp(transform.position, fTposition, cameraMoveSpeed * Time.deltaTime);

        if (bounds == null)
        {
            //throw new Exception("There isn't a bounds box available in the scene!");
        }
        else
        {
            SetBounds();
            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
            transform.position = new Vector3(clampedX, clampedY, -10);
        }

        // Used to zoom in the camera when the player goes underwater.
        public void ZoomIn()
        {
            StopAllCoroutines();
            zooming = true;
            if (theCamera.orthographicSize > zoomMin)
            {
                StopCoroutine(ZoomingOut());
                StartCoroutine(ZoomingIn());
            }
        }

        // Used to zoom out the camera when the player goes above the water.
        public void ZoomOut()
        {
            StopAllCoroutines();
            zooming = true;
            if (theCamera.orthographicSize < zoomMax)
            {
                StopCoroutine(ZoomingIn());
                StartCoroutine(ZoomingOut());
            }
        }

        // A coroutine used for the timing of zooming in.
        IEnumerator ZoomingIn()
        {
            for (int i = 0; i < zoomDivisor * 2; i++)
            {
                if (theCamera.orthographicSize <= zoomMin)
                {
                    theCamera.orthographicSize = zoomMin;
                    zooming = false;
                }
                else
                {
                    theCamera.orthographicSize -= (zoomMax - zoomMin) / zoomDivisor;
                }
                yield return new WaitForSeconds(zoomTime);
            }
        }

        IEnumerator ZoomingOut()
        {
            for (int i = 0; i < zoomDivisor * 2; i++)
            {
                if (theCamera.orthographicSize >= zoomMax)
                {
                    theCamera.orthographicSize = zoomMax;
                    zooming = false;
                }
                else
                {
                    theCamera.orthographicSize += (zoomMax - zoomMin) / zoomDivisor;
                }
                yield return new WaitForSeconds(zoomTime);
            }
        }

        // The method that sets the bounds as to how far the camera can travel
        public void SetBounds()
        {
            minBounds = bounds.bounds.min;
            maxBounds = bounds.bounds.max;
            if (bounds != null)
            {
                halfHeight = theCamera.orthographicSize;
                halfWidth = halfHeight * Screen.width / Screen.height;
            }
        }
    }