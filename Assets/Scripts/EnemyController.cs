using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private float speed = 3f;

    private float lookSpeed = 5f;

    private float jumpForce = 7f;

    private Vector3 velosity = Vector3.zero;

    private PlayerMotor motor;

    private Character character;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private BoxCollider col;

    [SerializeField]
    private LayerMask groundLayers;

    private float attackPeriod = 1f;

    private float nextAttack;

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private AudioSource bitting;

    void Start() {
        motor = GetComponent<PlayerMotor>();
        col = GetComponent<BoxCollider>();
        character  = GetComponent<Character>();
        nextAttack = Time.time;
        music.Play();
    }

    void Update () {
        if (player != null) {
            Vector3 velosity = player.transform.position - gameObject.transform.position;
            velosity.y = 0;
            velosity = velosity.normalized * speed;
            motor.Move(velosity);
        }
        else {
            velosity = Vector3.zero;
            motor.Move(velosity);
        }
    }

    void OnCollisionEnter(Collision col) {
        CheckAttack(col.gameObject);
    }

    void OnCollisionStay(Collision col) {
        CheckAttack(col.gameObject);
    }

    void CheckAttack(GameObject obj) {
        if (obj.tag == "Player" && nextAttack <= Time.time) {
            character.Attack(obj.GetComponent<Character>());
            nextAttack = Time.time + attackPeriod;
        }
    }

    private bool isGrounded () {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, 
        col.bounds.min.y, col.bounds.center.z), col.size.x * .9f, groundLayers);
    }
}