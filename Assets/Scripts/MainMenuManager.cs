using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void NewRun()
    {
        LevelSession.Reset();

        if (MapManager.Instance != null)
            MapManager.Instance.GenerateLevels();

        ScreenFader.Instance.FadeToScene("Overview Map");
    }

    public void Continue()
    {
        if(LevelSession.Levels.Count != 0)
            ScreenFader.Instance.FadeToScene("Overview Map");
    }

    public void Leave()
    {
        Application.Quit();
    }
}
