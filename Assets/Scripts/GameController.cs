using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
public class GameController : MonoBehaviour
{
    public DateTime GameInit;
    public LineRenderer Line;
    public Fade Fade;
    public GridRenderer Grid;
    public BugScript Bug;
    [Space]
    public int TotalTime;
    public int LastTime = 7;
    public float Points;

    [Header("UI")]
    public GameObject line;
    public TextMeshProUGUI Seconds;
    public List<TextMeshProUGUI> AllText;
    public List<TextMeshProUGUI> AllTextMultiply;

    [Space]
    public TextMeshProUGUI CashOutText;
    public TextMeshProUGUI EarnText;
    public TextMeshProUGUI MultiplyText;
    public TextMeshProUGUI GameMultiplyText;
    public TextMeshProUGUI CurrentBalance;
    public TextMeshProUGUI ButtonName;
    public TMP_InputField Field;
    public GameObject EarnObject;
    public GameObject LineTarget;
    public TextMeshProUGUI EarnValueText;
    public Color blackColor;
    public Color punchColor;

    [Header("Game")]
    public bool GameStarted;
    public float Currency;
    public float MultiplyTarget;
    public float TargetMoney;
    public float CashOut;
    public float EarnValue;
    public float Multiply = 1.0f;
    public int FinalSecond = 30;
    [Space]
    public float CompletionTime;
    public AnimationCurve curve;

    int gridX;
    int gridY;
    int intCont;
    int initTime;
    float fullTime;
    private void Start()
    {
        Field.text = "1,00";
        ValueChanged();

        CashOut = 2.0f;
        MultiplyTarget = 10.0f;
    }

    private void Update()
    {
        initTime = TotalTime;
        fullTime = (float)DateTime.Now.Subtract(GameInit).TotalSeconds;
        DateTime timeNow = DateTime.Now;
        double seconds = timeNow.Subtract(GameInit).TotalSeconds;

        //print(seconds);
        TotalTime = (int)seconds;

        Seconds.text = TotalTime.ToString() + "s";


        GameMultiplyText.gameObject.SetActive(GameStarted);
        CurrentBalance.text = Currency.ToString("0.0");
        Multiply = Mathf.Clamp(Multiply, 1.0f, 100f);
        //MultiplyText.text = Multiply.ToString("0.0") + "x";
        EarnValue = TargetMoney * Multiply;
        EarnText.text = EarnValue.ToString("0.0");
        LineTarget.SetActive(GameStarted);

        if (GameStarted)
        {
           
            if (TotalTime > 10)
            {
                AllText[AllText.Count - 1].text = TotalTime.ToString() + "s";
            }
            LineTarget.transform.localPosition = new Vector3(131, 210 * ((MultiplyTarget - 1) / 1f));
            CompletionTime = curve.Evaluate(30 * ((float)DateTime.Now.Subtract(GameInit).TotalSeconds / 30f));
            print(CompletionTime);
            Multiply = (CompletionTime / 2);
            GameMultiplyText.text = Multiply.ToString("0.00") + "x";
            //GameMultiplyText.text = Multiply.ToString("0.00") + "x";
            line.transform.DOLocalMove (new Vector3(Line.lastVertexX, Line.lastVertexY),1f);
            if (Bug.transform.localPosition.x >= 235)
            {
                Grid.gridSize += new Vector2(0.005f + Time.deltaTime, 0);
            }
            if (Bug.transform.localPosition.y >= 210)
            {
                Grid.gridSize += new Vector2(0,0.05f + Time.deltaTime);

                for(int i =0; i < AllTextMultiply.Count;i++)
                {
                    if(i == AllTextMultiply.Count - 1)
                    {
                        AllTextMultiply[i].text = Mathf.RoundToInt(Multiply).ToString() + "x";
                    }
                    else
                    {
                        AllTextMultiply[i].text = Mathf.RoundToInt(Multiply * (0.2f * i)).ToString() + "x";
                        if(i == 0)
                        {
                            AllTextMultiply[i].text = 1.ToString() + "x";

                        }
                    }
                    
                }
            }
            if (initTime != TotalTime)
            {
                GameMultiplyText.transform.DOShakePosition(1f, 5, 15, 90);
            }
            if (TotalTime >= FinalSecond)
            {
                StopGame(false);
            }
            if (Multiply >= MultiplyTarget)
            {
                StopGame(true);
            }


        }

    }

    IEnumerator printInfo()
    {
        while (true && GameStarted)
        {
            
                if (Line.points.Count > 1)
                {
                    yield return new WaitForSeconds(0.01f);

                    float restY = ((11 * (CompletionTime / 14) + Line.points[Line.points.Count - 1].y)) / 2f ;
                    float restX = ((float)DateTime.Now.Subtract(GameInit).TotalSeconds + Line.points[Line.points.Count - 1].x) / 2f;
                    Line.points.Add(new Vector2(restX, restY));

                }
                
                    Line.points.Add(new Vector2((float)DateTime.Now.Subtract(GameInit).TotalSeconds, 11 * (CompletionTime / 14)));
                    Line.SetVerticesDirty();
            
            yield return new WaitForSeconds(0.0005f);

        }
    }

    IEnumerator printTriangles()
    {
        while (true)
        {
            Fade.points.Add(new Vector2((float)DateTime.Now.Subtract(GameInit).TotalSeconds, Points));
            Fade.SetVerticesDirty();
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void PrintNewValues(int x, int y)
    {
        Line.points.Add(new Vector2(x, y));
        
    }

    public IEnumerator ColorPunchText(TextMeshProUGUI Text, Color initial, Color New, float duration)
    {

        Text.DOColor( New, 0.25f).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(duration);
        Text.DOColor( initial, 0.25f).SetEase(Ease.OutBounce);
    }
    public void InteractButton()
    {
        if(GameStarted)
        {
            Line.lastVertexY = 0;

            StopGame(true);
            print("Start");
        }
        else
        {
            FinalSecond = UnityEngine.Random.Range(30, 60);

            Line.lastVertexY = 0;
            MultiplyTarget = Multiply;
            Multiply = 1;
            //curve.AddKey(15f, 0);
            //curve.AddKey(25f, 0);

            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe newFrame = curve.keys[i];
                newFrame.value = UnityEngine.Random.Range(50f, 70f);
                newFrame.outTangent = 0;
                newFrame.inTangent = 0;
                if (i == 0)
                {
                    newFrame.value = 0;
                }
                else if(i == 1)
                {
                    newFrame.time = UnityEngine.Random.Range(60, 80f);

                }
                curve.MoveKey(i, newFrame);
            }
            GameStarted = true;
            ButtonName.text = "CASHOUT";
            StartGame();
        }
    }
    public void StartGame()
    {
        EarnObject.SetActive(false);
        ResetGame();

        StartCoroutine(printInfo());
        //StartCoroutine(printTriangles());

    }
    public void ResetGame()
    {
        Line.points = new List<Vector2>();
        Line.SetVerticesDirty();
        LastTime = 7;
        TotalTime = 0;
        Grid.gridSize = new Vector2Int(4, 5);
        GameInit = DateTime.Now;
    }
    public void StopGame(bool pressButton)
    {
        ButtonName.text = "BET";
        StopCoroutine(printInfo());
        //ResetGame();

        if(pressButton)
        {
            GameStarted = false;
            Currency += EarnValue;
            EarnObject.SetActive(true);
            EarnValueText.text = "+" + (EarnValue).ToString("0.0");
        }
        else
        {
            GameStarted = false;
            Currency -= TargetMoney;
            EarnValueText.text = "-" + (EarnValue ).ToString("0.0");

        }
        Multiply = 1.0f;
        ModifyMultiply(9f);
        MultiplyTarget = 2;
    }
    public void ModifyMultiply(float value)
    {
        if (!GameStarted)
        {
            Multiply = Mathf.Clamp(Multiply + value, 1.2f, 100f);
            CashOutText.text = Multiply.ToString("0.0") + "x";
            EarnValue = TargetMoney * Multiply;
            EarnText.text = EarnValue.ToString("0.0");

           
        }
    }
    public void ValueChanged()
    {
        TargetMoney = float.Parse(Field.text.ToString());
        print(float.Parse(Field.text.ToString()));
        if(TargetMoney > Currency)
        {
            Field.text = Currency.ToString() ;
            TargetMoney = Currency;
        }
        if(TargetMoney< 1)
        {
            Field.text = "0";
            TargetMoney = 1;
        }
        EarnValue = TargetMoney * Multiply;
        EarnText.text = EarnValue.ToString("0.0");
    }

    public void ChangeValue(float multi)
    {
        TargetMoney = Mathf.Clamp(TargetMoney * multi, 0, Currency);

        Field.SetTextWithoutNotify( TargetMoney.ToString());
        //TargetMoney = Currency;
        EarnValue = TargetMoney * Multiply;
        EarnText.text = EarnValue.ToString("0.0");
    }

}
