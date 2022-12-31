using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCounter : MonoBehaviour
{
    int scrollCount = 0;
    int scrollToWin = 20;
    bool inCollider;

    void OnTriggerStay(Collider other){
        if(other.tag == "Scrolls"){
            if(Input.GetKeyDown(KeyCode.E)){
                scrollCount++;
                Debug.Log(scrollCount);
                Destroy(other.gameObject);
            }
        }
    }
}
