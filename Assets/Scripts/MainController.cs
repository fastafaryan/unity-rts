using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesertSurvival;

namespace DesertSurvival
{
    public class MainController : MonoBehaviour
    {
        #region References to all scripts
        public UserInterface userInterface;
        public Resources resources;
        public Construction construction;
        public Time time;
        public Selection selection;
        public Grid grid;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            #region Set Controller References 
            userInterface = GameObject.Find("MainController").GetComponent<UserInterface>();
            resources = GameObject.Find("MainController").GetComponent<Resources>();
            construction = GameObject.Find("MainController").GetComponent<Construction>();
            time = GameObject.Find("MainController").GetComponent<Time>();
            selection = GameObject.Find("MainController").GetComponent<Selection>();
            grid = GameObject.Find("MainController").GetComponent<Grid>();
            #endregion
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
