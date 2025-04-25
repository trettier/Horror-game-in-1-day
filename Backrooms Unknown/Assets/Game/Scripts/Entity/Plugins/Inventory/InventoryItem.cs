using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Transform parentAfterDrag;
    public Item item;
    //[HideInInspector] public int count = 1;

    private Canvas canvas;
    private RectTransform rectTransform;
    private Camera canvasCamera;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasCamera = canvas.worldCamera;
    }


    public void InitializeItem(Item newItem, GameObject player)
    {
        item = newItem;
        image.sprite = item.image;
        item.itemController.Initialize(player);
        //RefreshCount();
    }

    public void Select()
    {
        if (item != null)
        {
            item.itemController.TakeItem();
        }
    }

    public void DeSelect()
    {
        if (item != null)
        {
            item.itemController.RemoveItem();
        }
    }

    //public void RefreshCount()
    //{
    //    countText.text = count.ToString();
    //    bool textActive = count > 1;
    //    countText.gameObject.SetActive(textActive);
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        parentAfterDrag.gameObject.GetComponent<InventorySlot>().AfterDrop();
    }
}
