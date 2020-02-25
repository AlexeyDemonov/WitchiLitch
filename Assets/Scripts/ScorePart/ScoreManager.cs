using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text RecordText;
    public Text ScoreText;

    int _record;
    int _currentScore;

    private void Awake()
    {
        _record = LoadRecord();
        _currentScore = 0;

        RecordText.text = _record.ToString();
        ScoreText.text = _currentScore.ToString();

        //Probably move it to LevelSceneInitializer?
        RegularEnemyController.EnemyDown += Handle_EnemyDown;
    }

    private void OnDestroy()
    {
        //Probably move it to LevelSceneInitializer?
        RegularEnemyController.EnemyDown -= Handle_EnemyDown;

        if (_currentScore == _record)
        {
            SaveRecord(_record);
        }
    }

    void Handle_EnemyDown()
    {
        _currentScore++;
        ScoreText.text = _currentScore.ToString();

        if (_currentScore > _record)
        {
            _record = _currentScore;
            RecordText.text = _record.ToString();
        }
    }

    int LoadRecord()
    {
        return UnityEngine.PlayerPrefs.GetInt("ScoreRecord", /*by default:*/0);
    }

    void SaveRecord(int record)
    {
        UnityEngine.PlayerPrefs.SetInt("ScoreRecord", record);
    }
}