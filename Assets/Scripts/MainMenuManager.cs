using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void NewRun()
    {
        ScreenFader.Instance.FadeToScene("Starter Screen");
    }
}
