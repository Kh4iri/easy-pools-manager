# Easy Pools Manager v1.0
Easy Pools Manager is a simple tool to manage your pools.

### What is Object Pooling?
Object Pooling is a way to optimize your projects and lower the burden that is placed on the CPU when having to rapidly create and destroy new objects. It is a good practice and design pattern to keep in mind to help relieve the processing power of the CPU to handle more important tasks and not become inundated by repetitive create and destroy calls.

### Requirements
- [Unity](https://unity.com/) **2021.1** or newer (this project uses Unity's [ObjectPool](https://docs.unity3d.com/2021.1/Documentation/ScriptReference/Pool.ObjectPool_1.html) API)

# Prefab Pool

![Prefab Pool](https://user-images.githubusercontent.com/99790424/174535455-49ad19b4-7f1a-4d41-81a9-f6410dc3a60c.png)

- **Collection Check**: Collection checks are performed when an instance is returned back to the pool. An exception will be thrown if the instance is already in the pool. Collection checks are only performed in the Editor.
- **Default Capacity**: The default capacity the stack will be created with.
- **Max Size**: The maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size.

Use `PrefabPool.Get()` API to get an instance from the pool.
```csharp
[SerializeField] private PrefabPool explosionPool;

private void Start()
{
    explosionPool.Get();
}
```

> Note: Actual pool is stored and managed in PoolsManager.

# Sample Scenes

https://user-images.githubusercontent.com/99790424/174527465-70abf41f-03ec-49aa-9f8b-f8319081e46a.mp4

Example Code:
```csharp
public class PoolableParticleSystem : PoolableObject
{
    /// <summary>
    /// OnParticleSystemStopped is called when all particles in the system have died, and no new particles will be born.
    /// This will only be called if the particle system's stopAction is set to Callback.
    /// </summary>
    private void OnParticleSystemStopped()
    {
        ReturnToPool();
    }
}
```

---

https://user-images.githubusercontent.com/99790424/174527701-2e799058-57e8-4f96-a7f0-30a73e7810c3.mp4

Example Codes:
```csharp
/// <summary>
/// Called when the bullet hits something.
/// </summary>
private void OnHit(RaycastHit2D hit)
{
    // Spawn impact effect and return to pool.
    impactFX.Get(hit.point, Quaternion.identity);
    ReturnToPool();
}
```

```csharp
// public class PoolableBullet : PoolableObject

/// <summary>
/// Called when the instance is taken from the pool. The base code is setting the game object to active.
/// </summary>
public override void OnGetFromPool(PoolableObject obj)
{
    base.OnGetFromPool(obj);

    PoolableBullet bullet = obj as PoolableBullet;
    bullet.ResetLifetime();
}
```

# Changelog

### v1.0
- Released