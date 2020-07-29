using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScalerController : MonoBehaviour, IPointerDownHandler
{
    public Animator[] Animators;//Continiue
    public Animator Home;

    public UnityEvent OnChangeAnim;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnChangeAnim.Invoke();
    }

    public void ScalerHomelAnim()
    {
        Home.SetBool("true", true);
        //Continue[].SetBool("true", false);
    }

    public void ScalerContinueAnim()
    {
        Home.SetBool("true", false);
    }
}