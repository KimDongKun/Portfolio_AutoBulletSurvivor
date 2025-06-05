using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI;

    public Image tutorialImage; // 튜토리얼 이미지를 표시할 Image 컴포넌트
    public Button prevButton;   // 이전 페이지 버튼
    public Button nextButton;   // 다음 페이지 버튼

    public Sprite[] tutorialImages; // 튜토리얼 이미지 배열
    private int currentIndex = 0;    // 현재 페이지 인덱스

    void Start()
    {
        // Resources 폴더에서 모든 튜토리얼 이미지 로드
        //tutorialImages = Resources.LoadAll<Sprite>("TutorialImages");

        // 버튼 클릭 이벤트 추가
        prevButton.onClick.AddListener(ShowPreviousImage);
        nextButton.onClick.AddListener(ShowNextImage);

        // 첫 번째 이미지 표시
        ShowImage();
    }
    private void Update()
    {
        if (SaveJSonData.isFirstJoinGame)
        {
            tutorialUI.SetActive(true);
            SaveJSonData.isFirstJoinGame = false;
        }
    }
    public void First_ShowImage()
    {
        currentIndex = 0;
        ShowImage();
    }
    void ShowImage()
    {
        if (tutorialImages.Length > 0)
        {
            tutorialImage.sprite = tutorialImages[currentIndex];
        }
        UpdateButtons();
    }

    void ShowPreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowImage();
        }
    }

    void ShowNextImage()
    {
        if (currentIndex < tutorialImages.Length - 1)
        {
            currentIndex++;
            ShowImage();
        }
    }

    void UpdateButtons()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < tutorialImages.Length - 1;
    }
}
