using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-200)]
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public TMP_FontAsset font;
    public string gameName;


    public Color mainColor;


    public GameObject tutorialHand;
    public GameObject tutorialText;
    public GameObject[] panels;
    public GameObject restartButton;
   

    public GameObject firstSectorProgress;
    public GameObject secondSectorProgress;
    public GameObject thirdSectorProgress;
    public GameObject currentLevelNumberText;
    public GameObject nextLevelNumberText;

    public Color outline0;

    private bool isLevelStartedForUI;
    private bool isLevelCompletedForUI;
    private bool isLevelFailedForUI;
    private bool isRestartButtonPressed;
    private bool isNextLevelButtonPressed;

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        isLevelStartedForUI = false;
        isNextLevelButtonPressed = false;
        isRestartButtonPressed = false;

        PrepareUI();

        LevelProgress();
    }

    private void LevelProgress()
    {
        currentLevelNumberText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.currentLevel.ToString();
        nextLevelNumberText.GetComponent<TextMeshProUGUI>().text = (GameManager.Instance.currentLevel + 1).ToString();
    }

    void Update()
    {
        LevelStartForUI();
        PaintSectorProgress();
    }

    private void PaintSectorProgress()
    {
        if (GameManager.Instance.CompletedLevelSectorCount == 1)
        {
            firstSectorProgress.GetComponent<Image>().color = Color.green;
        }
        else if (GameManager.Instance.CompletedLevelSectorCount == 2)
        {
            secondSectorProgress.GetComponent<Image>().color = Color.green;
        }
        else if (GameManager.Instance.CompletedLevelSectorCount == 3)
        {
            thirdSectorProgress.GetComponent<Image>().color = Color.green;
        }
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void PrepareUI()
    {
        Color transparentMainColor = mainColor;
        transparentMainColor.a = 0f;


        restartButton.SetActive(false);
    }

    public void LevelStartForUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLevelStartedForUI)
            {
                if (isLevelStartedForUI)
                {
                    return;
                }

                tutorialHand.SetActive(false);
                tutorialText.SetActive(false);

                restartButton.SetActive(true);

                GameManager.Instance.isLevelStarted = true;
                GameManager.Instance.isLevelCompleted = false;
                GameManager.Instance.isLevelFailed = false;
                isLevelStartedForUI = true;
            }
        }
    }

    public void LevelCompleteForUI(float delay)
    {
        if (!isLevelCompletedForUI)
        {
            StartCoroutine(WaitForLevelComplete(delay));

            isLevelCompletedForUI = true;
        }
    }

    IEnumerator WaitForLevelComplete(float delay)
    {
        yield return new WaitForSeconds(delay);

        panels[0].SetActive(false);
        panels[1].SetActive(true);

        yield return new WaitForSeconds(0.5f);
    }

    public void LevelFailForUI(float delay)
    {
        if (!isLevelFailedForUI)
        {
            StartCoroutine(WaitForLevelFail(delay));

            isLevelFailedForUI = true;
        }
    }

    IEnumerator WaitForLevelFail(float delay)
    {
        yield return new WaitForSeconds(delay);

        panels[0].SetActive(false);
        panels[2].SetActive(true);

        yield return new WaitForSeconds(1.25f);

        GameManager.Instance.RestartLevel();
    }


    public void NextLevelButton()
    {
        if (isNextLevelButtonPressed)
        {
            return;
        }

        GameManager.Instance.NextLevel();

        isNextLevelButtonPressed = true;
    }

    public void RestartButton()
    {
        if (isRestartButtonPressed)
        {
            return;
        }

        GameManager.Instance.RestartLevel();

        isRestartButtonPressed = true;
    }
}