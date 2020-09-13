﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GravityController : MonoBehaviour
{
    public static float timeStart;
    [SerializeField] private Animator animRotation;

    [SerializeField] private float secondsDivideBy = 7;
    [SerializeField] private float thrust = 2.0f;
    [SerializeField] private float microThrust = 0.1f;
    private Rigidbody2D _rb;
    private bool _top;

    private bool _thrusted;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ChangeGravity(0));
    }
#endif

    public void StartChangeGravity()
    {
        StartCoroutine(ChangeGravity(timeStart/secondsDivideBy));
    }
    public IEnumerator ChangeGravity(float timeForChangeRotate)
    {
        _thrusted = false;
        if (timeStart >= 0.2)
        {
            if (_top)
                _rb.AddForce(transform.up * (-1 * (1 + timeStart) * thrust), ForceMode2D.Impulse);
            else
                _rb.AddForce(transform.up * ((1 + timeStart) * thrust), ForceMode2D.Impulse);

            _thrusted = true;
        }

        if (!_thrusted)
        {
            if (_top)
                _rb.AddForce(transform.up * (-1 * microThrust), ForceMode2D.Impulse);
            else
                _rb.AddForce(transform.up * microThrust, ForceMode2D.Impulse);
        }    
        _rb.gravityScale *= -1;

        yield return new WaitForSeconds(timeForChangeRotate);
        Rotate();

        _top = !_top;

        timeStart = 0;
        yield return null;
    }
    private void Rotate()
    {
        animRotation.SetBool("Rotate", _top == false);
    }
}