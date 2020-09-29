using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DesertSurvival;

namespace DesertSurvival
{
    public class Building : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private LayerMask buildingLayerMask;
        private Collider[] hitColliders = null;
        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        [SerializeField]
        private string buildingName = "Building";
        public string BuildingName
        {
            get { return buildingName; }
        }

        #region Materials
        [SerializeField]
        private Material defaultMaterial = null;
        [SerializeField]
        private Material selectedMaterial = null;
        [SerializeField]
        private Material overlapedMaterial = null;
        #endregion

        #region Production Per Game Unit Time In Megawatt(MW)
        [SerializeField]
        public double oilProduction = 0;
        [SerializeField]
        public double powerProduction = 0;
        [SerializeField]
        public double waterProduction = 0;
        [SerializeField]
        public double steelProduction = 0;
        #endregion

        #region Consumption Per Game Time Unit
        [SerializeField]
        public double oilConsumption = 0;
        [SerializeField]
        public double powerConsumption = 0;
        [SerializeField]
        public double waterConsumption = 0;
        [SerializeField]
        public double steelConsumption = 0;
        #endregion

        #region Controllers
        private DesertSurvival.Resources resourcesController;
        private Construction constructionController;
        private UserInterface userInterfaceController;
        #endregion
        #endregion

        #region Methods

        #region Unity Methods
        private void Start()
        {
            resourcesController = GameObject.Find("MainController").GetComponent<DesertSurvival.Resources>();
            constructionController = GameObject.Find("MainController").GetComponent<Construction>();
            userInterfaceController = GameObject.Find("MainController").GetComponent<UserInterface>();
        }
        private void Update()
        {
            UpdateResources();
        }
        #endregion

        #region Is Overlapping Any Building
        public bool IsOverlappingAnyBuilding()
        {
            if (hitColliders != null)
            {
                foreach (Collider hitCollider in hitColliders)
                {
                    hitCollider.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
            }

            hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, buildingLayerMask);

            if (hitColliders.Length > 1)
            {

                foreach (Collider hitCollider in hitColliders)
                {
                    hitCollider.gameObject.GetComponent<MeshRenderer>().material = overlapedMaterial;
                }
                GameObject.Find("MainController").GetComponent<UserInterface>().DisplayOnScreenMessage("Can't place here!");
                return true;
            }

            foreach (Collider hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
            }

            return false;
        }
        #endregion

        #region Set Default Material
        void SetDefaultMaterial()
        {
            GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        #endregion

        #region Set Selected Material
        void SetSelectedMaterial()
        {
            GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        #endregion

        #region Set Overlapped Material
        void SetOverlappedMaterial()
        {
            GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        #endregion

        #region OnOverlap 
        public void OnOverlap()
        {
            GetComponent<MeshRenderer>().material = overlapedMaterial; // set overlapped material
                                                                       //selectedBuilding.GetComponent<MeshRenderer>().enabled = false;
        }
        #endregion

        #region Update Resources
        private void UpdateResources()
        {
            if (isActive)
            {
                resourcesController.AddToOil(oilProduction - oilConsumption);
                resourcesController.AddToPower(powerProduction - powerConsumption);
                resourcesController.AddToWater(waterProduction - waterConsumption);
                resourcesController.AddToSteel(steelProduction - steelConsumption);
            }
        }
        #endregion

        #region Toggle Activity
        public bool toggleActivity()
        {
            isActive = !isActive;
            return isActive;
        }
        #endregion
        #endregion

    }

}
