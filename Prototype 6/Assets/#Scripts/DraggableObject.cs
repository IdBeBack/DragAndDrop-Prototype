using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public LayerMask groundMask;

    private ShopManager shopManager;

    private Camera mainCamera;

    [HideInInspector] public bool justSpawned = false;
    [HideInInspector] public float boundsHeight;

    [HideInInspector] public readonly float rayDistance = 50f;

    private void Awake()
    {
        shopManager = GameObject.Find("#ScriptsHolder").GetComponent<ShopManager>();

        mainCamera = Camera.main;

        BoxCollider collider = transform.GetComponent<BoxCollider>();
        boundsHeight = (collider != null ? collider : transform.GetChild(0).GetComponent<BoxCollider>()).bounds.size.y / 2f;
    }

    private void Start()
    {
        shopManager.SelectItem(gameObject); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            justSpawned = false;

            if (ShopManager.placedCorrectly && !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayDistance, LayerMask.GetMask("ShopItem")))
                shopManager.ResetItem();
        }

        if (justSpawned) Dragging(Input.mousePosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ShopManager.placedCorrectly) shopManager.SelectItem(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (shopManager.IsSelected(gameObject)) Dragging(eventData.position);
    }

    private void Dragging(Vector3 rayDirection)
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(rayDirection), out RaycastHit hit, rayDistance, groundMask))
            transform.position = new Vector3((int)hit.point.x, hit.point.y + boundsHeight + ShopManager.selectedIncreseY, (int)hit.point.z);
    }
}