using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void NewRun()
    {
        ScreenFader.Instance.FadeToScene("Overview Map");
    }
}
