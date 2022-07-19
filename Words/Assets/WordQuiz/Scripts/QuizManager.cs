using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UPersian.Components;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameLose;
    [SerializeField] public GameObject pausePage;
    [SerializeField]
    private QuizData questionData;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    private RtlText persian;
    [SerializeField]
    private WordData[] answerWordArray;
    [SerializeField]
    private WordData[] optionsWordArray;
    [SerializeField]
    private Button resetBtn;


    // Storing all the answers chracter
    private char[] charArray = new char[12];
    // store next blank
    private int currentAnswerIndex = 0;
    private bool correctAnswer = true;
    private List<int> selectedWordIndex;
    private int currentQuestionIndex = 0 ;
    private GameStatus gameStatus = GameStatus.Playing;
    private string Answer;
    private string persianAnswer;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip audioClip1;
    public AudioClip audioClip2;
   
    private void Awake()
    {
        if(instance == null) instance = this;
        else 
            Destroy(gameObject);

        selectedWordIndex = new List<int>();    
    }

    private void Start()
    {
        SetQuestion();
        }

    //seting charachters for answer
    private void SetQuestion()
    {
        currentAnswerIndex = 0;
        selectedWordIndex.Clear();
        questionImage.sprite= questionData.questions[currentQuestionIndex].questionImage; 
        Answer = questionData.questions[currentQuestionIndex].answer;
        persianAnswer = questionData.questions[currentQuestionIndex].persian_answer;
        persian.text = persianAnswer;

        ResetQuestion();

        for (int i = 0; i < Answer.Length; i++)
        {
            charArray[i] = char.ToUpper(Answer[i]);
        }

        for (int i = Answer.Length; i < optionsWordArray.Length; i++)
        {
             charArray[i] = (char)UnityEngine.Random.Range(65 , 91);
        }

        //to shuffle the options
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList<char>()).ToArray();

        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetChar(charArray[i]);
        }

        currentQuestionIndex++;
        gameStatus = GameStatus.Playing;


    }

    public void SelectedOption(WordData wordData)
    {
        if(gameStatus == GameStatus.Next || currentAnswerIndex >= Answer.Length) return;

        selectedWordIndex.Add(wordData.transform.GetSiblingIndex());
        answerWordArray[currentAnswerIndex].SetChar(wordData.charValue);
        wordData.gameObject.SetActive(false); //so it cant be clicked again
        currentAnswerIndex++;

        if (currentAnswerIndex >= Answer.Length)
        {
            CheckAnswer();
        }

    }

    private void ResetQuestion()
    {
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetChar('_');
        }

        for (int i = Answer.Length ; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < optionsWordArray.Length; i++)
        {
           optionsWordArray[i].gameObject.SetActive(true);
        }
    }

    public void ResetLastWord()
    {
        if (selectedWordIndex.Count > 0 )
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionsWordArray[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1);
            currentAnswerIndex--;
            answerWordArray[currentAnswerIndex].SetChar('_');
        }
    }
    public void pause()
    {
        audioSource1.Stop();
        Timer.timerIsRunning = false;
        pausePage.SetActive(true);
    
    }
    public static void quit()
    {
        Application.Quit();
    }
    public  void resume()
    {
        audioSource1.Play();
        Timer.timerIsRunning = true;
        pausePage.SetActive(false);

    }
    public void ResetGame()
    {
        currentQuestionIndex = 0;
        audioSource1.Play();
        Timer.timeStart();
        gameOver.SetActive(false);
        gameLose.SetActive(false);
        resetBtn.gameObject.SetActive(false);
        SetQuestion();
    }

     public IEnumerator AnswerGuide()
    {
        for (int i = 0; i < Answer.Length; i++)
        {
            answerWordArray[i].SetChar(char.ToUpper(Answer[i]));
        }

        yield return new WaitForSeconds(3);

        for (int i = 0; i < Answer.Length; i++)
        {
           ResetLastWord();
        }
    }

    private void CheckAnswer()
    {
        correctAnswer = true;
        for (int i = 0 ; i < Answer.Length ; i++ )
        {
            if (char.ToUpper(Answer[i]) != char.ToUpper(answerWordArray[i].charValue))
            {
                correctAnswer = false;
                break;
            }
        }

        if (correctAnswer)
        {
            gameStatus = GameStatus.Next;
            Debug.Log("Correct");
            

            if(currentQuestionIndex < questionData.questions.Count)
            {
                audioSource2.clip = audioClip1;
                audioSource2.Play();
                Invoke("SetQuestion" , 0.5f );
                
            }
            else
            {
                audioSource1.Stop();
                Timer.timerIsRunning = false;
                resetBtn.gameObject.SetActive(true);
                gameOver.SetActive(true);
            }
            
        }
        else if (!correctAnswer)
        {
            Debug.Log("Incorrect");
            audioSource2.clip = audioClip2;
            audioSource2.Play();
            StartCoroutine(AnswerGuide());

        }
        
    }

    
}


   

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
    public string persian_answer;
}

public enum GameStatus
{
    Playing,
    Next
}