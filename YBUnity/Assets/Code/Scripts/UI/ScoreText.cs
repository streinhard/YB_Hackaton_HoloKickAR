using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        UpdateScore(0 ,0);
    }

    public void UpdateScore(int left, int right)
    {
        textMesh.text = left + " - " + right;
    }
}
