using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private int gridSize = 10; // Grid size in meters.
    [SerializeField]
    private Vector2 groundSize = new Vector2(1000, 1000); // Ground size to place grid lines along.
    [SerializeField]
    private LayerMask groundMask; // Groundmask of object for building to place to.
    [SerializeField]
    private GameObject linePrefab = null; // Lines to spawn for creating grids.
    private List<GameObject> gridLines = new List<GameObject>(); // Pointer list to spawned grid lines.
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Draw Grids
    public void DrawGrids()
    {
        GameObject currentLine;
        LineRenderer lineRenderer;


        int numberOfGridsX = Mathf.RoundToInt(groundSize.x / gridSize);
        int numberOfGridsZ = Mathf.RoundToInt(groundSize.y / gridSize);

        // Draw on x axis
        for (int x = -(numberOfGridsX / 2); x <= (numberOfGridsX / 2); x++)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(x * gridSize + (gridSize / 2), 0.25f, -(groundSize.x / 2)));
            lineRenderer.SetPosition(1, new Vector3(x * gridSize + (gridSize / 2), 0.25f, (groundSize.x / 2)));
            gridLines.Add(currentLine);
        }

        // Draw on z axis
        for (int z = -(numberOfGridsZ / 2); z <= (numberOfGridsZ / 2); z++)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-(groundSize.x / 2), 0.25f, z * gridSize + (gridSize / 2)));
            lineRenderer.SetPosition(1, new Vector3((groundSize.x / 2), 0.25f, z * gridSize + (gridSize / 2)));
            gridLines.Add(currentLine);
        }
    }
    #endregion

    #region Clear Grids
    public void ClearGrids()
    {
        foreach (GameObject line in gridLines)
        {
            Destroy(line);
        }
        gridLines.Clear();
    }
    #endregion
}
