using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CamControl : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    [SerializeField] private float camMoveSpeed;
    [SerializeField] private float camRotateSpeed;
    [SerializeField] private float camZoomSpeed;

    [SerializeField] private Vector2 moveBorders;
    [SerializeField] private Vector2 zoomBorders;

    private float mouseX;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))// перемещаем камеру | moving camera
        {
            if(Input.GetAxis("Mouse X") < 0 && cam.transform.localPosition.x < moveBorders.y)
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(cam.transform.localPosition.x - Input.GetAxis("Mouse X") * camMoveSpeed, cam.transform.localPosition.y, cam.transform.localPosition.z), camMoveSpeed * Time.deltaTime);
            }
            if (Input.GetAxis("Mouse X") > 0 && cam.transform.localPosition.x > moveBorders.x)
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(cam.transform.localPosition.x - Input.GetAxis("Mouse X") * camMoveSpeed, cam.transform.localPosition.y, cam.transform.localPosition.z), camMoveSpeed * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))// вращаем камеру | rotate camera
        {
            mouseX = Mathf.Clamp(Input.GetAxis("Mouse X") * camRotateSpeed * Time.deltaTime, -360f, 360f);
            transform.Rotate(new Vector3(0f, mouseX, 0f));
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)// приближаем камеру | zoom camera
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.transform.localPosition.z > zoomBorders.x)
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * camMoveSpeed), camMoveSpeed);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.transform.localPosition.z < zoomBorders.y)
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * camMoveSpeed), camMoveSpeed);
            }
        }
    }
}
