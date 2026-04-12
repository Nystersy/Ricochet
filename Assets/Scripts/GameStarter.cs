using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject mainMenu;

    public void StartGame()
    {
        if (mainMenu != null)
            mainMenu.SetActive(false);

        Time.timeScale = 1f;
        Debug.Log("Игра запущена!");
    }
}