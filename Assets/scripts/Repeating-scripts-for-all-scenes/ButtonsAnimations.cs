using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private AudioManager audioManager;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.Buttons(audioManager.buttonHover);
        animator.SetBool("isHover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHover", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickEffect());
        audioManager.Buttons(audioManager.buttonClicked);

    }

    private IEnumerator ClickEffect()
    {
        animator.SetBool("isClick", true);
        yield return new WaitForSeconds(0.2f); // czas trwania efektu klikniêcia
        animator.SetBool("isClick", false);
    }
}

