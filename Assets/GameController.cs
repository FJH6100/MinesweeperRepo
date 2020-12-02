using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int NumMines = 10;
    Button[,] MyButtons = new Button[10,10];
    Button[] Mines;
    Text EndText;
    bool GameOver;

    bool IsValid(int Value1, int Value2)
    {
        if ((Value1 > -1 && Value1 < 10) && (Value2 > -1 && Value2 < 10))
        {
            return true;
        }    
        else
            return false;
    }

    List<Button> MineInRange(int Check1, int Check2)
    {
        int Counter = 0;
        List<Button> ValidButtons = new List<Button>();
        //North
        if (IsValid(Check1 - 1,Check2))
        {
            ValidButtons.Add(MyButtons[Check1 - 1, Check2]);
            if (Mines.Contains(MyButtons[Check1 - 1, Check2]))
                Counter++;
        }
        //Northeast
        if (IsValid(Check1 - 1, Check2 + 1))
        {
            ValidButtons.Add(MyButtons[Check1 - 1, Check2 + 1]);
            if (Mines.Contains(MyButtons[Check1 - 1, Check2 + 1]))
                Counter++;
        }
        //East
        if (IsValid(Check1, Check2 + 1))
        {
            ValidButtons.Add(MyButtons[Check1, Check2 + 1]);
            if (Mines.Contains(MyButtons[Check1, Check2 + 1]))
                Counter++;
        }
        //Southeast
        if (IsValid(Check1 + 1, Check2 + 1))
        {
            ValidButtons.Add(MyButtons[Check1 + 1, Check2 + 1]);
            if (Mines.Contains(MyButtons[Check1 + 1, Check2 + 1]))
                Counter++;
        }
        //South
        if (IsValid(Check1 + 1, Check2))
        {
            ValidButtons.Add(MyButtons[Check1 + 1, Check2]);
            if (Mines.Contains(MyButtons[Check1 + 1, Check2]))
                Counter++;
        }
        //Southwest
        if (IsValid(Check1 + 1, Check2 - 1))
        {
            ValidButtons.Add(MyButtons[Check1 + 1, Check2 - 1]);
            if (Mines.Contains(MyButtons[Check1 + 1, Check2 - 1]))
                Counter++;
        }
        //West
        if (IsValid(Check1, Check2 - 1))
        {
            ValidButtons.Add(MyButtons[Check1, Check2 - 1]);
            if (Mines.Contains(MyButtons[Check1, Check2 - 1]))
                Counter++;
        }
        //Northwest
        if (IsValid(Check1 - 1, Check2 - 1))
        {
            ValidButtons.Add(MyButtons[Check1 - 1, Check2 - 1]);
            if (Mines.Contains(MyButtons[Check1 - 1, Check2 - 1]))
                Counter++;
        }
        MyButtons[Check1, Check2].GetComponentInChildren<Text>().text = Counter.ToString();
        return ValidButtons;
    }
    bool CheckForWin()
    {
        int Counter = 0;
        foreach (Button MyButton in MyButtons)
        {
            if (MyButton.GetComponentInChildren<Text>().text != "")
                Counter++;
        }
        if (Counter >= 100-NumMines)
            return true;
        else
            return false;
    }
    void ChangeButtonText(Button ThisButton)
    {
        if (ThisButton.GetComponentInChildren<Text>().text == "" && !GameOver)
        {
            if (Mines.Contains(ThisButton))
            {
                ThisButton.GetComponentInChildren<Text>().text = "X";
                foreach (Button Mine in Mines)
                {
                    Mine.GetComponentInChildren<Text>().text = "X";
                }
                EndText.text = "Game Over";
                GameOver = true;
            }    
            else
            {
                string Letters = ThisButton.name.ToString();
                List<Button> Neighbors = MineInRange(int.Parse(Letters[0].ToString()), int.Parse(Letters[1].ToString()));
                if (int.Parse(ThisButton.GetComponentInChildren<Text>().text) == 0)
                {
                    foreach (Button MyButton in Neighbors)
                    {
                        ChangeButtonText(MyButton);
                    }
                }
                if (CheckForWin())
                {
                    GameOver = true;
                    EndText.text = "You Win";
                    foreach (Button Mine in Mines)
                    {
                        Mine.GetComponentInChildren<Text>().text = "*";
                    }
                }
            }
            ThisButton.enabled = false;

        }
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Start is called before the first frame update
    void Start()
    {
        EndText = GameObject.Find("EndText").GetComponent<Text>();
        for (int j = 0; j < 10; j++)
        {
            for (int k = 0; k < 10; k++)
            {
                string ButtonName = j.ToString() + k.ToString();
                Button ThisButton = GameObject.Find(ButtonName).GetComponent<Button>();
                MyButtons[j, k] = ThisButton;
                ThisButton.onClick.AddListener(() => ChangeButtonText(ThisButton));
            }
        }
        Mines = new Button[NumMines];
        for (int i = 0; i < NumMines; i++)
        {
            Mines[i] = MyButtons[Random.Range(0, 9), Random.Range(0, 9)];
        }
        GameObject.Find("Restart").GetComponent<Button>().onClick.AddListener(RestartGame);
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
