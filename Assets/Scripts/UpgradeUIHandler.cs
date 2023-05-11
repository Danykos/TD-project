using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject s;

   

    public void OnPointerEnter(PointerEventData eventData){
        UIManager.main.SetHoveringState(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
        s.SetActive(false);
    }

}
