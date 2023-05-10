using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject s;
    private bool mouse_over = false;

   

    public void OnPointerEnter(PointerEventData eventData){
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
        s.SetActive(false);
    }

}
