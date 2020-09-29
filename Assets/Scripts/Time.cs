using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesertSurvival;

namespace DesertSurvival
{
    public class Time : MonoBehaviour
    {
        private int gameSpeedMultiplier = 100;

        private int gameSpeed = 1;
        public int GameSpeed { get { return gameSpeed; } }
        private float timeInSeconds = 0;
        public float TimeInSeconds { get { return timeInSeconds; } }
        private float second = 0;
        public float Second { get { return second; } }
        private float minute = 0;
        public float Minute { get { return minute; } }
        private float hour = 0;
        public float Hour { get { return hour; } }
        private float day = 0;
        public float Day { get { return day; } }

        private MainController mainController;

        // Start is called before the first frame update
        void Start()
        {
            mainController = GameObject.Find("MainController").GetComponent<MainController>();
        }

        // Update is called once per frame
        void Update()
        {
            SimulateTime(); // Increase time.
            CalculateDateTime(); // Calculate date time from time in seconds.
        }

        #region Simulate Time
        // Increase time.
        private void SimulateTime()
        {
            timeInSeconds += UnityEngine.Time.deltaTime * gameSpeed * gameSpeedMultiplier;
        }
        #endregion

        #region Speed Up Simulation Time
        public void SpeedUpTime()
        {
            if ((gameSpeed) + 1 <= 5)
            {
                gameSpeed += 1;
                mainController.userInterface.GameSpeed = (gameSpeed).ToString();
            }
        }
        #endregion

        #region Speed Down Simulation Time
        public void SpeedDownTime()
        {
            if (gameSpeed - 1 >= 0)
            {
                gameSpeed = gameSpeed - 1;
                mainController.userInterface.GameSpeed = (gameSpeed).ToString();
            }
        }
        #endregion

        #region Calculate Date Time
        // Calculates current day, hour minute and second from timeInSeconds in a date time format.
        private void CalculateDateTime()
        {
            /* Calculate day, hour, minute and second as a clock format. */
            day = Mathf.Floor(timeInSeconds / 86400);
            hour = Mathf.Floor((timeInSeconds - (day * 86400)) / 3600);
            minute = Mathf.Floor((timeInSeconds - (day * 86400) - (hour * 3600)) / 60);
            second = Mathf.Floor((timeInSeconds - (day * 86400) - (hour * 3600) - (minute * 60)));
        }
        #endregion
    }
}