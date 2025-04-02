using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 diference = rigidBody2D.velocity.normalized;
        float angle = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void Throw(float force, Vector3 direction)
    {
        rigidBody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
