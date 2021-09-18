using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    #region SpeedPotion_variables
    [SerializeField]
    [Tooltip("the amount the player increases their speed by")]
    private int speedamount;
    #endregion

    #region Speed_functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().increaseSpeed(speedamount);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
