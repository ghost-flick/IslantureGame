using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject lizard;
    [SerializeField] private FinalQuest finalQuest;
    private int counter;
    void Start()
    {
        StartCoroutine(SpawnLizards());
    }

    // Update is called once per frame
    public IEnumerator SpawnLizards()
    {
        while (counter < 25)
        {
            counter += 1;
            var liz = Instantiate(lizard, transform.position, Quaternion.identity);
            var lizObj = liz.GetComponent<Lizard>();
            lizObj.eventOnDeath.AddListener(finalQuest.UpdateProgress);
            yield return new WaitForSeconds(10);
        }
    }
}
