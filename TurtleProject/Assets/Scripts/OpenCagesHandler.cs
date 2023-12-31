using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    private GameObject canvas;
    private TextMeshProUGUI timer_text, crub_text, NPCName, dialogueText;
    private Transform MazePrompt;
    private Image crub_icon, key_icon;

    private float timeRemaining;
    private float seconds;

    private bool hasKey;
    private int openCages;
    private int totCages;

    private GameObject[] arr_cages;

    void Awake()
    {
        this.totCages = this.GetComponent<SpawnCages>().totalCages;  //prendo il numero di casse
        this.canvas = GameObject.Find("Canvas");
        
        this.timer_text = canvas.transform.Find("MazeContainer/TimerText").gameObject.GetComponent<TextMeshProUGUI>();
        this.crub_icon = canvas.transform.Find("MazeContainer/CrubIcon").gameObject.GetComponent<Image>();
        this.key_icon = canvas.transform.Find("MazeContainer/KeyIcon").gameObject.GetComponent<Image>();
        this.crub_text = canvas.transform.Find("MazeContainer/FreedCrub").gameObject.GetComponent<TextMeshProUGUI>();
        
        this.MazePrompt = canvas.transform.Find("MazePrompt");
        MazePrompt.gameObject.SetActive(false);
        this.NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        this.dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();

        //COMMENTO PROVA 

        /*
        timer_text.enabled = false;
        crub_icon.enabled = false;
        key_icon.enabled = false;
        crub_text.enabled = false;
        */
    }

    //TODO: richiama ogni volta che parte minigame MazeExploring
    public void restartMazeGame()
    {
        this.timeRemaining = 3f;
        this.seconds = Mathf.Round(timeRemaining);
        Debug.Log("SECONDI: " + this.seconds);

        this.hasKey = false;
        this.openCages = 0;

        timer_text.enabled = true;
        timer_text.SetText(seconds.ToString());

        crub_icon.enabled = true;
        key_icon.enabled = false;

        crub_text.enabled = true;
        crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());

        this.GetComponent<SpawnCages>().restartGame();
        //TODO: fai partire Coroutine(?)
    }
    public void mazeStartPrompt()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
            return;
        if (!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            MazePrompt.gameObject.SetActive(true);
        if (Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            MazePrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            NPCName.SetText("Pesce");
            dialogueText.SetText("Hey Shelly! Ci sono dei granchi che hanno bisogno di essere liberati! \n" +
                "Ti va di aiutarmi?" + " Nel labirinto troverai delle chiavi con cui poter aprire le gabbie \n" +
                "Attenta! Puoi prendere solo una chiave alla volta ed hai 3 minuti di tempo per liberarli tutti \n");
        }
    }
    public void PesceTriggerExit()
    {
        if (canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        if (MazePrompt.gameObject.activeSelf)
            MazePrompt.gameObject.SetActive(false);
        if (!canvas.transform.Find("BarsPanel").gameObject.activeSelf)
            canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }

    private void Update()
    {
        //if (GameObject.Find("Director").GetComponent<GameDirector>().getGameState() != GameDirector.GameState.MazeExploring)
            //return;

        if (this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
            this.seconds = Mathf.Round(timeRemaining);
            this.timer_text.SetText(seconds.ToString());
        }


        if(IsFinished())   
        {
            //Debug.Log("THE END");
            this.timer_text.enabled = false;
            this.crub_text.enabled = false;
            this.crub_icon.enabled = false;
            this.key_icon.enabled = false;
            this.hasKey = false;

            //faccio scomparire le chiavi
            GameObject[] arr_keys = GameObject.FindGameObjectsWithTag("Chiave");
            for(int i = 0; i<arr_keys.Length; i++)
            {
                Destroy(arr_keys[i]);
            }

            //ANIMAZIONE gabbie che salgono 
            this.arr_cages = GameObject.FindGameObjectsWithTag("Gabbia");
            Debug.Log("dim array: " + arr_cages.Length);

            if(arr_cages.Length != 0)
            {
                //Debug.Log(arr_cages);
                //StartCoroutine(cageGoesUp());
                for (int i = 0; i < arr_cages.Length; i++)
                {
                    if (arr_cages[i] != null)
                    {
                        arr_cages[i].GetComponent<CageScript>().GoUp();
                        if(arr_cages[i].transform.position.y > 100f)
                        {
                            //arr_cages[i].GetComponent<Rigidbody>().isKinematic = true;      //ho gi� disabilitato la fisica per l'oggetto
                            Debug.Log("gabbia distrutta");
                            Destroy(arr_cages[i]);
                        }
                    }

                }
            }
            else
            {
                this.gameObject.SetActive(false);
                Debug.Log("ogg posizioni ELIMINATO");
            }
                




            //Debug.Log("GIOCO FINITO");
            //this.gameObject.SetActive(false);  //cos�, una volta finito il gioco, Update non viene pi� richiamato

        }
    }
    
    //TODO: sistemare coroutine
    IEnumerator cageGoesUp()
    {

        for (int i = 0; i < arr_cages.Length; i++)
        {
            if (arr_cages[i] != null)
            {
                arr_cages[i].GetComponent<CageScript>().GoUp();
                if (arr_cages[i].transform.position.y > 120f)
                {
                    //arr_cages[i].GetComponent<Rigidbody>().isKinematic = true;      //ho gi� disabilitato la fisica per l'oggetto
                    Debug.Log("gabbia distrutta");
                    Destroy(arr_cages[i]);
                }
            }

        }
        yield return new WaitForSeconds(4);
        this.gameObject.SetActive(false);
        StopCoroutine(cageGoesUp());

    }



    /*public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!hasKey)
                {
                    Destroy(other.gameObject);
                    this.hasKey = true;
                    Debug.Log("ho la chiave");
                }
                else
                    return;
            }
        }
       
        if (other.CompareTag("Gabbia"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hasKey && other.GetComponent<CageScript>().isLocked)
                {
                    Debug.Log("apro gabbia");
                    other.GetComponent<CageScript>().OpenCage();
                    this.hasKey = false;
                    this.openCages++;
                    Debug.Log("GABBIE APERTE: " + this.openCages);
                    this.crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());
                }
                else
                    return;
                
            }
        }
           
    }*/

    //TODO: temporaneo (metodo chiamato da TurtleController)
    public void TriggerMethod(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!hasKey)
                {
                    Destroy(other.gameObject);
                    this.hasKey = true;
                    this.key_icon.enabled = true;
                    Debug.Log("ho la chiave");
                }
                else
                    return;
            }
        }

        if (other.CompareTag("Gabbia"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hasKey && other.GetComponent<CageScript>().isLocked)
                {
                    Debug.Log("apro gabbia");
                    other.GetComponent<CageScript>().OpenCage();
                    this.hasKey = false;
                    this.key_icon.enabled = false;
                    this.openCages++;
                    Debug.Log("GABBIE APERTE: " + this.openCages);
                    this.crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());
                }
                else
                    return;

            }
        }
    }
    private bool IsFinished()
    {
        if(this.openCages == this.totCages || this.seconds == 0)
         {
            Debug.Log("FINE" + " openCages: " + this.openCages + " + seconds: " + this.seconds);
             return true;
         }
        else
         { 
             return false; 
         }
    }

}
