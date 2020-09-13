using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onLongClick;
    private bool _pointerDown;
    private float _pointerDownTimer;
    private readonly float requiredHoldTime = 10f;


    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Update()
    {
        if (_pointerDown)
        {
            _pointerDownTimer += Time.deltaTime;
            if (_pointerDownTimer >= requiredHoldTime)
            {
                Reset();
            }
        }
    }

    private void Reset()
    {
        _pointerDown = false;
        GravityController.timeStart = _pointerDownTimer;
        onLongClick.Invoke();

        _pointerDownTimer = 0;
    }
}