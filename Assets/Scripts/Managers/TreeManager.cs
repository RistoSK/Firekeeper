using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private GameObject tree;
    [SerializeField] private int _treeSpawnAmountPerArea = 400;
    [SerializeField] private int _xMinimunPosition = 15;
    [SerializeField] private int _zMinimunPosition = 10;

    private HashSet<int> _treePositionsOnGrid = new HashSet<int>();
    private Vector3 _treePosition;

    private void Start()
    {
        int x;
        int z;

        for (int i = 0; i < _treeSpawnAmountPerArea; i++)
        {
            x = _xMinimunPosition;
            z = _zMinimunPosition;
            int gridPosition;

            while (true)
            {
                gridPosition = Random.Range(0, 900);

                if (!_treePositionsOnGrid.Contains(gridPosition))
                {
                    _treePositionsOnGrid.Add(gridPosition);
                    break;
                }
            }

            int xOffSet = Random.Range(-1, 1);
            int zOffSet = Random.Range(-1, 1);

            x += gridPosition % 30 * 5 + xOffSet;
            z += gridPosition / 30 * 5 + zOffSet;

            _treePosition = new Vector3(x, 0, z);

            GameObject tree1 = Instantiate(tree, transform);
            GameObject tree2 = Instantiate(tree, transform);
            GameObject tree3 = Instantiate(tree, transform);
            GameObject tree4 = Instantiate(tree, transform);

            tree1.transform.position = _treePosition;
            tree2.transform.position = new Vector3(-_treePosition.x, 0, _treePosition.z);
            tree3.transform.position = new Vector3(_treePosition.x, 0, -_treePosition.z);
            tree4.transform.position = new Vector3(-_treePosition.x, 0, -_treePosition.z);

            Vector3 rotation = tree1.transform.eulerAngles;
            rotation.y = Random.Range(0, 360);
            tree1.transform.eulerAngles = rotation;
            tree2.transform.eulerAngles = rotation;
            tree3.transform.eulerAngles = rotation;
            tree4.transform.eulerAngles = rotation;
        }  
    }
}
