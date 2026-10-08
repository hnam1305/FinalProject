using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject computerUI; // Kéo ComputerUI vào đây

    void Update()
    {
        if (computerUI != null && computerUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseComputer();
            }
        }
    }

    public void OpenComputer()
    {
        if (computerUI != null)
        {
            computerUI.SetActive(true); 

            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseComputer()
    {
        if (computerUI != null)
        {
            computerUI.SetActive(false); 

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}