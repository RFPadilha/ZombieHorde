using chillestCapybara;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerMono : MonoBehaviour
{
    [SerializeField] Slider playerHealthSlider;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI killsText;
    PersistentStats persistentStats;
    private Entity _playerEntity;
    private EntityManager _entityManager;
    bool startUpdating = false;
    public float kills = 0f;
    public float elapsedTime = 0f;
    IEnumerator Start()
    {
        persistentStats = FindObjectOfType<PersistentStats>();
        yield return new WaitForSeconds(1f);
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _playerEntity = _entityManager.CreateEntityQuery(typeof(PlayerHealthComponent)).GetSingletonEntity();
        playerHealthSlider.maxValue = _entityManager.GetComponentData<PlayerHealthComponent>(_playerEntity).maxValue;
        startUpdating = true;
    }

    void Update()
    {
        if (startUpdating)
        {
            UpdatePlayerHealth();
            UpdateTimer();
            UpdateScore();
        }
    }
    void UpdatePlayerHealth()
    {
        float currentHealth = _entityManager.GetComponentData<PlayerHealthComponent>(_playerEntity).currentValue;
        playerHealthSlider.value = currentHealth;
        if (currentHealth <= 0) EndGame();
    }
    void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        float minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void UpdateScore()
    {
        kills = _entityManager.GetComponentData<PlayerScoreComponent>(_playerEntity).score;
        killsText.text = kills.ToString();
    }
    void EndGame()
    {
        persistentStats.score = kills;
        float minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = Mathf.FloorToInt(elapsedTime % 60);
        persistentStats.timeSurvived = string.Format("{0:00}:{1:00}", minutes, seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }
}
