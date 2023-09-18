using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnCages : MonoBehaviour
{
    [SerializeField] private GameObject cagePrefab;
    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private Transform cagesParent;
    [SerializeField] private Transform keysParent;

    [SerializeField] public int totalCages = 4;
    //private int timer = 180f;

    //private Vector3[] mazeSpawnPoints;
    private List<Vector3> mazeSpawnPoints;
    private float posx;
    private float posy;
    private float posz;

    private int min;
    private int max;
    private int num;

    private void Awake()
    {        
        this.mazeSpawnPoints = new List<Vector3>();
        this.posx = this.transform.position.x;
        this.posy = this.transform.position.y;
        this.posz = this.transform.position.z;

        restartGame();
        Spawn();
    }

    public void restartGame()
    {
        mazeSpawnPoints.Add(new Vector3(posx - 54.8f, posy + 10f, posz - 302.6f));
        mazeSpawnPoints.Add(new Vector3(posx + 3.6f, posy + 10f, posz - 298.6f));
        mazeSpawnPoints.Add(new Vector3(posx - 116.8f, posy + 10f, posz - 213.5f));
        mazeSpawnPoints.Add(new Vector3(posx - 58.2f, posy + 10f, posz - 214.4f));
        mazeSpawnPoints.Add(new Vector3(posx - 115.1f, posy + 10f, posz - 86.5f));
        mazeSpawnPoints.Add(new Vector3(posx + 2.8f, posy + 10f, posz - 111.6f));
        mazeSpawnPoints.Add(new Vector3(posx - 52.9f, posy + 10f, posz - 36.9f));
        mazeSpawnPoints.Add(new Vector3(posx - 115.7f, posy + 10f, posz + 1f));
        mazeSpawnPoints.Add(new Vector3(posx - 178.2f, posy + 10f, posz - 71.2f));
        mazeSpawnPoints.Add(new Vector3(posx - 292.6f, posy + 10f, posz - 77.8f));
        mazeSpawnPoints.Add(new Vector3(posx - 371.7f, posy + 10f, posz - 31.4f));
        mazeSpawnPoints.Add(new Vector3(posx - 54.8f, posy + 10f, posz + 7.1f));
        mazeSpawnPoints.Add(new Vector3(posx - 109.9f, posy + 10f, posz - 295.9f));
        mazeSpawnPoints.Add(new Vector3(posx - 4.8f, posy + 10f, posz - 158.6f));
        mazeSpawnPoints.Add(new Vector3(posx, posy + 10f, posz));
    }    

    private void Spawn()
    {
        this.min = 0;
        this.max = mazeSpawnPoints.Count;

        for (int i = 0; i < totalCages * 2; i++)
        {
            this.num = Random.Range(0, mazeSpawnPoints.Count);
            if(i % 2 == 0)
            {
                Instantiate(cagePrefab, mazeSpawnPoints[num], Quaternion.identity, cagesParent);
            }
            else
            {
                Instantiate(keyPrefab, mazeSpawnPoints[num], Quaternion.identity, keysParent);
            }
            this.mazeSpawnPoints.RemoveAt(num);
        }

    }
}
