using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FurniturePlacement : MonoBehaviour
{
    /// <summary>
    /// The furniture selected with the Canvas buttons
    /// </summary>
    private GameObject SelectedFurniture;

    /// <summary>
    /// The current position of the furniture placeholder
    /// </summary>
    private Vector3 StoreFurniturePlaceholderPosition;

    /// <summary>
    /// The gameobject to place in the world that moves with the mouse cursor
    /// </summary>
    private GameObject FurniturePlaceholder;

    /// <summary>
    /// Funds script
    /// </summary>
    private Funds FundsScript;

    /// <summary>
    /// During awake the Funds script is assigned
    /// </summary>
    private void Awake()
    {
        FundsScript = this.gameObject.GetComponent<Funds>();
    }

    /// <summary>
    /// During update the FurniturePlaceholder is updated and the furniture is placed by left mouse click
    /// </summary>
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        var MousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        if (SelectedFurniture != null)
        {
            // Get grid cell over which the mouse hovers
            float posX = (float)(Math.Round(MousePosition.x * 2) / 2);
            float posY = (float)(Math.Round(MousePosition.y * 2) / 2);
            var newPosition = new Vector3(posX, posY, 0);

            // If grid cell over which the mouse hovers has changed, then update the position of FurniturePlaceholder or spawn a fresh one
            if (newPosition != StoreFurniturePlaceholderPosition)
            {
                if (FurniturePlaceholder == null)
                {
                    FurniturePlaceholder = Instantiate(SelectedFurniture, StoreFurniturePlaceholderPosition, transform.rotation);
                    var MousePlacementColor = FurniturePlaceholder.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    if (MousePlacementColor != null)
                    {
                        MousePlacementColor.color = new Vector4(1,1,1,0.5f);
                    }
                }
                else
                {
                    FurniturePlaceholder.transform.position = newPosition;
                }

                StoreFurniturePlaceholderPosition = newPosition;
            }

            // Place the SelectedFurniture opn the grid with left mouse button
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var enoughFunds = FundsScript.StartingFunds - FundsScript.Price;
                if (enoughFunds > 0)
                {
                    Instantiate(SelectedFurniture, newPosition, transform.rotation);
                    FundsScript.StartCoroutine("UpdateFunds");
                }
                else if (!FundsScript.isBlinking)
                {
                    FundsScript.StartCoroutine("BlinkRed");
                }
            }
        }
    }

    /// <summary>
    /// Called from Unity3D Canvas furniture buttons.
    /// This function forces to create a new 
    /// </summary>
    /// <param name="obj">Furniture object</param>
    public void UpdateFurnitureSelection(GameObject furniture)
    {
        Destroy(FurniturePlaceholder);
        SelectedFurniture = furniture;
    }
}
