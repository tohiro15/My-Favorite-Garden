using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FXPool : MonoBehaviour
{
    [SerializeField] private GameObject _fxPrefab;
    [SerializeField] private int _poolSize = 10;

    private List<GameObject> _pool = new();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_fxPrefab, transform);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }

    public GameObject GetFromPool(Vector3 position)
    {
        foreach (var fxObj in _pool)
        {
            if (!fxObj.activeInHierarchy)
            {
                fxObj.transform.position = position;
                fxObj.SetActive(true);

                ParticleSystem ps = fxObj.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    StartCoroutine(ReturnToPoolAfterDuration(fxObj, ps.main.duration + ps.main.startLifetime.constantMax));
                }
                else
                {
                    StartCoroutine(ReturnToPoolAfterDuration(fxObj, 2f));
                }

                return fxObj;
            }
        }

        GameObject newObj = Instantiate(_fxPrefab, position, Quaternion.identity, transform);
        _pool.Add(newObj);

        ParticleSystem newPs = newObj.GetComponent<ParticleSystem>();
        if (newPs != null)
        {
            newPs.Play();
            StartCoroutine(ReturnToPoolAfterDuration(newObj, newPs.main.duration + newPs.main.startLifetime.constantMax));
        }
        else
        {
            StartCoroutine(ReturnToPoolAfterDuration(newObj, 2f));
        }
        return newObj;
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDuration(GameObject fxObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        fxObj.SetActive(false);
    }

    public void DestroyParent(GameObject parent, Vector3 position)
    {
        StartCoroutine(DestroyWithFX(parent, position));
    }
    private IEnumerator DestroyWithFX(GameObject parent,Vector3 position)
    {
        GameObject fx = GetFromPool(position);
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();

        float duration = ps != null ?
            ps.main.duration + ps.main.startLifetime.constantMax :
            2f;

        parent.transform.DOScale(Vector3.zero, ps.main.duration + ps.main.startLifetime.constantMax).SetEase(Ease.InCirc).OnComplete(() => { parent.gameObject.SetActive(false); });

        yield return new WaitForSeconds(duration);

        Destroy(parent);
    }
}
