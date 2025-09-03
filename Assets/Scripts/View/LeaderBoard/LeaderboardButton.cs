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
        // Подписка на событие нажатия кнопки
        _button.onClick.AddListener(ShowLeaderboard);
    }

    private void ShowLeaderboard()
    {
        // Проверка доступности социальной платформы
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            // Если пользователь не авторизован
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
                    Debug.LogError("Ошибка авторизации!");
                }
            });
        }
    }
}
