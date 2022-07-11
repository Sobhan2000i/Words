using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    private QuestionData question;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    private WordData[] answerWordArray;
    [SerializeField]
    private WordData[] optionsWordArray;

    private void Awake()
    {
        if(instance == null) instance = this;
        else 
            Destroy(gameObject);
    }


    // Storing all the answers chracter
    private char[] charArray = new char[12];
    // store next blank
    private int currentAnswerIndex = 0;

    

    private void Start()
    {
        SetQuestion();
    }

  
    //seting charachters for answer
    private void SetQuestion()
    {
        currentAnswerIndex = 0;
        ResetQuestion();

        questionImage.sprite= question.questionImage;

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
        if(currentAnswerIndex >= answerWordArray.Length) return;
        answerWordArray[currentAnswerIndex].SetChar(wordData.charValue);
        wordData.gameObject.SetActive(false); //so it cant be clicked again
        currentAnswerIndex++;
    }

    private void ResetQuestion()
    {
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetChar('_');
        }

        for (int i = question.answer.Length ; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }
    }

    
}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}