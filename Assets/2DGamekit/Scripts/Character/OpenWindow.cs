using UnityEngine;
using UnityEngine.UI;
public class OpenWindow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject FountainMenu;
    public GameObject checkpoint;
    // public Button leaveButton;
    public bool canActivate = false;
    void Start()
    {
        // FountainMenu = GameObject.Find("CheckpointMenu");
        checkpoint = this.gameObject;
        // FountainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamekit2D.PlayerInput.Instance.Interact.Down && canActivate)
        {
            OpenMenu();
        }
    }
    public void OnLeaveButtonPressed()
    {
        CloseMenu();
    }

    void OpenMenu()
    {
        FountainMenu.SetActive(true);

    }
    void CloseMenu()
    {
        FountainMenu.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = true;
        }
    }
}
