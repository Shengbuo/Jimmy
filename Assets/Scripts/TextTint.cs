using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Tints the text of the option menu
/// </summary>

public class TextTint : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmp;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.color = new Color32(226, 180, 31, 181);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.color = new Color32(226, 180, 31, 255);
    }
}
