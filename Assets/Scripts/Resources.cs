using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DesertSurvival;

namespace DesertSurvival
{
    public class Resources : MonoBehaviour
    {
        #region Properties
        // Resources
        private double oil = 100; // in barrel
        private double power = 100; // in Megawatt(MW)
        private double water = 100; // in meters cube
        private double steel = 100; // in kilograms
        private int human = 0;

        public List<GameObject> buildings;
        private MainController mainController;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            mainController = GameObject.Find("MainController").GetComponent<MainController>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        #region Add to Oil
        public void AddToOil(double valueToAdd)
        {
            this.oil += mainController.time.GameSpeed * valueToAdd;
            mainController.userInterface.Oil = oil.ToString() + " Barrels";
        }
        #endregion

        #region Add to Power
        public void AddToPower(double valueToAdd)
        {
            this.power += mainController.time.GameSpeed * valueToAdd;
            mainController.userInterface.Power = power.ToString() + " MW";
        }
        #endregion

        #region Add to Water
        public void AddToWater(double valueToAdd)
        {
            mainController.userInterface.Water = water.ToString() + " m3";
            this.water += mainController.time.GameSpeed * valueToAdd;
        }
        #endregion

        #region Add to Steel
        public void AddToSteel(double valueToAdd)
        {
            this.steel += mainController.time.GameSpeed * valueToAdd;
            mainController.userInterface.Steel = steel.ToString() + " kg";
        }
        #endregion

        #region Add to Human
        public void AddToHuman(int valueToAdd)
        {
            this.human += mainController.time.GameSpeed * valueToAdd;
            mainController.userInterface.Human = human.ToString();
        }
        #endregion

        #region Destroy Building
        public void DestroyBuilding()
        {
            buildings.Remove(mainController.selection.SelectedObject);
            mainController.userInterface.buildingInformationPanel.SetActive(false);
            Destroy(mainController.selection.SelectedObject);
        }
        #endregion

        #region Disable Building
        public void DisableBuilding()
        {
            mainController.selection.SelectedObject.GetComponent<Building>().IsActive = false;
        }
        #endregion

    }
}