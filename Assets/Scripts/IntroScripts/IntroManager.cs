using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    int currStage;

    public GameObject introDataObj;
    public TextMeshProUGUI introText;
    public GameObject dialogueBox;
    public Button skipButton;
    public GameObject resultPopup;
    public TextMeshProUGUI ewhaText;

    string[] data; // 특정 씬에 대한 intro data 저장
    bool isSkip = false;
    bool isPrintingLines = false;
    int curDialogueIndex = 0;

    // 목적지에 따라 추가되는 이화력 데이터
    Dictionary<int, int> ewhaPoint = new Dictionary<int, int> {
    { 2, 2 },
    { 3, 3 },
    { 4, 5 },
    { 5, 10 }
    };

    void Start()
    {
        currStage = GameManager.Instance.GetStageNumber();

        data = introDataObj.GetComponent<IntroDataScript>().GetData(currStage);
        StartCoroutine(PrintDialogue());
    }

    IEnumerator PrintDialogue()
    {
        introText.text = "";
        float txtdelay = 0.1f;
        int count = 0;
        isPrintingLines = true;
        while (count < data[curDialogueIndex].Length)
        {
            introText.text += data[curDialogueIndex][count].ToString();
            count++;
            if (!isSkip)
            {
                yield return new WaitForSeconds(txtdelay);
            }
        }
        isPrintingLines = false;
        isSkip = false;
    }

    public void OnClickDialogueBox()
    {
        skipButton.onClick.Invoke();
    }

    public void OnClickDialogueButton()
    {
        if (isPrintingLines)
        {
            isSkip = true;
        }

        if (curDialogueIndex < data.Length - 1) // 마지막 대사 직전까지
        {
            if (!isPrintingLines)
            {
                curDialogueIndex++;
                StartCoroutine(PrintDialogue());
            }
        }
        else if (curDialogueIndex == data.Length - 1) // 마지막 대사일 때
        {
            if (!isPrintingLines)
            {
                StartCoroutine(PrintDialogue());
                GameManager.Instance.AddEwhaPower(ewhaPoint[currStage]);
                resultPopup.SetActive(true);
                ewhaText.text = "+" + ewhaPoint[currStage];
            }
        }
    }

    public void OnClickOkButton()
    {
        // 수정수정~~ TestCameraScene->CameraScene
        if (currStage == 2) { SceneManager.LoadScene("CameraScene"); } // 예외처리: 튜토리얼 스테이지의 경우 바로 카메라 씬
        else { SceneManager.LoadScene("Scene_Quiz"); }

        resultPopup.SetActive(false);
        gameObject.SetActive(false);
    }
}
