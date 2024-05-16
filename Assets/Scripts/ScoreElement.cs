using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text hazardsText;
    public TMP_Text permitsText;
    public TMP_Text ppesText;
    public TMP_Text scoreText;

    public void NewScoreElement (string _username, int _hazards, int _permits, int _ppes, int _score)
    {
        usernameText.text = _username;
        hazardsText.text = _hazards.ToString();
        permitsText.text = _permits.ToString();
        ppesText.text = _ppes.ToString();
        scoreText.text = _score.ToString();
    }
}