using UnityEngine;

public class EnemySettings : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _enemySpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDamage;

    [Header("Spawning")]
    [SerializeField] private bool _shouldSpawn = true;
    [SerializeField] private int _spawnAmount;
    [SerializeField] private float _minRadius;
    [SerializeField] private float _xEnemyPositionAxisRange;
    [SerializeField] private float _zEnemyPositionAxisRange;

    public static EnemySettings EnemySettingsInstance { get; private set; }

    public static float EnemySpeed => EnemySettingsInstance._enemySpeed;

    public static float AttackRange => EnemySettingsInstance._attackRange;

    public static float AttackCooldown => EnemySettingsInstance._attackCooldown;

    public static float AttackDamage => EnemySettingsInstance._attackDamage;

    public static bool ShouldSpawn => EnemySettingsInstance._shouldSpawn;

    public static float SpawnAmount => EnemySettingsInstance._spawnAmount;

    public static float MinRadius => EnemySettingsInstance._minRadius;

    public static float xPositionRange => EnemySettingsInstance._xEnemyPositionAxisRange;

    public static float zPositionRange => EnemySettingsInstance._zEnemyPositionAxisRange;

    private void Awake()
    {
        if (EnemySettingsInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            EnemySettingsInstance = this;
        }
    }
}
