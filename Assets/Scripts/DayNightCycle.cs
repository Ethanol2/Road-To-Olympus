using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    public bool TimeRunsOnUpdate = true;
    public static DayNightCycle Instance { get; private set; }

    [Space]
    [SerializeField] private int day = 0;
    [SerializeField] private int time = 0;

    [SerializeField] private float tickRate = 5f;
    [SerializeField] private float secondsTracker = 0f;

    [Space]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dateText;

    [Space]
    [SerializeField] private Transform apolloAndArtemis;
    [SerializeField] private Luminosity timeOfDay;
    [SerializeField] private Light sceneLight;

    [Header("Day")]
    [SerializeField] private Color daySkyColour = Color.cyan;
    [SerializeField] private Color dayLightColour = Color.cyan;
    [SerializeField] private float dayStart = 0.3f;

    [Header("Sundown")]
    [SerializeField] private Color sundownSkyColour = Color.red;
    [SerializeField] private Color sundownLightColour = Color.red;
    [SerializeField] private float sundownStart = 0.75f;

    [Header("Night")]
    [SerializeField] private Color nightSkyColour = Color.black;
    [SerializeField] private Color nightLightColour = Color.black;
    [SerializeField] private float nightStart = 0.8f;

    [Header("Sunrise")]
    [SerializeField] private Color sunriseSkyColour = Color.red;
    [SerializeField] private Color sunriseLightColour = Color.red;
    [SerializeField] private float sunriseStart = 0.25f;

    private float timeToMinute = 0f;
    public const int TOTAL_MINUTES = 1440;
    public float TOTAL_SECONDS { get; private set; }
    private Camera cam;

    public Luminosity TimeOfDay => timeOfDay;
    public static float TickRate => Instance.tickRate;
    public UnityEvent OnTick;

    public enum Luminosity
    {
        Night, Sunrise, Day, Sunset
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        TOTAL_SECONDS = TOTAL_MINUTES * tickRate;
        timeToMinute = tickRate;
        UpdateSunAndMoon(time * tickRate);
        UpdateClock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimeRunsOnUpdate) { return; }

        timeToMinute -= Time.deltaTime;
        if (timeToMinute <= 0f)
        {
            timeToMinute = tickRate;
            AddTime(1);
            OnTick.Invoke();
        }
        secondsTracker += Time.deltaTime;
        UpdateSunAndMoon();
    }
    public void AddTime(int minutes, bool updateSun = true)
    {
        time += minutes;
        if (time > TOTAL_MINUTES)
        {
            day++;
            time = time - TOTAL_MINUTES;
        }
        UpdateClock();
        UpdateLuminosity();
        if (updateSun) UpdateSunAndMoon(time * tickRate);
    }
    public void SetTime(int newTime, int newDay)
    {
        time = newTime;
        day = newDay;
        UpdateClock();

        float timePercent = (float)time / (float)TOTAL_MINUTES;
        UpdateLuminosity();
        UpdateSunAndMoon(timePercent * TOTAL_SECONDS);
    }
    public void LerpTime(int minutesToLerp, float lerpTime = 1f)
    {
        StartCoroutine(_LerpTime(minutesToLerp, lerpTime));
    }
    private IEnumerator _LerpTime(int minutesToLerp, float lerpTime)
    {
        bool timeUpdateCache = TimeRunsOnUpdate;

        float timePerMinute = lerpTime / (float)minutesToLerp;
        float timeToMinute = timePerMinute;
        int minutesAdded = 0;

        while (minutesAdded < minutesToLerp)
        {
            timeToMinute -= Time.deltaTime;
            if (timeToMinute <= 0f)
            {
                timeToMinute = timePerMinute;
                minutesAdded++;
                AddTime(1);
            }
            UpdateSunAndMoon();

            yield return null;
        }

        TimeRunsOnUpdate = timeUpdateCache;
    }
    [ContextMenu("Set to Noon")]
    public void SetToNoon()
    {
        SetTime(TOTAL_MINUTES / 2, day);
    }

    [ContextMenu("Set to Midnight")]
    public void SetToMidnight()
    {
        SetTime(0, day);
    }
    [ContextMenu("Set to Sunrise")]
    public void SetToSunrise()
    {
        SetTime(TOTAL_MINUTES / 4, day);
    }
    [ContextMenu("Set to Sundown")]
    public void SetToSundown()
    {
        SetTime((TOTAL_MINUTES / 4) * 3, day);
    }
    private void UpdateSunAndMoon(float realSeconds)
    {
        if (realSeconds > TOTAL_SECONDS)
        {
            realSeconds -= TOTAL_SECONDS;
        }
        secondsTracker = realSeconds;
        UpdateSunAndMoon();
    }
    private void UpdateSunAndMoon()
    {
        if (secondsTracker > TOTAL_SECONDS)
        {
            secondsTracker = 0f;
        }

        Vector3 euler = apolloAndArtemis.localEulerAngles;
        euler.z = Mathf.Lerp(0f, 360f, secondsTracker / TOTAL_SECONDS);
        apolloAndArtemis.localEulerAngles = euler;
    }
    private void UpdateClock()
    {
        float hourFloat = (float)time / (float)TOTAL_MINUTES;
        hourFloat *= 24;
        float minutes = (hourFloat - Mathf.Floor(hourFloat)) * 60f;
        int hourInt = Mathf.RoundToInt(Mathf.Floor(hourFloat));

        string amPM = "AM";

        if (hourInt >= 12)
        {
            hourInt -= 12;
            amPM = "PM";
        }

        if (hourInt == 0f)
        {
            hourInt = 12;
        }

        timeText.text = $"{hourInt}:{Mathf.RoundToInt(minutes).ToString("00")} {amPM}";
        dateText.text = "Day " + (day + 1);
    }
    private void UpdateLuminosity()
    {
        Luminosity oldTimeOfDay = timeOfDay;

        float timePercent = (float)time / (float)TOTAL_MINUTES;

        if (timePercent > dayStart && timePercent < sundownStart)
        {
            timeOfDay = Luminosity.Day;
        }
        else if (timePercent > nightStart || timePercent < sunriseStart)
        {
            timeOfDay = Luminosity.Night;
        }
        else if (timePercent > sunriseStart && timePercent < dayStart)
        {
            timeOfDay = Luminosity.Sunrise;
        }
        else if (timePercent > sundownStart && timePercent < nightStart)
        {
            timeOfDay = Luminosity.Sunset;
        }

        if (oldTimeOfDay != timeOfDay)
            LerpColour();
    }
    private void LerpColour()
    {
        Color skyColour = cam.backgroundColor;
        Color lightColour = sceneLight.color;
        switch (timeOfDay)
        {
            case Luminosity.Day:
                skyColour = daySkyColour;
                lightColour = dayLightColour;
                break;
            case Luminosity.Night:
                skyColour = nightSkyColour;
                lightColour = nightLightColour;
                break;
            case Luminosity.Sunrise:
                skyColour = sunriseSkyColour;
                lightColour = sunriseLightColour;
                break;
            case Luminosity.Sunset:
                skyColour = sundownSkyColour;
                lightColour = sundownLightColour;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(LerpColour(cam.backgroundColor, skyColour, SetSkyColour));
        StartCoroutine(LerpColour(sceneLight.color, lightColour, SetLightColour));
    }
    private void SetSkyColour(Color colour)
    {
        cam.backgroundColor = colour;
    }
    private void SetLightColour(Color colour)
    {
        sceneLight.color = colour;
    }

    private IEnumerator LerpColour(Color start, Color target, Action<Color> SetColour)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            SetColour(Color.Lerp(start, target, t));
            yield return null;
        }

        SetColour(target);
    }

    public static void PauseTime() => Instance.TimeRunsOnUpdate = false;
    public static void ResumeTime() => Instance.TimeRunsOnUpdate = true;
}
