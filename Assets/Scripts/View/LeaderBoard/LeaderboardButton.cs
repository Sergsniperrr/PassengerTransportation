using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class LeaderboardButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        // �������� �� ������� ������� ������
        _button.onClick.AddListener(ShowLeaderboard);
    }

    private void ShowLeaderboard()
    {
        // �������� ����������� ���������� ���������
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            // ���� ������������ �� �����������
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
                    Debug.LogError("������ �����������!");
                }
            });
        }
    }
}
