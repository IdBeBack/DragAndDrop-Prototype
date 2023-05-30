using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ShopItemData item;
    [SerializeField] private TextMeshProUGUI nameText;

    private UIDocument itemsDocument;

    private void Awake()
    {
        itemsDocument = GameObject.Find("#ShopItems").GetComponent<UIDocument>();
    }

    private void Start()
    {
        nameText.SetText(item.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ShopManager.placedCorrectly)
        {
            DraggableObject draggableObject = item.prefab.GetComponent<DraggableObject>();

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out RaycastHit hit, draggableObject.rayDistance, draggableObject.groundMask))
            {
                Vector3 newPos = new Vector3(hit.point.x, hit.point.y + draggableObject.boundsHeight, hit.point.z);

                Instantiate(item.prefab, newPos, item.prefab.transform.rotation, itemsDocument.transform);
            }

            draggableObject.justSpawned = true;
        }
    }
}