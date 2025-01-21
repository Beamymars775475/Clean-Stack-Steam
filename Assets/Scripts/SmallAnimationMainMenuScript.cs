using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallAnimationMainMenuScript : MonoBehaviour
{
    public GameObject[] prefabs;

    public GameObject[] prefabsButtons;

    public bool canSpawn;

    public bool isAbleToStartFlood;

    public Transform placeToPutFlyingItems;

    public Transform placeToPutButtons;
    void Start()
    {
        canSpawn = true;
        isAbleToStartFlood = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn && isAbleToStartFlood)
        {
            StartCoroutine(CooldownForDescription(0.05f));
            GameObject prefab = Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(gameObject.transform.position.x+Random.Range(-2, 2), gameObject.transform.position.y+2f, gameObject.transform.position.z), Quaternion.identity);
            prefab.transform.SetParent(placeToPutFlyingItems);
            Rigidbody2D rigidBodyPrefab = prefab.GetComponent<Rigidbody2D>();
            rigidBodyPrefab.velocity = new Vector2(0f, -5f);
        }
    }

    IEnumerator CooldownForDescription(float cooldown)
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldown);

        canSpawn = true;
    }

    public void SpawnMenuButtons()
    {
            GameObject prefab1 = Instantiate(prefabsButtons[0], new Vector3(gameObject.transform.position.x+Random.Range(-0.5f, 0.5f), gameObject.transform.position.y+2f, gameObject.transform.position.z), Quaternion.identity);
            prefab1.transform.SetParent(placeToPutButtons);
            Rigidbody2D rigidBodyPrefab1 = prefab1.GetComponent<Rigidbody2D>();
            rigidBodyPrefab1.velocity = new Vector2(0f, -5f);

            GameObject prefab2 = Instantiate(prefabsButtons[1], new Vector3(gameObject.transform.position.x+Random.Range(-0.5f, 0.5f), gameObject.transform.position.y+3f, gameObject.transform.position.z), Quaternion.identity);
            prefab2.transform.SetParent(placeToPutButtons);
            Rigidbody2D rigidBodyPrefab2 = prefab2.GetComponent<Rigidbody2D>();
            rigidBodyPrefab2.velocity = new Vector2(0f, -5f);

            GameObject prefab3 = Instantiate(prefabsButtons[2], new Vector3(gameObject.transform.position.x+Random.Range(-0.5f, 0.5f), gameObject.transform.position.y+4f, gameObject.transform.position.z), Quaternion.identity);
            prefab3.transform.SetParent(placeToPutButtons);
            Rigidbody2D rigidBodyPrefab3 = prefab3.GetComponent<Rigidbody2D>();
            rigidBodyPrefab3.velocity = new Vector2(0f, -5f);
    }
}
