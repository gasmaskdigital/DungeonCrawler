using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class testPlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody myRigidBody;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) { myRigidBody.linearVelocity += Vector3.forward * speed * Time.deltaTime; };
        if (Input.GetKey(KeyCode.A)) { myRigidBody.linearVelocity += Vector3.left * speed * Time.deltaTime; };
        if (Input.GetKey(KeyCode.S)) { myRigidBody.linearVelocity += Vector3.back * speed * Time.deltaTime; };
        if (Input.GetKey(KeyCode.D)) { myRigidBody.linearVelocity += Vector3.right * speed * Time.deltaTime; };

        if (Input.GetKey(KeyCode.R)) {
            myRigidBody.transform.position = Vector3.up;
            myRigidBody.linearVelocity = Vector3.zero;
        };

    }
}
