﻿using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementAirControl : MonoBehaviour
  {
    [HideInInspector] public float startSpeed;

    [SerializeField] float forwardSpeed = 100f;
    [SerializeField] float strafeSpeed = 75f;

    PlayerInputController input;

    Rigidbody rigidBody;
    Transform orientation;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void FixedUpdate()
    {
      Movement();
    }

    void Movement()
    {
      var force = orientation.right * input.move.x * strafeSpeed * Time.deltaTime + orientation.forward * input.move.y * forwardSpeed * Time.deltaTime;
      rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
  }
}