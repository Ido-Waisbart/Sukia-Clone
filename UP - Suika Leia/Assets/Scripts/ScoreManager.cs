using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] protected TMP_Text totalScoreText;
    [SerializeField] protected TMP_Text scorePopupPrefab;  // Auto destroy prefab with text.
    [SerializeField] protected Transform scorePopupParent;
    private int totalScore = 0;
    //int comboMaxDeltaTime = 1;
    // A combo resets when the player shoots a new fruit.
    private int combo = 1;
    public int Combo => combo;  // For SuikaGameManager.cs to save in the game history.

    public void AddToScore(int points, Vector3 popupPosition){
        points *= combo;
        totalScore += points;
        totalScoreText.text = totalScore.ToString();

        TMP_Text scorePopupText = Instantiate(scorePopupPrefab, popupPosition, Quaternion.identity, scorePopupParent);
        scorePopupText.text = points.ToString();
        combo++;
    }

    public void ResetCombo(){
        combo = 1;
    }
}
