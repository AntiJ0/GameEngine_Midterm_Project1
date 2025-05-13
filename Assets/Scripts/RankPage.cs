using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;       // ��ũ ����� �ٴ� �θ�
    [SerializeField] GameObject rowPrefab;        // �� �� ��ũ ǥ�ÿ� ������
    [SerializeField] Button[] stageButtons;       // �������� ���� ��ư�� (Inspector���� 1~5 ��ư ���)

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();

        // �� ��ư�� Ŭ�� �̺�Ʈ ���
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stageNumber = i + 1; // ��ư �ε����� ���� �������� ��ȣ�� ��ȯ
            stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(stageNumber));
        }

        RefreshRankList(1); // �⺻: �������� 1
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