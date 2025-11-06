using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public LevelManager _level;
    public void OnSpawn(Vector2 direction, LevelManager level)
    {
        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
        _level = level;
        Invoke(nameof(DestroyThisObj), 2f);
    }

    private void DestroyThisObj()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _level.enemies.Remove(other.gameObject);
            _level.CheckLevelEnd();
            Destroy(other.gameObject);
        }

        DestroyThisObj();
    }
}
