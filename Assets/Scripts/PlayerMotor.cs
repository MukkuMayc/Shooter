using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Rigidbody rb;

	private Vector3 velosity = Vector3.zero;

	private Vector3 rotation = Vector3.zero;

	private Vector3 camRotation = Vector3.zero;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	public void Move (Vector3 _velosity) {
		velosity = _velosity;
	}

	public void Rotate (Vector3 _rotation) {
		rotation = _rotation;
	}

	public void RotateCam (Vector3 _camRotation) {
		camRotation = _camRotation;
	}

	public void Jump (Vector3 _jumpVector) {
		rb.AddForce(_jumpVector, ForceMode.Impulse);
	}
	
	void FixedUpdate () {
		PerformMove();
		PerformRotation();
	}

	void PerformMove () {
		if (velosity != Vector3.zero) {
			rb.MovePosition(rb.position + velosity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation () {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null)
			cam.transform.Rotate(-camRotation);
	}
}
