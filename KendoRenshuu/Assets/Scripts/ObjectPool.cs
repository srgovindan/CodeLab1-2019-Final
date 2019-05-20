using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Pool;
    
    public List<GameObject> SkeletonPool;
    public List<GameObject> WormPool;


    void Start()
    {
        //SINGLETON
        if (Pool == null)
        {
            Pool = this;
            DontDestroyOnLoad(Pool);
        }
        else
            Destroy(gameObject);

        SkeletonPool = new List<GameObject>();
        WormPool = new List<GameObject>();
    }


    public void AddSkeleton(GameObject skeleton)
    {
        SkeletonPool.Add(skeleton);
        skeleton.SetActive(false);
        skeleton.GetComponent<Enemy>().CurrentEnemyState = Enemy.EnemyState.Move; //reset the enemy state to move when it is reused
    }

    public GameObject GetSkeleton()
    {
        GameObject result = null;

        if (SkeletonPool.Count > 0) //if there are skeletons in the pool
        {
            result = SkeletonPool[0];
            SkeletonPool.Remove(result);
            result.SetActive(true);
        }
        else //no skeletons in the pool
        {
            result = Instantiate(Resources.Load<GameObject>("Prefabs/Skeleton"));
        }
        return result;
    }
    
    public void AddWorm(GameObject worm)
    {
        WormPool.Add(worm);
        worm.SetActive(false);
        worm.GetComponent<Enemy>().CurrentEnemyState = Enemy.EnemyState.Move; //reset the enemy state to move when it is reused
    }

    public GameObject GetWorm()
    {
        GameObject result = null;

        if (WormPool.Count > 0) //if there are worms in the pool
        {
            result = WormPool[0];
            WormPool.Remove(result);
            result.SetActive(true);
        }
        else //no worms in the pool
        {
            result = Instantiate(Resources.Load<GameObject>("Prefabs/Worm"));
        }
        return result;
    }
}
