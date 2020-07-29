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
    [SerializeField] private float defParticle;

    private void Start()
    {
        defParticle = Particle.hSliderValue;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void FixedUpdate()
    {
        if (_pointerDown)
        {
            _pointerDownTimer += Time.deltaTime;
            Particle.hSliderValue += 1;
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
        Particle.hSliderValue = defParticle;
        onLongClick.Invoke();

        _pointerDownTimer = 0;
    }
}