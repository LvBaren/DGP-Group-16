using UnityEngine;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    [Header("All Buttons in Scene")]
    public List<ButtonTrigger> buttons = new List<ButtonTrigger>();

    private ButtonTrigger activeButton;

    public void OnButtonPressed(ButtonTrigger pressedButton)
    {
        // Sluit de vorige deur en zet vorige plane uit
        if (activeButton != null && activeButton != pressedButton)
        {
            if (activeButton.linkedDoor != null)
                activeButton.linkedDoor.SetOpen(false);

            if (activeButton.nextLevelPlane != null)
                activeButton.nextLevelPlane.SetActive(false);

            activeButton.ResetButton();
        }

        // Open de nieuwe deur en activeer de plane
        activeButton = pressedButton;

        if (activeButton.linkedDoor != null)
            activeButton.linkedDoor.SetOpen(true);

        if (activeButton.nextLevelPlane != null)
            activeButton.nextLevelPlane.SetActive(true);
    }
}
