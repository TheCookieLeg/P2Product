using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();

        if (animator == null) animator = GetComponentInParent<Animator>();
    }

  public void OnPointerDown(PointerEventData eventData){
        if (GameManager.Instance.canClickTimer > 0) return;

        animator.SetTrigger("ButtonDown");
        animator.ResetTrigger("ButtonUp");
    }

    public void OnPointerUp(PointerEventData eventData){
        if (GameManager.Instance.canClickTimer > 0) return;

        animator.SetTrigger("ButtonUp");
    }

    public void OnPointerExit(PointerEventData eventData){
        if (GameManager.Instance.canClickTimer > 0) return;

        animator.SetTrigger("ButtonUp");
        animator.ResetTrigger("ButtonDown");
    }
}
