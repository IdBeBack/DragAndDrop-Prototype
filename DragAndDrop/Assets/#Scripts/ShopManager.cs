using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private LayerMask shopItemLayer;
    [SerializeField] private Material selectMaterial;
    [SerializeField] private Material errorMaterial;

    private GameObject selectedItem;
    private MeshRenderer selectedItemRenderer;
    private BoxCollider selectedItemCollider;

    private Material initialMaterial;

    private bool hasErrorMaterial;

    [HideInInspector] public static bool placedCorrectly = true;

    [HideInInspector] public static readonly float selectedIncreseY = .05f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && selectedItem)
            selectedItem.transform.Rotate(Vector3.up, 90f);

        if (Input.GetKeyDown(KeyCode.Delete) && selectedItem)
            Destroy(selectedItem);

        if (selectedItemRenderer)
        {
            if (Collides())
            {
                if (!hasErrorMaterial)
                {
                    selectedItemRenderer.sharedMaterial = errorMaterial;
                    hasErrorMaterial = true;
                    placedCorrectly = false;
                }
            }
            else
            {
                if (hasErrorMaterial)
                {
                    selectedItemRenderer.material = selectMaterial;
                    hasErrorMaterial = false;
                    placedCorrectly = true;
                }
            }
        }
    }

    public bool IsSelected(GameObject shopItem) => selectedItem == shopItem;

    public void SelectItem(GameObject shopItem)
    {
        DeselectItem();

        selectedItem = shopItem;

        selectedItem.transform.Translate(Vector3.up * selectedIncreseY);

        MeshRenderer renderer = selectedItem.GetComponent<MeshRenderer>();
        selectedItemRenderer = renderer ? renderer : selectedItem.transform.GetChild(0).GetComponent<MeshRenderer>();

        initialMaterial = selectedItemRenderer.material;
        selectedItemRenderer.material = selectMaterial;

        BoxCollider collider = selectedItem.GetComponent<BoxCollider>();
        selectedItemCollider = (collider ? collider : selectedItem.transform.GetChild(0).GetComponent<BoxCollider>());
    }

    public void ResetItem()
    {
        DeselectItem();

        selectedItem = null;
        selectedItemRenderer = null;
    }

    private void DeselectItem() 
    { 
        if (selectedItem)
        {
            selectedItemRenderer.material = initialMaterial;
            selectedItem.transform.Translate(Vector3.down * selectedIncreseY);
        }
    }

    private bool Collides() =>
        selectedItemCollider ? Physics.OverlapBox(selectedItemCollider.bounds.center, 
            selectedItemCollider.bounds.extents * .99f, Quaternion.identity, shopItemLayer).Length > 1 : false;
}