using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;       // 랭크 목록이 붙는 부모
    [SerializeField] GameObject rowPrefab;        // 한 줄 랭크 표시용 프리팹
    [SerializeField] Button[] stageButtons;       // 스테이지 선택 버튼들 (Inspector에서 1~5 버튼 등록)

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();

        // 각 버튼에 클릭 이벤트 등록
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stageNumber = i + 1; // 버튼 인덱스를 실제 스테이지 번호로 변환
            stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(stageNumber));
        }

        RefreshRankList(1); // 기본: 스테이지 1
    }

    void OnStageButtonClicked(int stage)
    {
        RefreshRankList(stage);
    }

    void RefreshRankList(int stage)
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var sortedData = allData.results
            .Where(r => r.stage == stage)
            .OrderByDescending(x => x.score)
            .ToList();

        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}