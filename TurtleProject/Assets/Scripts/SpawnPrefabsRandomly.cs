using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class SpawnPrefabsRandomly : MonoBehaviour
{ 
    
    [SerializeField] private GameObject prefabToSpawn1;
    [SerializeField] private GameObject prefabToSpawn2;
    [SerializeField] private GameObject prefabToSpawn3;
    [SerializeField] private GameObject sostegno;
    [SerializeField] private GameObject rete;
    //private Animator anim;
    [SerializeField] private float spawnAreaWidth = 100f; // Larghezza dell'area in cui verranno spawnati i prefabs
    [SerializeField] private float spawnAreaLength = 100f; // Lunghezza dell'area in cui verranno spawnati i prefabs
    [SerializeField] private int numerodaspawnare =10;
    private float originepianox;
    private  float originepianoz;
    public float lunghezzapiano;
    public float larghezzapiano;
    private static int oggettiNelPiano ;
    public List<GameObject> spazzature = new List<GameObject>();
    public Rigidbody rb ;
    public float upwardForce;
    public int a ,b;
  
     public Image img;
    public float totalTime = 60f;
    public float totalTime2;// Il tempo totale del timer in secondi.
    private float currentTime;
    private float currentTime2;
    public int rifiutiraccolti;
    private Transform obstacleRacePrompt;
    private TextMeshProUGUI NPCName, dialogueText, rewardsText, testocontatore,testocronometro,testo5;
    private Button confirmButton, cancelButton;
    private GameObject canvas;
    private bool run ;



    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Trova gli oggetti di gioco e di interfaccia all'avvio
        //sostegno.gameObject.SetActive(true);
        run = false;
        canvas = GameObject.Find("Canvas");
       // this.gameObject.SetActive(false);
        obstacleRacePrompt = canvas.transform.Find("ObstacleRacePrompt");
        obstacleRacePrompt.gameObject.SetActive(false);
        NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        testocronometro=canvas.transform.Find("Trashcronometro").gameObject.GetComponent<TextMeshProUGUI>();
        testocontatore=canvas.transform.Find("trashcontatore").gameObject.GetComponent<TextMeshProUGUI>();
        testo5=canvas.transform.Find("trash5").gameObject.GetComponent<TextMeshProUGUI>();
        img=canvas.transform.Find("imgSpazzatura").gameObject.GetComponent<Image>();
        testocronometro.gameObject.SetActive(false);
        testocontatore.gameObject.SetActive(false);
        testo5.gameObject.SetActive(false);
        img.gameObject.SetActive(false);
        dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        confirmButton = canvas.transform.Find("DialoguePanel/ConfirmButton").gameObject.GetComponent<Button>();
        cancelButton = canvas.transform.Find("DialoguePanel/CancelButton").gameObject.GetComponent<Button>();
      

        //Aggiunge i listener ai bottoni di dialogo
        confirmButton.onClick.RemoveAllListeners();
         confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(ConfirmButton_onClick);
        cancelButton.onClick.AddListener(CancelButton_onClick);
        
    }
    public void raceStartPrompt1()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
            return;
        if(!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
           obstacleRacePrompt.gameObject.SetActive(true);
        if(Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            obstacleRacePrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            NPCName.SetText("pesce rosso");
            dialogueText.SetText("ciao");
        }
    }  
    public void PesceRossoTriggerExit()
    {
        if (canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        if(obstacleRacePrompt.gameObject.activeSelf)
            obstacleRacePrompt.gameObject.SetActive(false);
        if(!canvas.transform.Find("BarsPanel").gameObject.activeSelf)
            canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }
        public void ConfirmButton_onClick()
    {
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        obstacleRacePrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
        Start1();
    }
    public void CancelButton_onClick()
    {
        PesceRossoTriggerExit();
        obstacleRacePrompt.gameObject.SetActive(true);
    }
    // -
    void Start1()
    { 
            if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
        { 
            GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
            return;
        }
         GameDirector.Instance.setGameState(GameDirector.GameState.TrashCollecting);
            a=0;
            b=0;
           // anim=rete.GetComponent<Animator>();
           // totalTime=60f;
           // rifiutiraccolti=0;
            //numerodaspawnare=5;
            sostegno.gameObject.SetActive(false);
            testocronometro.gameObject.SetActive(true);
            testocontatore.gameObject.SetActive(true);
            testo5.gameObject.SetActive(true);
            img.gameObject.SetActive(true);
            totalTime2=totalTime+4f;
           oggettiNelPiano = 0;
           currentTime = totalTime;
           currentTime2=totalTime2;
          Transform tp = GetComponent<Transform>();
          originepianox=tp.position.x;
          originepianoz=tp.position.z;
          run=true;
          SpawnPrefabs();
    }
    void FixedUpdate(){

 
         Debug.Log("oggetti nel paino"+oggettiNelPiano);
         Debug.Log("oggetti raccolti"+rifiutiraccolti);
        if(currentTime>0){
            testocronometro.text = Mathf.Round(currentTime).ToString();
            testocontatore.text = oggettiNelPiano.ToString();
             
            currentTime -= Time.deltaTime;
        }
        if (currentTime2>0){
             testocontatore.text = oggettiNelPiano.ToString();
             currentTime2 -=Time.deltaTime;
        }
         
         if (currentTime <= 0 && a==0)
        {    
            if (b==0){
               rifiutiraccolti=oggettiNelPiano; 
               b=1;
            }
            rb.AddForce(Vector3.up * upwardForce,ForceMode.Impulse);
           
            testocronometro.gameObject.SetActive(false);
            testocontatore.gameObject.SetActive(false);
            testo5.gameObject.SetActive(false);
            img.gameObject.SetActive(false);
            currentTime = 0;
            
        }
      
        if (currentTime2<=0){

          foreach (GameObject spazzatura in spazzature)
            {
                Destroy(spazzatura);
            }
            spazzature.Clear();
         
           a=1;
           currentTime2=0;
           sostegno.gameObject.SetActive(true);
           GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
           run=false;
           //GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
           //this.gameObject.SetActive(false);

           
          
        }

        
        
        

    }
  

    void SpawnPrefabs()
    {
       
        for (int i = 0; i < numerodaspawnare; i++) // Numero di prefabs da spawnare
        {
            Vector3 randomPosition = new Vector3( originepianox+lunghezzapiano,Random.Range(80f,100f), Random.Range(originepianoz-spawnAreaLength/2,originepianoz+spawnAreaLength / 2 ));
            Vector3 randomPosition1 = new Vector3( Random.Range(originepianox-spawnAreaLength/2,originepianox+spawnAreaLength / 2 ),Random.Range(80f,100f), originepianoz+lunghezzapiano);
            Vector3 randomPosition2 = new Vector3( originepianox-lunghezzapiano,Random.Range(80f,100f), Random.Range(originepianoz-spawnAreaLength/2,originepianoz+spawnAreaLength / 2 ));
            Vector3 randomPosition3 = new Vector3( Random.Range(originepianox-spawnAreaLength/2,originepianox+spawnAreaLength / 2 ),Random.Range(80f,100f), originepianoz-lunghezzapiano);
            
            if (i<2){
                if(i%2==0){
                    spazzature.Add(Instantiate(prefabToSpawn1, randomPosition, Quaternion.identity));
                }else{
              spazzature.Add(Instantiate(prefabToSpawn1, randomPosition1, Quaternion.identity));  
                }
            }
            else if(i>=2&&i<4){
                if(i%2==0){
                    spazzature.Add(Instantiate(prefabToSpawn2, randomPosition2, Quaternion.identity));

                }else{
                        spazzature.Add(Instantiate(prefabToSpawn2, randomPosition3, Quaternion.identity));
                }
                  
            }
            else{
                spazzature.Add(Instantiate(prefabToSpawn3, randomPosition, Quaternion.identity));
            }
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano++;
            //Debug.Log("Oggetto entrato nel piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano--;
           // Debug.Log("Oggetto uscito dal piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }
    }
      
    /*private void OnEnable()
    {
       Start1();
    }
    */

     
}






