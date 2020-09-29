using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesertSurvival;

namespace DesertSurvival
{
    public class Construction : MonoBehaviour
    {
        #region Properties

        #region Boolean Properties
        [HideInInspector]
        public bool isPlacementEnabled = false;
        [HideInInspector]
        public bool isReplacing = false;
        [HideInInspector]
        public bool canPlace = true; // Checker for if building is in a placeable place. 
        #endregion

        #region Pointers to building prefabs
        [SerializeField]
        private GameObject powerPlant = null;
        #endregion

        #region Main Controller
        private MainController mainController;
        #endregion
 
        #endregion

        private void Start()
        {
            mainController = GameObject.Find("MainController").GetComponent<MainController>();
        }

        // Update is called once per frame
        void Update()
        {
            OnLeftClick();
            TogglePlacement();
        }

        #region Get Mouse Position In 3D
        Vector3 GetMouseScreenPosition()
        {
            // get mouse position and assign it to property
            return new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        }
        #endregion

        #region Spawn Power Plant
        public void SpawnPowerPlant()
        {
            SpawnBuilding(powerPlant);
        }
        #endregion

        #region Spawn Building
        public void SpawnBuilding(GameObject buildingToSpawn)
        {
            mainController.selection.SelectedObject = Instantiate(buildingToSpawn, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            isPlacementEnabled = true;
            mainController.grid.DrawGrids();
        }
        #endregion

        #region Toggle Placement
        void TogglePlacement()
        {
            if (isPlacementEnabled)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(GetMouseScreenPosition()), out hitInfo))
                {
                    if (hitInfo.transform.gameObject.tag == "Ground")
                        mainController.selection.SelectedObject.transform.position = hitInfo.point - new Vector3(hitInfo.point.x % 10, 0f, hitInfo.point.z % 10);
                }

                CheckOverlap();
            }
            
        }
        #endregion

        #region Check Overlap
        // Checks if building is overlapping any other building. If yes does followings:
        // 1 - Prevent placement by setting canPlace to false.
        // 2 - Make selected game object invisible by set mesh renderer of object to false..
        void CheckOverlap()
        {
            if (mainController.selection.SelectedObject.GetComponent<Building>().IsOverlappingAnyBuilding())
            {
                mainController.selection.SelectedObject.GetComponent<Building>().OnOverlap();
                canPlace = false;
            }
            else
            {
                mainController.selection.SelectedObject.GetComponent<MeshRenderer>().enabled = true;
                canPlace = true;
            }
        }
        #endregion
    
        #region Move Building
        public void MoveBuilding()
        {
            isReplacing = true;
            mainController.userInterface.buildingInformationPanel.SetActive(false);
            isPlacementEnabled = true;
            mainController.grid.DrawGrids();
        }
        #endregion

        #region Check Mouse Hit
        void CheckMouseHit()
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(GetMouseScreenPosition()), out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == "Building")
                {
                    mainController.selection.SelectBuilding(hitInfo);
                }
                else
                {
                    mainController.selection.IsSelected = false;
                }
            }

        }
        #endregion

        #region Place Building
        void PlaceBuilding()
        {
            if (canPlace)
            {
                isPlacementEnabled = false;
                mainController.resources.buildings.Add(mainController.selection.SelectedObject);
                mainController.userInterface.buildingInformationPanel.SetActive(false);
                mainController.grid.ClearGrids();
                if (!isReplacing)
                    mainController.selection.SelectedObject.GetComponent<Building>().IsActive = true;
                else 
                    isReplacing = false;
            }
        }
        #endregion

        #region On Left Click
        void OnLeftClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isPlacementEnabled)
                    PlaceBuilding();
                else
                    CheckMouseHit();
            }
        }
        #endregion
    }
}
