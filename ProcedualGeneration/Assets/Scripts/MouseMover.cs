using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMover : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float moveVelocity = 3f;

    private float Horizontal = 0.0f;
    private float Vertical = 0.0f;





    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Horizontal += speedH * Input.GetAxis("Mouse X");
        Vertical -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(Vertical, Horizontal, 0.0f);
        Vector3 v = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(v * Time.deltaTime * moveVelocity, Space.Self);
    }
}
