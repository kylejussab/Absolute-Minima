using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public string LevelNumber;
    public int BackboneLength;
    public float BranchChance;
    public (int min, int max) EnemyRange;
    public int Reward;

    public LevelData(string levelNumber, string levelName, int backboneLength, float branchChance, (int min, int max) enemyRange)
    {
        LevelNumber = levelNumber;
        LevelName = levelName;
        BackboneLength = backboneLength;
        BranchChance = AddVariationToBranchChance(branchChance);
        EnemyRange = enemyRange;
        Reward = CalculateReward();
    }

    private int CalculateReward()
    {
        float baseline = (EnemyRange.min + EnemyRange.max) / 2f;
        float backboneBonus = BackboneLength * 0.5f;
        float branchPenalty = BranchChance * 5f;

        // Core reward calculation
        float reward = baseline + backboneBonus - branchPenalty;

        // Random variation
        float variationPercent = Random.Range(0.01f, 0.40f);
        float variationAmount = reward * variationPercent;

        if (Random.value < 0.5f)
            reward += variationAmount;
        else
            reward -= variationAmount;

        int finalReward = Mathf.RoundToInt(reward);
        return Mathf.Max(1, finalReward);
    }

    private float AddVariationToBranchChance(float baseChance)
    {
        float variationPercent = Random.Range(0.01f, 0.25f);
        float variationAmount = baseChance * variationPercent;

        if (Random.value < 0.5f)
            baseChance += variationAmount;
        else
            baseChance -= variationAmount;

        return Mathf.Clamp(baseChance, 0f, 1f);
    }
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private string[] LevelNames = new string[]
    {
        "Paper Tear",
        "Rough Sketch",
        "Blank Page",
        "Faded Line",
        "Torn Edge",
        "Graphite Mark",
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
        "Faint Grid",
        "Skipped Line",
        "Artistic Dream",
        "Canvas Whispers",
        "Color Burst",
        "Palette Echo",
        "Abstract Flow",
        "Painted Horizon",
        "Creative Spark",
        "Brush Strokes",
        "Vision Fragment",
        "Chromatic Pulse"
    };

    public string currentLevel;

    public LevelData selectedLevel;

    private TMP_Text inkDropsText;
    private Button backButton;
    private TMP_Text notesLevelName;
    private TMP_Text notesLevelNumber;
    private TMP_Text notesOne;
    private TMP_Text notesTwo;
    private TMP_Text notesThree;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overview Map")
        {
            StartCoroutine(InitOverviewMapUI());
        }
    }

    private IEnumerator InitOverviewMapUI()
    {
        yield return null;

        inkDropsText = GameObject.Find("Non-Clickables").transform.Find("Ink Drops Number").GetComponent<TMP_Text>();
        inkDropsText.text = LevelSession.InkDrops + " ink drops";

        backButton = GameObject.Find("Back Button").GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();

        backButton.onClick.AddListener(() => { ScreenFader.Instance.FadeToScene("Main Menu"); });

        notesLevelName = GameObject.Find("Non-Clickables").transform.Find("Notes Level Name").GetComponent<TMP_Text>();
        notesLevelNumber = GameObject.Find("Non-Clickables").transform.Find("Notes Level Number").GetComponent<TMP_Text>();
        notesOne = GameObject.Find("Non-Clickables").transform.Find("Note (1)").GetComponent<TMP_Text>();
        notesTwo = GameObject.Find("Non-Clickables").transform.Find("Note (2)").GetComponent<TMP_Text>();
        notesThree = GameObject.Find("Non-Clickables").transform.Find("Note (3)").GetComponent<TMP_Text>();
        playButton = GameObject.Find("Play").GetComponent<Button>();

        selectedLevel = null;
        playButton.gameObject.SetActive(false);

        ApplyLevelNamesToScene();
    }

    public void GenerateLevels()
    {
        string[] levelNumbers =
        {
            "1",
            "2a", "2b",
            "3a", "3b", "3c", "3d",
            "4a", "4b", "4c",
            "5a", "5b", "5c", "5d"
        };

        List<string> availableNames = new List<string>(LevelNames);
        System.Random rng = new System.Random();

        foreach (string number in levelNumbers)
        {
            if (availableNames.Count == 0) break;

            int index = rng.Next(availableNames.Count);
            string chosenName = availableNames[index];
            availableNames.RemoveAt(index);

            int levelNum = int.Parse(number.Substring(0, 1));

            (int min, int max) backboneRange = GetBackboneRange(levelNum);
            int backboneLength = Random.Range(backboneRange.min, backboneRange.max + 1);

            (float min, float max) branchChanceRange = GetBranchChanceRange(levelNum);
            float branchChance = Random.Range(branchChanceRange.min, branchChanceRange.max);

            (int minEnemies, int maxEnemies) = GetEnemyRange(levelNum);

            LevelData level = new LevelData(number, chosenName, backboneLength, branchChance, (minEnemies, maxEnemies));

            LevelSession.Levels.Add(level);
        }

        // Add the final level
        LevelSession.Levels.Add(new LevelData("6", "The Animator", 0, 0.0f, (0, 0)));
    }

    public void ApplyLevelNamesToScene()
    {
        foreach (LevelData level in LevelSession.Levels)
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

    public void SelectLevel(string levelKey, string levelState)
    {
        LevelData level = LevelSession.Levels.Find(l => l.LevelNumber == levelKey);
        if (level != null)
        {
            selectedLevel = level;

            notesLevelName.text = level.LevelName;
            notesLevelNumber.text = level.LevelNumber + ".";

            if (level.LevelNumber != "6")
            {
                notesOne.text = "Number of rooms: " + level.BackboneLength;
                notesTwo.text = "Shop chance: " + Mathf.Round(level.BranchChance * 100f) + "%";
                notesThree.text = "Reward: " + level.Reward + " ink drops";
            }
            else
            {
                notesOne.text = "???";
                notesTwo.text = "???";
                notesThree.text = "???";
            }
            

            if (levelState == "possible")
            {
                playButton.gameObject.SetActive(true);
                playButton.onClick.RemoveAllListeners();

                playButton.onClick.AddListener(() =>
                {
                    LevelSession.CurrentLevel = level;
                    ScreenFader.Instance.FadeToScene("Level");
                });
            }
            else
                playButton.gameObject.SetActive(false);
        }
    }

    // Helpers
    private (int min, int max) GetBackboneRange(int levelNum)
    {
        switch (levelNum)
        {
            case 1: return (4, 6);
            case 2: return (6, 8);
            case 3: return (8, 11);
            case 4: return (11, 13);
            case 5: return (13, 16);
            default: return (4, 16);
        }
    }

    private (float min, float max) GetBranchChanceRange(int levelNum)
    {
        switch (levelNum)
        {
            case 1: return (0.02f, 0.06f);
            case 2: return (0.05f, 0.12f);
            case 3: return (0.10f, 0.20f);
            case 4: return (0.20f, 0.30f);
            case 5: return (0.25f, 0.40f);
            default: return (0.05f, 0.25f);
        }
    }

    private (int min, int max) GetEnemyRange(int levelNum)
    {
        switch (levelNum)
        {
            case 1: return (2, 5);
            case 2: return (4, 8);
            case 3: return (6, 12);
            case 4: return (8, 15);
            case 5: return (10, 18);
            default: return (12, 18);
        }
    }
}