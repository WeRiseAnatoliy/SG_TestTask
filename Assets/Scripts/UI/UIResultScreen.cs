using UnityEngine;
using UnityEngine.SceneManagement;

public class UIResultScreen : MonoBehaviour
{
    private LevelController _levelController;
    [SerializeField] GameObject _victoryScreen;
    [SerializeField] GameObject _defeatScreen;

    private void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        if(_levelController != null)
        {
            _levelController.OnLevelComplete += OnLevelComplete;
            _levelController.OnLevelLoose += OnLevelLoose;
        }

        _victoryScreen.SetActive(false);
        _defeatScreen.SetActive(false);
    }

    private void OnLevelComplete ()
    {
        _victoryScreen.SetActive(true);
    }

    private void OnLevelLoose()
    {
        _defeatScreen.SetActive(true);
    }

    public void RestartLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}