using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int lifes=3;
    int highscore;
    int currentQuestion=0;
    int misstakes = 0;
    int reward;

    int buttonId;

    public List<Question> questions;

    //public TMP_Text scoreRound;
    public TMP_Text scoreTotal;
    private int score;
    public GameObject questionField;
    public List<GameObject> answerFields;
    public List<GameObject> buttonBlock;
    public List<GameObject> lifesIcons;
    public RadialFill clock;

    public GameObject muteSound;
    public GameObject unmuteSound;
    public GameObject muteMusic;
    public GameObject unmuteMusic;

    public GameObject muteSound1;
    public GameObject unmuteSound1;
    public GameObject muteMusic1;
    public GameObject unmuteMusic1;

    public GameObject Menu;
    public GameObject Game;
    public GameObject End;

    public GameObject soundCorrect;
    public GameObject soundWrong;
    public GameObject soundLine;
    public GameObject soundAlarm;

    public GameObject menuStart;

    public TMP_Text highscoreTextMenu;
    public TMP_Text highscoreText;
    public TMP_Text scoreText;


    private bool isTime=false;
    private bool isSound = true;
    private bool isMusic = true;

    private ADSpam InstanceAD;
    // Start is called before the first frame update
    void Start()
    {
        Shuffle(questions);
        //StartCoroutine("RoundStart");
        highscore= PlayerPrefs.GetInt("highscore", 0);
        isSound = PlayerPrefs.GetInt("sound", 1) == 1;
        if(!isSound)
        {
            TurnOffSound();
        }
        isMusic = PlayerPrefs.GetInt("music", 1) == 1;
        if (!isMusic)
        {
            TurnOffMusic();
        }
        highscoreTextMenu.text = "ЛУЧШИЙ СЧЕТ: " + highscore.ToString();
        InstanceAD = GameObject.FindGameObjectWithTag("AD").GetComponent<ADSpam>();
#if !DEBUG
InstanceAD.ShowCommon();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(clock.currentTime==0&&isTime)
        {
            isTime = false;
            TimeUp();
        }
        scoreTotal.text = "Счет: "+score.ToString();
        if(isMusic)
        {
            muteMusic.SetActive(true);
            unmuteMusic.SetActive(false);
            muteMusic1.SetActive(true);
            unmuteMusic1.SetActive(false);
        }    
        else
        {
            muteMusic.SetActive(false);
            unmuteMusic.SetActive(true);
            muteMusic1.SetActive(false);
            unmuteMusic1.SetActive(true);
        }
        if (isSound)
        {
            muteSound.SetActive(true);
            unmuteSound.SetActive(false);
            muteSound1.SetActive(true);
            unmuteSound1.SetActive(false);
        }
        else
        {
            muteSound.SetActive(false);
            unmuteSound.SetActive(true);
            muteSound1.SetActive(false);
            unmuteSound1.SetActive(true);
        }
    }
    public void BTStart()
    {
        StartCoroutine("MenuClick");
#if !DEBUG
InstanceAD.ShowCommon();
#endif
    }
    public void Restart()
    {
        Menu.SetActive(false);
        End.SetActive(false);
        Game.SetActive(true);
#if !DEBUG
InstanceAD.ShowCommon();
#endif
        lifes = 3;
        currentQuestion = 0;
        clock.Restart();
        isTime = true;

        Shuffle(questions);
        score = 0;
        StopAllCoroutines();
        for (int i = 0; i < 3; i++)
        {
            lifesIcons[i].SetActive(true);
        }
        StartCoroutine("RoundStart");
    }
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0,i + 1);

            T tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }

    public void CheckAnswer(bool isTrue)
    {
        buttonId = System.Convert.ToInt32(!isTrue);
        StartCoroutine("Click");
        buttonBlock[buttonId].SetActive(true);
        if (isTrue == questions[currentQuestion].isTrue)
        {
            GameObject clickSoundCorrect = Instantiate(soundCorrect);
            if (!isSound)
                clickSoundCorrect.GetComponent<AudioControll>().Off();
            isTime = false;

            for (int i = 0; i < 2; i++)
            {
                buttonBlock[i].SetActive(true);
            }
            StartCoroutine("Score");
            return;
        }
        GameObject clickSoundWrong = Instantiate(soundWrong);
        GameObject clickSoundLine = Instantiate(soundLine);
        if (!isSound)
        {
            clickSoundWrong.GetComponent<AudioControll>().Off();
            clickSoundLine.GetComponent<AudioControll>().Off();

        }
        answerFields[buttonId].GetComponent<Button>().interactable = false;
        answerFields[buttonId].transform.Find("Image").gameObject.SetActive(true);
    
        misstakes++;
        
        if (misstakes==1)
        {
            for (int i = 0; i < 2; i++)
            {
                buttonBlock[i].SetActive(true);
            }
            reward = 0;
            //scoreRound.text = reward.ToString();
            isTime = false;
            lifes--;
            lifesIcons[lifes].SetActive(false);
            if (lifes == 0)
            {
                StartCoroutine("DelayEnd");
                return;
            }
            currentQuestion++;
            StartCoroutine("Delay");
        }

    }
    void TimeUp()
    {
        lifes--;
        lifesIcons[lifes].SetActive(false);
        GameObject timeupSound= Instantiate(soundAlarm);
        if (!isSound)
            timeupSound.GetComponent<AudioControll>().Off();
        currentQuestion++;
        for (int i = 0; i < 2; i++)
        {
            buttonBlock[i].SetActive(true);
        }
        if (lifes == 0)
        {
            StartCoroutine("DelayEnd");
        }
        else
        {
            StartCoroutine("Delay");
        }
    }
    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(1f);
        Game.SetActive(false);
        End.SetActive(true);
        if (highscore < score)
            highscore = score;
        scoreText.text = "СЧЕТ: "+score.ToString();
        highscoreText.text = "ЛУЧШИЙ СЧЕТ: "+highscore.ToString();
        PlayerPrefs.SetInt("highscore", highscore);
#if !DEBUG
InstanceAD.ShowCommon();
#endif    
    }
        IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine("RoundStart");
    }
    IEnumerator Score()
    {
        float value = 1f / reward;
        for(int i=0; i< reward; i++)
        {
            score++;
            yield return new WaitForSeconds(value);
        }
        if (currentQuestion+1==questions.Count)
        {
            StartCoroutine("DelayEnd");
        }
        else
        {
        currentQuestion++;
        StartCoroutine("RoundStart");

        }
    }
    IEnumerator Click()
    {
        int id = buttonId;
        float value = 0.3f / 60f;
        for (int i = 0; i < 60; i++)
        {
            if(i<10)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 20)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 30)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 40)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 50)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 60)
            {
                answerFields[id].transform.parent.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }

        }


    }
    IEnumerator MenuClick()
    {
#if !DEBUG
InstanceAD.ShowCommon();
#endif        
        GameObject clickSoundCorrect = Instantiate(soundCorrect);
            if (!isSound)
            clickSoundCorrect.GetComponent<AudioControll>().Off();
        menuStart.transform.Find("Image").gameObject.SetActive(true);
        float value = 0.3f / 60f;
        for (int i = 0; i < 60; i++)
        {
            if (i < 10)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 20)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 30)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 40)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 50)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, 2));
                yield return new WaitForSeconds(value);
            }
            else if (i < 60)
            {
                menuStart.transform.Rotate(new Vector3(0, 0, -2));
                yield return new WaitForSeconds(value);
            }

        }
        menuStart.transform.Find("Image").gameObject.SetActive(false);
        Restart();

    }
    IEnumerator RoundStart()
    {
        clock.Restart();
        isTime = true;

        reward = questions[currentQuestion].reward;

        misstakes = 0;

        questionField.transform.Find("Text").GetComponent<TMP_Text>().text = "";
        for (int i = 0; i < 2; i++)
        {
            answerFields[i].transform.localScale = new Vector3(0, 0, 0);
            answerFields[i].transform.Find("Text").GetComponent<TMP_Text>().text = "";
            buttonBlock[i].SetActive(false);
            answerFields[i].GetComponent<Button>().interactable = true;
            answerFields[i].transform.Find("Image").gameObject.SetActive(false);
        }

        questionField.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        float value = 0.3f / 60f;
        for (int i = 0; i < 60; i++)
        {
            questionField.transform.Rotate(new Vector3(6, 0, 0));
            yield return new WaitForSeconds(value);
        }
        float size = 1f / 60;
        float currentSize=0;
        value = 0.3f / 60;
        questionField.transform.Find("Text").GetComponent<TMP_Text>().text = questions[currentQuestion].question;
        for(int i = 0; i < 60; i++)
        {
            currentSize += size;

            for (int j = 0; j < 2; j++)
            {
                answerFields[j].transform.localScale = new Vector3(currentSize, currentSize, currentSize);
            }
            yield return new WaitForSeconds(value);
        }

            answerFields[0].transform.Find("Text").GetComponent<TMP_Text>().text = "Правда";
            answerFields[1].transform.Find("Text").GetComponent<TMP_Text>().text = "Ложь";
            answerFields[0].transform.localScale = new Vector3(1, 1, 1);
            answerFields[1].transform.localScale = new Vector3(1, 1, 1);


        //scoreRound.text = reward.ToString();
        clock.Unpause();
    }
    public void TurnOnSound()
    {
        isSound = true;
        var obj = GameObject.FindGameObjectsWithTag("Sound");
        foreach(GameObject sound in obj)
        {
            sound.GetComponent<AudioControll>().On();
        }
        PlayerPrefs.SetInt("sound", 1);
    }
    public void TurnOffSound()
    {
        isSound = false;
        var obj = GameObject.FindGameObjectsWithTag("Sound");
        foreach (GameObject sound in obj)
        {
            sound.GetComponent<AudioControll>().Off();
        }
        PlayerPrefs.SetInt("sound", 0);
    }

    public void TurnOnMusic()
    {
        isMusic = true;
        var music= GameObject.FindGameObjectWithTag("Music");
        music.GetComponent<AudioSource>().mute = false;
        PlayerPrefs.SetInt("music", 1);
    }
    public void TurnOffMusic()
    {
        isMusic = false;
        var music = GameObject.FindGameObjectWithTag("Music");
        music.GetComponent<AudioSource>().mute = true;
        PlayerPrefs.SetInt("music", 0);

    }
    public void OpenMenu()
    {
        Menu.SetActive(true);
        End.SetActive(false);
        Game.SetActive(false);
        highscoreTextMenu.text = "ЛУЧШИЙ СЧЕТ: " + highscore.ToString();
#if !DEBUG
InstanceAD.ShowCommon();
#endif


    }
    public void MoreGames()
    {
        Application.OpenURL("https://yandex.ru/games/developer?name=I.V.Games");
    }
}
