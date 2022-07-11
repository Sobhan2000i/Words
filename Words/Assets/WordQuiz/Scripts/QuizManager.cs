using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    private QuestionData question;

    [SerializeField]
    private WordData[] answerWordArray;
    [SerializeField]
    private WordData[] optionsWordArray;

    private void Start()
    {
        SetQuestion();
    }

    // Storing all the answers chracter
    private char[] charArray = new char[12];

    
    //seting charachters for answer
    private void SetQuestion()
    {
        for (int i = 0; i < question.answer.Length; i++)
        {
            charArray[i] = char.ToUpper(question.answer[i]);
        }

        for (int i = question.answer.Length; i < optionsWordArray.Length; i++)
        {
             charArray[i] = (char)UnityEngine.Random.Range(65 , 91);
        }

        //to shuffle the options
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList<char>()).ToArray();

        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetChar(charArray[i]);
        }


    }

    public void SelectedOption(WordData wordData)
    {
        
    }

    
}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}