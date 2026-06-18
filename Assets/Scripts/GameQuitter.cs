using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Выход из игры");

#if UNITY_EDITOR
        // В редакторе Unity просто останавливаем Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В собранной игре закрываем приложение
        Application.Quit();
#endif
    }
}