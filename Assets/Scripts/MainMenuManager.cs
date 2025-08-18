using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void NewRun()
    {
        ScreenFader.Instance.FadeToScene("Starter Screen");
    }
}
