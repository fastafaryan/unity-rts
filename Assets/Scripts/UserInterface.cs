using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DesertSurvival;

namespace DesertSurvival
{
    public class UserInterface : MonoBehaviour
    {
        #region Time 
        [SerializeField]
        private Text day = null;
        [SerializeField]
        private Text clock = null;
        [SerializeField]
        private Text gameSpeed = null;
        public string GameSpeed { set { gameSpeed.text = value; } }
        #endregion

        #region Message
        [SerializeField]
        private GameObject onScreenMessagePanel = null;
        [SerializeField]
        private GameObject onScreenMessageText = null;
        private float timeToDisplayMessage = 1.5f;
        private bool isDisplayingMessage = false;
        private float currentMessageTime = 0f;
        #endregion

        #region Resources 
        [SerializeField]
        private Text oil = null;
        public string Oil { set { oil.text = value; } }
        [SerializeField]
        private Text power = null;
        public string Power { set { power.text = value; } }
        [SerializeField]
        private Text water = null;
        public string Water { set { water.text = value; } }
        [SerializeField]
        private Text steel = null;
        public string Steel { set { steel.text = value; } }
        [SerializeField]
        private Text human = null;
        public string Human { set { human.text = value; } }
        #endregion

        #region Building Information Panel 
        [SerializeField]
        public GameObject buildingInformationPanel;
        #endregion
        // Reference to main controller
        private MainController mainController;

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            // Set reference to main controller
            mainController = GameObject.Find("MainController").GetComponent<MainController>();

            buildingInformationPanel.SetActive(false);
            onScreenMessagePanel.SetActive(false);
            gameSpeed.text = (mainController.time.GameSpeed).ToString();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateDateTimeUI(); // Update date time UI elements.
            RemoveOnScreenMessage();
        }
        #endregion

        #region Entry Methods 

        #region Speed Up Time
        public void SpeedUpTime()
        {
            mainController.time.SpeedUpTime();
        }
        #endregion

        #region Speed Down Time
        public void SpeedDownTime()
        {
            mainController.time.SpeedDownTime();
        }
        #endregion

        #region Close Building Information Panel
        public void CloseBuildingInformationPanel()
        {
            buildingInformationPanel.SetActive(false);
        }
        #endregion

        #region Toggle Activity
        public void ToggleActivity()
        {
            bool isActive = mainController.selection.SelectedObject.GetComponent<Building>().toggleActivity();
            Text activityText = GameObject.Find("T.ToggleActivityButton").GetComponent<Text>();
            if (isActive)
                activityText.text = "Disable";
            else
                activityText.text = "Enable";
        }
        #endregion

        #endregion

        #region Inner Methods

        #region Update Date Time UI
        // Update UI texts of date and time
        private void UpdateDateTimeUI()
        {
            // Convert to string for displaying in UI Text.
            string timeAsClock = "";
            timeAsClock += mainController.time.Hour.ToString().Length == 1 ? "0" + mainController.time.Hour.ToString() : mainController.time.Hour.ToString();
            timeAsClock += mainController.time.Minute.ToString().Length == 1 ? ":0" + mainController.time.Minute.ToString() : ":" + mainController.time.Minute.ToString();
            clock.text = timeAsClock; // Assign value to UI Clock Text.
            day.text = "Day " + mainController.time.Day.ToString(); // Assign value to UI Day Text.
        }
        #endregion

        #region Display On Screen Message
        public void DisplayOnScreenMessage(string Message)
        {
            onScreenMessagePanel.SetActive(true);
            onScreenMessageText.GetComponent<Text>().text = Message;
            isDisplayingMessage = true;
        }
        #endregion

        #region Remove On Screen Message
        public void RemoveOnScreenMessage()
        {
            if (isDisplayingMessage)
            {
                currentMessageTime += UnityEngine.Time.deltaTime;
                if (currentMessageTime >= timeToDisplayMessage)
                {
                    onScreenMessagePanel.SetActive(false);
                    isDisplayingMessage = false;
                    currentMessageTime = 0;
                }
            }
        }
        #endregion

        public void UpdateBuildingName(string value)
        {
            GameObject.Find("T.BuildingName").GetComponent<Text>().text = value;
        }

        #endregion
    }
}
