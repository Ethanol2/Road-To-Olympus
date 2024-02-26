using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TravelManager : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float timeToTravel = 5f;

    [Header("References")]
    [SerializeField] private CharacterStats playerStats;
    [SerializeField] private Transform travelUI;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text kmsText;
    [SerializeField] private TMP_Text kmRemainingText;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(CloseWindow);
        travelUI.gameObject.SetActive(false);
    }
    public void Travel(Milestone[] milestones, System.Action<int> onTravelStop)
    {
        StartCoroutine(TravelRoutine(milestones, onTravelStop));
    }
    private IEnumerator TravelRoutine(Milestone[] milestones, System.Action<int> onTravelStop)
    {
        DayNightCycle.Instance.TimeRunsOnUpdate = false;
        int targetDistance = milestones.Length;

        travelUI.gameObject.SetActive(true);
        headerText.gameObject.SetActive(false);
        kmsText.gameObject.SetActive(false);
        kmRemainingText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        float hoursPassed = 10f / playerStats.Speed;
        int minutesPassed = Mathf.RoundToInt(hoursPassed * 60);
        float dramaTime = 0.5f;
        float dramaTimer = 0f;

        yield return new WaitForSeconds(dramaTime);
        while (dramaTimer < 0f)
        {
            dramaTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0)) { break; }
            yield return null;
        }

        headerText.gameObject.SetActive(true);
        headerText.text = "Travelling";

        dramaTimer = 0f;

        yield return new WaitForSeconds(dramaTime);
        while (dramaTimer < 0f)
        {
            dramaTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0)) { break; }
            yield return null;
        }

        kmsText.gameObject.SetActive(true);
        kmsText.text = "0 KM";

        int distanceTravelled = 0;
        float timePerKM = timeToTravel / targetDistance;
        DayNightCycle.Instance.LerpTime(minutesPassed, timeToTravel);
        while (distanceTravelled < targetDistance)
        {
            headerText.text = "Travelling";
            int periodCount = 0;
            while (periodCount <= 4)
            {
                yield return new WaitForSeconds(timePerKM / 4f);
                headerText.text += ".";
                periodCount++;
            }            

            distanceTravelled++;
            kmsText.text = $"{distanceTravelled} KM";
            
            if (EncounterManager.Instance.CheckForEncounter(milestones[distanceTravelled - 1]))
            {
                CloseWindow();
                onTravelStop(distanceTravelled);
                yield break;
            }
        }

        yield return new WaitForSeconds(dramaTime);
        kmRemainingText.gameObject.SetActive(true);
        kmsText.text = $"You travelled {distanceTravelled} KM in {minutesPassed} minutes";

        yield return new WaitForSeconds(dramaTime);
        kmRemainingText.text = $"{ProgressTracker.Instance.KilometersRemaining - distanceTravelled} KM remaining";

        yield return new WaitForSeconds(0.2f);

        continueButton.gameObject.SetActive(true);
        onTravelStop(distanceTravelled);
    }
    private void CloseWindow()
    {
        DayNightCycle.Instance.TimeRunsOnUpdate = true;

        travelUI.gameObject.SetActive(false);
    }
}
