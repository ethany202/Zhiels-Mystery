using UnityEngine;
using TMPro;
using System;

public class ClockManager : MonoBehaviour
{

    public TMP_Text time;
    public GameObject timeObj;

    private int guess;
    private int sumIndex = 0;
    private const string FINAL_TIME = "03:08";

    private string[] times = { "02:44", "04:52", "05:36", "03:59", "01:11" };
    private static int[] answers = { 6, 3, 4, 11, 1 };
    private bool[] passedSums;

    private void Start()
    {
        ResetStage();
    }
    public bool ChangeTime()
    {
        guess++;
        passedSums[sumIndex] = true;
        if (guess > times.Length)
        {
            time.text = FINAL_TIME;
            timeObj.SetActive(false);
            return false;
        }
        else if (guess == times.Length)
        {
            sumIndex = guess - 1;
            time.text = times[sumIndex] + "";
            return true;
        }
        else
        {
            while(passedSums[sumIndex])
            {
                sumIndex = (new System.Random()).Next(0, times.Length - 1);
            } 
            time.text = times[sumIndex] + "";
            return true;
        }


    }


    public void ResetStage()
    {
        guess = 1;
        passedSums = new bool[5] { false, false, false, false, false };
        sumIndex = (new System.Random()).Next(0, times.Length - 1);
        time.text = times[sumIndex] + "";
    }

    public static int[] GetAnswers()
    {
        return answers;
    }

    public int CurrentGuess()
    {
        return guess;
    }

    public int CurrentSumIndex()
    {
        return sumIndex;
    }

}
