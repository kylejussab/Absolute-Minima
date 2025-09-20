using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public string LevelNumber;
    public int BackboneLength;
    public float BranchChance;
    public int Reward;

    public LevelData(string levelNumber, string levelName)
    {
        LevelNumber = levelNumber;
        LevelName = levelName;
        // For now defaults — you’ll randomize or set these later
        BackboneLength = 0;
        BranchChance = 0f;
        Reward = 0;
    }
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private string[] LevelNames = new string[]
    {
        "Ink Spill",
        "Paper Tear",
        "Rough Sketch",
        "Blank Page",
        "Faded Line",
        "Torn Edge",
        "Graphite Mark",
        "Ink Blot",
        "Shaded Margin",
        "Empty Spread",
        "Smudged Stroke",
        "Crossed Lines",
        "Curved Spiral",
        "Stained Folio",
        "Ghost Draft",
        "Bent Corner",
        "Scribbled Note",
        "Jagged Binding",
        "Worn Cover",
        "Split Spine",
        "Hidden Margin",
        "Faint Outline",
        "Doodle Nest",
        "Creased Fold",
        "Shadowed Page",
        "Broken Sketch",
        "Paper Grain",
        "Ink Trail",
        "Faint Grid",
        "Skipped Line"
    };

    public string currentLevel;

    public List<LevelData> Levels = new List<LevelData>();
    public LevelData selectedLevel;

    private TMP_Text notesLevelName;
    private TMP_Text notesLevelNumber;
    private Button playButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GenerateLevels();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Overview Map")
        {
            notesLevelName = GameObject.Find("Non-Clickables").transform.Find("Notes Level Name").GetComponent<TMP_Text>();
            notesLevelNumber = GameObject.Find("Non-Clickables").transform.Find("Notes Level Number").GetComponent<TMP_Text>();
            playButton = GameObject.Find("Play").GetComponent<Button>();

            selectedLevel = null;
            playButton.gameObject.SetActive(false);

            ApplyLevelNamesToScene();
        }
    }

    private void GenerateLevels()
    {
        Levels.Clear();

        string[] levelNumbers =
        {
            "1",
            "2a", "2b",
            "3a", "3b", "3c", "3d",
            "4a", "4b", "4c",
            "5a", "5b", "5c", "5d"
        };

        // Pick 14 unique random names
        List<string> availableNames = new List<string>(LevelNames);
        System.Random rng = new System.Random();

        foreach (string number in levelNumbers)
        {
            if (availableNames.Count == 0) break;

            int index = rng.Next(availableNames.Count);
            string chosenName = availableNames[index];
            availableNames.RemoveAt(index);

            LevelData level = new LevelData(number, chosenName);
            Levels.Add(level);
        }
    }

    public void ApplyLevelNamesToScene()
    {
        foreach (LevelData level in Levels)
        {
            GameObject levelObj = GameObject.Find("Level " + level.LevelNumber);

            if (levelObj != null)
            {
                Transform nameTransform = levelObj.transform.Find("Level Name/Text (TMP)");
                if (nameTransform != null)
                {
                    TMP_Text text = nameTransform.GetComponentInChildren<TMP_Text>();
                    if (text != null)
                    {
                        text.text = level.LevelName;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Could not find GameObject for Level " + level.LevelNumber);
            }
        }
    }

    public void SelectLevel(string levelKey)
    {
        LevelData level = Levels.Find(l => l.LevelNumber == levelKey);
        if (level != null)
        {
            selectedLevel = level;

            if (notesLevelName != null)
                notesLevelName.text = level.LevelName;

            if (notesLevelNumber != null)
                notesLevelNumber.text = level.LevelNumber + ".";

            playButton.gameObject.SetActive(true);
        }
    }
}
