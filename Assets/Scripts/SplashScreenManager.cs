using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    public GameObject splashKyle;
    public GameObject splashUnity;
    public GameObject splashTitle;
    public GameObject mainMenu;

    public float transitionTime = 3.0f;

    void Start()
    {
        StartCoroutine(PlaySplashSequence());
    }

    IEnumerator PlaySplashSequence()
    {
        splashKyle.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        splashKyle.SetActive(false);

        splashUnity.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        splashUnity.SetActive(false);

        splashTitle.SetActive(true);

        while(!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        splashTitle.SetActive(false);
        mainMenu.SetActive(true);
    }
}
