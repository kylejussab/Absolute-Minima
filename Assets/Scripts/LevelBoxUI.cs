using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBoxUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameObject unclickedBox;
    private GameObject clickedBox;

    public string levelKey;

    void Awake()
    {
        unclickedBox = transform.Find("Unclicked Box").gameObject;
        clickedBox = transform.Find("Clicked Box").gameObject;

        clickedBox.SetActive(false);
        unclickedBox.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        clickedBox.SetActive(true);
        unclickedBox.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        clickedBox.SetActive(false);
        unclickedBox.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MapManager.Instance != null)
            MapManager.Instance.SelectLevel(levelKey);
    }
}
