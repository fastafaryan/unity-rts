using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DesertSurvival;

namespace DesertSurvival
{
    public class Selection : MonoBehaviour
    {

        #region Properties
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private GameObject selectedObject;
        public GameObject SelectedObject
        {
            get { return selectedObject; }
            set { selectedObject = value; }
        }

        private Building selectedBuilding;
        public Building SelectedBuilding
        {
            get { return selectedBuilding; }
            set {
                selectedBuilding = value;
                mainController.userInterface.UpdateBuildingName(value.BuildingName);
            }
        }

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

        #region Select Building
        public void SelectBuilding(RaycastHit hitInfo)
        {
            // Display building informaiton UI.
            mainController.userInterface.buildingInformationPanel.SetActive(true);
            // Assign selected building into variable.
            SelectedObject = hitInfo.transform.gameObject;
            SelectedBuilding = SelectedObject.GetComponent<Building>();
            GameObject.Find("C.PowerProductionPerTime/Value").GetComponent<Text>().text = (SelectedObject.GetComponent<Building>().powerProduction * 1000).ToString() + " kWh";
            // Set checker to true.
            IsSelected = true;
        }
        #endregion

    }

}