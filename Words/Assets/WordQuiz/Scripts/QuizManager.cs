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
    [SerializeField] public GameObject hint;
    [SerializeField] public GameObject timeHolder;
    [SerializeField] public GameObject soundHolder;
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
    [SerializeField]
    private Text soundBtn;
    [SerializeField]
    private  Text timerBtn;
    [SerializeField]
    private Text hintText;
    [SerializeField]
    private Text winText;
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
    public AudioSource googleAudio;

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
    public void  DownoadTheAudio()
    {
        string url = "http://api.voicerss.org/?key=afc4e28b1aed4c71866161a3a22585c3&hl=en-us&src=" + Answer+ "&r=-3";
        //string url = "http://api.voicerss.org/?key=afc4e28b1aed4c71866161a3a22585c3&hl=en-us&src=" + Answer+ "&r=-3&c=MP3";
        if ((AudioListener.pause == false))
        {
            WWW www = new WWW(url);
            //yield return www;
              googleAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
           // googleAudio.clip =new  NAudioPlayer.FromMp3Data(www.bytes);
            googleAudio.Play();
        }
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
    public void reseAllLetters()
    {
        for (int i = 0; i < Answer.Length; i++)
        {
            ResetLastWord();
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
    public  void timerOnOff()
    {
        if (timerBtn.text== "(timer-off)")
        {
            timerBtn.text = "(timer-on)";
            timeHolder.SetActive(true);
        }
        else
        {
            timerBtn.text = "(timer-off)";
            timeHolder.SetActive(false);
        }
       
    }
    public void hint_function1()
    {
        DownoadTheAudio();
        if (timerBtn.text == "(timer-on)")
            hintText.text = Answer.ToUpper() + "\n\n-5 sec";
       else
            hintText.text = Answer;
        hint.SetActive(true);
        // yield return new WaitForSeconds(1);
        //System.Threading.Thread.Sleep(1);
        Invoke("hint_function2", 1f);
    }
    public void hint_function2()
    {
        hint.SetActive(false);
       if(timerBtn.text == "(timer-on)")
        Timer.timeRemaining = Timer.timeRemaining - 4;

    }

    public void resume()
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

    public void sooundonoff() {
        if (AudioListener.pause == true)
        {
            soundHolder.SetActive(true);
            soundBtn.text = "(Sound-On)";
            AudioListener.pause = false;
        }
        else
        {
             soundBtn.text = "(Sound-Off)";
            AudioListener.pause = true;
            soundHolder.SetActive(false);
        }
    }

     public IEnumerator AnswerGuide()
    {
        for (int i = 0; i < Answer.Length; i++)
        {
            answerWordArray[i].SetChar(char.ToUpper(Answer[i]));
        }

        yield return new WaitForSeconds(3);
        reseAllLetters();
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
                //Invoke("DownoadTheAudio", 1f);

            }
            else
            {
                audioSource1.Stop();
                winText.text = "YOU\nWON\n\n\ntime left: " +(int)Timer.timeRemaining+" seconds"; 
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
            Invoke("DownoadTheAudio", 0.5f);
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