using System.Collections;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    private string letter;
    private TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }

    public string GetLetter()
    {
        return letter;
    }

    public void SetLetter(string _letter)
    {
        letter = _letter;
        textMesh.text = _letter;
    }
}