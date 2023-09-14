using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnCages : MonoBehaviour
{
    [SerializeField] private GameObject cage;
    [SerializeField] private GameObject key;
    [SerializeField] private Transform cages;
    [SerializeField] private Transform keys;

    public static int totCages = 4;
    //private int timer = 180f;

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();

        for(int i=0; i<totCages; i++)
        {
            Vector3 v = new Vector3( Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            GameObject newCage = Instantiate(cage, v, Quaternion.identity);
            newCage.transform.SetParent(cages);

            Vector3 v1 = new Vector3(Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 0.5f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            GameObject newKey = Instantiate(key, v1, Quaternion.identity);
            newKey.transform.SetParent(keys);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
