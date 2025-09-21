using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBoxUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameObject unclickedBox;
    private GameObject clickedBox;
    private GameObject checkIcon;
    private GameObject blockedIcon;
    private GameObject nameText;
    private GameObject numberText;
    private GameObject background;
    private TMP_Text levelNameTMPro;
    private string levelName; 

    private string levelState;

    public string levelKey;

    void Awake()
    {
        unclickedBox = transform.Find("Unclicked Box").gameObject;
        clickedBox = transform.Find("Clicked Box").gameObject;
        checkIcon = transform.Find("Check Icon").gameObject;
        blockedIcon = transform.Find("Blocked Icon").gameObject;
        nameText = transform.Find("Level Name").gameObject;
        numberText = transform.Find("Level Number").gameObject;
        background = transform.Find("Background").gameObject;

        clickedBox.SetActive(false);
        unclickedBox.SetActive(true);

        levelNameTMPro = nameText.GetComponentInChildren<TMP_Text>();
        SetSelected(false);
    }

    private IEnumerator Start()
    {
        SetCompletedLevels();
        SetPossibleLevels();
        SetImpossibleLevels();

        yield return null;
        levelName = levelNameTMPro.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MapManager.Instance == null ||
            MapManager.Instance.selectedLevel == null ||
            MapManager.Instance.selectedLevel.LevelNumber != levelKey)
        {
            clickedBox.SetActive(true);
            unclickedBox.SetActive(false);

            levelNameTMPro.text = $"<u>{levelName}</u>";
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MapManager.Instance == null ||
            MapManager.Instance.selectedLevel == null ||
            MapManager.Instance.selectedLevel.LevelNumber != levelKey)
        {
            clickedBox.SetActive(false);
            unclickedBox.SetActive(true);

            levelNameTMPro.text = levelName;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MapManager.Instance != null)
        {
            MapManager.Instance.SelectLevel(levelKey, levelState);

            foreach (LevelBoxUI box in FindObjectsOfType<LevelBoxUI>())
                box.SetSelected(false);

            SetSelected(true);
        }
            
    }

    // Helpers
    void SetSelected(bool isSelected)
    {
        if (levelState == "complete" || levelState == "impossible") return;

        if (isSelected)
        {
            clickedBox.SetActive(true);
            unclickedBox.SetActive(false);
            levelNameTMPro.text = $"<u>{levelName}</u>";
        }
        else
        {
            clickedBox.SetActive(false);
            unclickedBox.SetActive(true);
            levelNameTMPro.text = levelName;
        }
    }

    void SetCompletedLevels()
    {
        if (LevelSession.completedLevels.Contains(levelKey))
        {
            background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

            clickedBox.SetActive(true);
            unclickedBox.SetActive(false);

            nameText.SetActive(false);
            numberText.SetActive(false);
            checkIcon.SetActive(true);

            levelState = "complete";
        }
    }

    void SetPossibleLevels()
    {
        if (LevelSession.completedLevels.Count == 0)
        {
            if (levelKey.StartsWith("1"))
            {
                levelState = "possible";
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;

                clickedBox.SetActive(false);
                unclickedBox.SetActive(true);

                nameText.SetActive(true);
                numberText.SetActive(true);
                blockedIcon.SetActive(false);
            }
            return;
        }

        int highestCompleted = 0;
        foreach (string completed in LevelSession.completedLevels)
        {
            if (int.TryParse(completed.Substring(0, 1), out int num))
                if (num > highestCompleted)
                    highestCompleted = num;
        }

        int possibleLevel = highestCompleted + 1;

        if (levelKey.StartsWith(possibleLevel.ToString()))
            levelState = "possible";
    }

    void SetImpossibleLevels()
    {
        if (LevelSession.completedLevels.Contains("2a"))
        {
            if (levelKey == "2b" || levelKey == "3c" || levelKey == "3d" || levelKey == "4c" || levelKey == "5d")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }
                
        }

        if (LevelSession.completedLevels.Contains("2b"))
        {
            if (levelKey == "2a" || levelKey == "3a" || levelKey == "3b" || levelKey == "4a" || levelKey == "5a")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }

        }

        if (LevelSession.completedLevels.Contains("3a"))
        {
            if (levelKey == "3b" || levelKey == "4b" || levelKey == "5b" || levelKey == "5c")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }

        }

        if (LevelSession.completedLevels.Contains("3b"))
        {
            if (levelKey == "3a" || levelKey == "4a" || levelKey == "5a")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }
        }

        if (LevelSession.completedLevels.Contains("3c"))
        {
            if (levelKey == "3d" || levelKey == "4c" || levelKey == "5d")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }
        }

        if (LevelSession.completedLevels.Contains("3d"))
        {
            if (levelKey == "3c" || levelKey == "4b" || levelKey == "5b" || levelKey == "5c")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }

        }

        if (LevelSession.completedLevels.Contains("5b"))
        {
            if (levelKey == "5c")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }

        }

        if (LevelSession.completedLevels.Contains("5c"))
        {
            if (levelKey == "5b")
            {
                background.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

                clickedBox.SetActive(true);
                unclickedBox.SetActive(false);

                nameText.SetActive(false);
                numberText.SetActive(false);
                blockedIcon.SetActive(true);

                levelState = "impossible";
            }

        }
    }
}
