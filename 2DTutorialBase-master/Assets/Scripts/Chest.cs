using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region GameObject_variables
    [SerializeField]
    [Tooltip("A potion")]
    private GameObject potion;
    #endregion

    #region Chest_functions

    IEnumerator DestroyChest()
    {
        yield return new WaitForSeconds(.3f);
        Instantiate(potion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        StartCoroutine("DestroyChest");
    }
    #endregion
}
