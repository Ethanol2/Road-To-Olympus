using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Action = System.Action;

public class ModalController : MonoBehaviour
{
    public static ModalController Instance { get; private set; }

    [SerializeField] private Transform panel;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button[] buttons = new Button[3];
    [SerializeField] private TMP_Text notifsRemaining;

    [Space]
    [SerializeField] private ModalInfo currentModal;
    [SerializeField] private List<ModalInfo> modalList;

    [Space]
    public UnityEvent OnModalOpen;
    public UnityEvent OnModalClosed;

    [System.Serializable]
    public class ModalInfo
    {
        public string Title, Description;
        public Sprite Image;

        public string ButtonText1, ButtonText2, ButtonText3;
        public System.Action Button1, Button2, Button3;
    }

    private void Awake()
    {
        Instance = this;
        panel.gameObject.SetActive(false);
    }
    private void Start()
    {
        buttons[0].onClick.AddListener(OnButton1Click);
        buttons[1].onClick.AddListener(OnButton2Click);
        buttons[2].onClick.AddListener(OnButton3Click);
    }    
    private void OnButton1Click()
    {
        currentModal?.Button1?.Invoke();
        CloseOrNextModal();
    }
    private void OnButton2Click()
    {
        currentModal?.Button2?.Invoke();
        CloseOrNextModal();
    }
    private void OnButton3Click()
    {
        currentModal?.Button3.Invoke();
        CloseOrNextModal();
    }
    private void CloseOrNextModal()
    {
        if (modalList.Count == 0) { panel.gameObject.SetActive(false); return; }
        DisplayModal(modalList[0]);
        modalList.RemoveAt(0);
        UpdateCountText();
    }
    private void DisplayModal(ModalInfo info)
    {
        currentModal = info;

        panel.gameObject.SetActive(true);

        image.overrideSprite = info.Image;
        title.gameObject.SetActive(!string.IsNullOrEmpty(info.Title));
        title.text = info.Title;
        description.gameObject.SetActive(!string.IsNullOrEmpty(info.Description));
        description.text = info.Description;

        buttons[0].gameObject.SetActive(true);
        buttons[0].GetComponentInChildren<TMP_Text>().text = string.IsNullOrEmpty(info.ButtonText1) ? "Close" : info.ButtonText1;

        buttons[1].gameObject.SetActive(info.Button2 != null);
        buttons[1].GetComponentInChildren<TMP_Text>().text = info.ButtonText2;
        buttons[2].gameObject.SetActive(info.Button3 != null);
        buttons[2].GetComponentInChildren<TMP_Text>().text = info.ButtonText3;
    }
    private void UpdateCountText()
    {
        notifsRemaining.text = $"{modalList.Count} Notifications Remaining";
    }

    public static void OpenModal(string title, string description, Sprite image = null, 
        string buttonText1 = null, Action button1 = null, 
        string buttonText2 = null, Action button2 = null, 
        string buttonText3 = null, Action button3 = null)
    {
        OpenModal(new ModalInfo() { 
            Description = description, 
            Title = title, 
            Image = image, 
            Button1 = button1, ButtonText1 = buttonText1, 
            Button2 = button2, ButtonText2 = buttonText2,
            Button3 = button3, ButtonText3 = buttonText3,
        });
    }
    public static void OpenModal(ModalInfo info)
    {
        Instance?._OpenModal(info);
    }
    private void _OpenModal(ModalInfo info)
    {
        if (!panel.gameObject.activeInHierarchy)
        {
            DisplayModal(info);
        }
        else
        {
            modalList.Add(info);
        }
        UpdateCountText();
    }
}
