using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Load the car data
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");

            //Find the player cars prefab
            foreach (CarData cardata in carDatas)
            {
                //We found the car data for the player
                if (cardata.CarUniqueID == playerSelectedCarID)
                {
                    //Now spawn it on the spawnpoint
                    GameObject playerCar = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    playerCar.GetComponent<CarInputHandler>().playerNumber = i + 1;

                    break;
                }
            }
        }
    }

}
