using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnRocksAbove : MonoBehaviour
{
    [SerializeField] BoxCollider spawnBox = null;
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] GameObject boulderToSpawn = null;
    [SerializeField] private int initialSpawn = 3;
    [SerializeField] float maxInitialTorque = 10;


    private IEnumerator Start()
    {
        Spawn(initialSpawn);
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Spawn(1);
        }
    }

    private void Spawn(int numBombs)
    {
        for (int i = 0; i < numBombs; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnBox.size.x, spawnBox.size.x),
                Random.Range(-spawnBox.size.y, spawnBox.size.y),
                Random.Range(-spawnBox.size.z, spawnBox.size.z)
            );
            spawnPosition = spawnBox.transform.TransformPoint(spawnPosition / 2);
            GameObject bomb = Instantiate(boulderToSpawn, spawnPosition, Quaternion.identity);
            bomb.transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            bomb.GetComponent<Rigidbody>().AddTorque(//add a small spin to it as well
                Random.onUnitSphere * Random.Range(0, maxInitialTorque),
                ForceMode.VelocityChange
            );
        }

    }


}
