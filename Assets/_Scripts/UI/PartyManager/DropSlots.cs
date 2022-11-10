using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlots : MonoBehaviour, IDropHandler
{
    public int ID = 0;
    [SerializeField] private PartyInfoBlock pInfoBlock;
    PointerEventData lastEventData;

    public void OnDrop(PointerEventData eventData)
    {       
        Debug.Log("OnDrop");
        if(eventData.pointerDrag != null)
        {
            //eventData.pointerDrag.transform.SetParent(transform);
            //Vector2 t = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            switch (ID)
            {
                case 0:
                    pInfoBlock.AddMember(eventData.pointerDrag.GetComponent<SlotStatsSheetHolder>().charaStat);
                    break;
                case 1:
                    pInfoBlock.AddInactiveMember(eventData.pointerDrag.GetComponent<SlotStatsSheetHolder>().charaStat);
                    break;
            }
            //GetComponent<SlotStatsSheetHolder>().charaStat = eventData.pointerDrag.GetComponent<SlotStatsSheetHolder>().charaStat;
            //lastEventData = eventData;
        }
    }

}
