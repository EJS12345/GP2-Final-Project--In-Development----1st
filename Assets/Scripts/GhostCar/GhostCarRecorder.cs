using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarRecorder : MonoBehaviour
{
    public Transform carSpriteObject;
    public GameObject ghostCarPlaybackPrefab;

    //Local variables
    GhostCarData ghostCarData = new GhostCarData();
    bool isRecording = true;

    //Other components
    Rigidbody2D carRigidbody2D;
    CarInputHandler carInputHandler;

    private void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carInputHandler = GetComponent<CarInputHandler>();

        if (carRigidbody2D == null)
            Debug.LogError("Rigidbody2D component missing from the GameObject.");

        if (carInputHandler == null)
            Debug.LogError("CarInputHandler component missing from the GameObject.");
    }

    void Start()
    {
        if (ghostCarPlaybackPrefab == null)
        {
            Debug.LogError("GhostCarPlaybackPrefab is not assigned in the inspector.");
            return;
        }

        if (carInputHandler == null)
        {
            Debug.LogError("Cannot proceed as CarInputHandler is null.");
            return;
        }

        // Instantiate and load the ghost car
        GameObject ghostCar = Instantiate(ghostCarPlaybackPrefab);
        GhostCarPlayback ghostCarPlayback = ghostCar.GetComponent<GhostCarPlayback>();

        if (ghostCarPlayback == null)
        {
            Debug.LogError("GhostCarPlayback component missing on the ghost car prefab.");
            return;
        }

        ghostCarPlayback.LoadData(carInputHandler.playerNumber);

        StartCoroutine(RecordCarPositionCO());
        StartCoroutine(SaveCarPositionCO());
    }

    IEnumerator RecordCarPositionCO()
    {
        while (isRecording)
        {
            if (carSpriteObject != null)
                ghostCarData.AddDataItem(new GhostCarDataListItem(carRigidbody2D.position, carRigidbody2D.rotation, carSpriteObject.localScale, Time.timeSinceLevelLoad));

            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator SaveCarPositionCO()
    {
        yield return new WaitForSeconds(5);
        SaveData();
    }

    void SaveData()
    {
        string jsonEncodedData = JsonUtility.ToJson(ghostCarData);
        Debug.Log($"Saved ghost data: {jsonEncodedData}");

        if (carInputHandler != null)
        {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}_{carInputHandler.playerNumber}_ghost", jsonEncodedData);
            PlayerPrefs.Save();
        }

        // Stop recording as we have already saved the data
        isRecording = false;
    }
}

