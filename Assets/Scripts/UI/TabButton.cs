using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    public UnityEvent OnTabDeselected;
    public UnityEvent OnTabSelected;
    
    public Image Background;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        Background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void Select()
    {
        if (OnTabSelected != null) OnTabSelected.Invoke();
    }

    public void Deselect()
    {
        if (OnTabDeselected != null) OnTabDeselected.Invoke();
    }
}