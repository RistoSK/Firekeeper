using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPartsManager : MonoBehaviour
{
    [Header("Spaceship Part Properties")]
    [SerializeField] public static float particleDuration = 1f;

    [Header("Spaceship Part Manager")]
    [SerializeField] private int _spaceshipPartsAmount;
    [SerializeField] private float _notSpawnableArea;
    [SerializeField] private float _xSpaceshipPositionAxisRange;
    [SerializeField] private float _zSpaceshipPositionAxisRange;

    [SerializeField] private List<GameObject> _spaceshipParts;
    [SerializeField] private ResourceManager _resourceManager;

    private void Start()
    {
        _resourceManager.SetMaxSpaceshipParts(_spaceshipPartsAmount);

        for (int loop = 0; loop < _spaceshipPartsAmount; loop++)
        {
            Vector3 position = Vector3.zero;
            position.y = 1f;
            bool invalid = true;
            int tries = 0;

            while (invalid)
            {
                tries++;
                position.x = Random.Range(-_xSpaceshipPositionAxisRange, _xSpaceshipPositionAxisRange);

                position.z = Random.Range(-_zSpaceshipPositionAxisRange, _zSpaceshipPositionAxisRange);
                if (position.x > _notSpawnableArea || position.x < -_notSpawnableArea)
                {
                    if (position.z > _notSpawnableArea || position.z < -_notSpawnableArea)
                    {
                        invalid = false;
                    }
                }
                if (tries > 100)
                {
                    invalid = false;
                }
            }
            int index = Random.Range(0, _spaceshipParts.Count);
            GameObject tree = Instantiate(_spaceshipParts[index], transform);
            tree.transform.position = position;
            Vector3 rotation = tree.transform.eulerAngles;
            rotation.y = Random.Range(0, 360);
            tree.transform.eulerAngles = rotation;
        }
    }
}
