using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostOnTrigger : MonoBehaviour
{
    public float speed = 10f;
    public float drag = 1f;
    public float impulseForce = 100f;
    public float timeToLaunch = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement pl))
        {
            pl.m_rigidBody.velocity = Vector3.zero;
            pl.enabled = false;
            StartCoroutine(SetLaunch(pl.m_rigidBody));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement pl))
        {
            pl.enabled = true;
        }
    }

    private IEnumerator SetLaunch(Rigidbody toThrow)
    {
        
        while(!Extension.Mathematics.Approximately(toThrow.transform.rotation.y, transform.rotation.y, 0.05f) || !Extension.Mathematics.IsCentered(toThrow.transform, transform, 1.5f))
        {
            toThrow.transform.SetPositionAndRotation(
                Vector3.Lerp(toThrow.transform.position, new Vector3(transform.position.x, toThrow.transform.position.y, transform.position.z), drag * Time.fixedDeltaTime),
                Quaternion.RotateTowards(toThrow.transform.rotation, transform.rotation, speed * Time.fixedDeltaTime)
            );
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(LaunchIn(timeToLaunch, toThrow));
        yield return null;
    }

    private IEnumerator LaunchIn(float time, Rigidbody rb)
    {

        while (time > 0)
        {
            //Debug.Log(time);
            time -= Time.fixedDeltaTime; // this is because the default value of FixedUpdate is .2f
            yield return new WaitForFixedUpdate();
        }

        rb.AddForce(transform.forward * impulseForce, ForceMode.Impulse);
        yield return null;
    }

}
