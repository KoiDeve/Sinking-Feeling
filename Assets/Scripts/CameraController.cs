using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject followTarget;
    private Vector3 fTposition;
    public float cameraMoveSpeed = 3.5f;

    public float zoomMax, zoomMin, zoomDivisor, zoomCut, zoomTime;
    private Camera theCamera;

    public float threshold_min;

    public BoxCollider2D bounds;
    public Vector3 minBounds, maxBounds;
    public float halfWidth, halfHeight;

    private bool zooming = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = followTarget.transform.position;
        theCamera = GetComponent<Camera>();
        SetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        fTposition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);

        //Debug.LogError("This section of the code does not work. Needs fixing.");
        //transform.position = Vector3.Lerp(fTposition, fTposition, cameraMoveSpeed * Time.deltaTime);
        //transform.position = fTposition;
        //Debug.LogError("This section of the code does not work. Needs fixing.");

        transform.position = Vector3.Lerp(transform.position, fTposition, cameraMoveSpeed * Time.deltaTime);

        if (bounds == null)
        {
            //throw new Exception("There isn't a bounds box available in the scene!");
        }
        else {
            SetBounds();
            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
            transform.position = new Vector3(clampedX, clampedY, -10);
        }

        
            /*if (transform.position.y < threshold_min)
            {
                Debug.Log("Zooming in");
                ZoomIn();
            }
            else {
                Debug.Log("Zooming out");
                ZoomOut();
            }
        */
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        StopAllCoroutines();
            Debug.LogWarning("issure");
            ZoomOut();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        Debug.LogError("issure");
        ZoomIn();
    }*/

    public void ZoomIn() {
        StopAllCoroutines();
        Debug.Log("zoomINNNN");
        zooming = true;
        if (theCamera.orthographicSize > zoomMin) {
            StopCoroutine(ZoomingOut());
            StartCoroutine(ZoomingIn());
        }
    }

    public void ZoomOut() {
        StopAllCoroutines();
        Debug.Log("zzzommmm OUTTTT");
        zooming = true;
        if (theCamera.orthographicSize < zoomMax) {
            StopCoroutine(ZoomingIn());
            StartCoroutine(ZoomingOut());

        }
    }

    IEnumerator ZoomingIn()
    {

            for (int i = 0; i < zoomDivisor*2; i++)
            {
                if (theCamera.orthographicSize <= zoomMin)
                {
                    theCamera.orthographicSize = zoomMin;
                    zooming = false;
                }
                else {
                    theCamera.orthographicSize -= (zoomMax - zoomMin) / zoomDivisor;
                }
                yield return new WaitForSeconds(zoomTime);
            }
        }

    IEnumerator ZoomingOut() {

    for (int i = 0; i < zoomDivisor*2; i++)
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
