using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    private float speed = 10f;

    private float lookSpeed = 5f;

    private float jumpForce = 7f;

    private PlayerMotor motor;

    //[SerializeField]
    private SphereCollider col;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private MyGun gun;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        col = GetComponent<SphereCollider>();
    }

    void Update () {
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");
        
        Vector3 moveHor = transform.right * xMov;
        Vector3 moveVer = transform.forward * zMov;

        Vector3 velosity = (moveHor + moveVer).normalized * speed;
        
		motor.Move(velosity);

        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSpeed;
        motor.Rotate(rotation);

        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 camRotation = new Vector3(xRot, 0f, 0f) * lookSpeed;
        motor.RotateCam(camRotation);
        
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            motor.Jump(Vector3.up * jumpForce);
        }

        if (Input.GetKey(KeyCode.Mouse0)) {
            if (gun != null) {
			    gun.Shoot();
            }
		}
    }

    private bool isGrounded () {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, 
        col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }
}