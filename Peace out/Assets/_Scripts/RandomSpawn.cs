using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject startScroll;
    public GameObject mainHallScroll1;
    public GameObject mainHallScroll2;
    public GameObject mainHallScroll3;
    public GameObject mainHallScroll4;
    public GameObject mainHallScroll5;
    public GameObject mainHallScroll6;
    public GameObject RightWingScroll1;
    public GameObject RightWingScroll2;
    public GameObject RightWingScroll3;
    public GameObject RightWingScroll4;
    public GameObject RightWingScroll5;
    public GameObject RightWingScroll6;
    public GameObject RightWingJailsScroll1;
    public GameObject RightWingJailsScroll2;
    public GameObject RightWingJailsScroll3;
    public GameObject RightWingJailsScroll4;
    public GameObject RightWingJailsScroll5;
    public GameObject RightWingJailsScroll6;
    public GameObject LowerLeftWingScroll1;
    public GameObject LowerLeftWingScroll2;
    public GameObject LowerLeftWingScroll3;
    public GameObject LowerLeftWingScroll4;
    public GameObject LowerLeftWingScroll5;
    public GameObject LeftWingJailsScroll1;
    public GameObject LeftWingJailsScroll2;
    public GameObject LeftWingJailsScroll3;
    public GameObject LeftWingJailsScroll4;
    public GameObject LeftWingJailsScroll5;
    public GameObject UpperLeftWingScroll1;
    public GameObject UpperLeftWingScroll2;
    public GameObject UpperLeftWingScroll3;
    public GameObject UpperLeftWingScroll4;
    public GameObject UpperLeftWingScroll5;
    public GameObject UpperLeftWingScroll6;
    public GameObject UpperLeftWingScroll7;
    public GameObject UpperLeftWingScroll8;
    List<GameObject> MHScrolls = new List<GameObject>();
    List<GameObject> RWScrolls = new List<GameObject>();
    List<GameObject> RWJScrolls = new List<GameObject>();
    List<GameObject> LLWScrolls = new List<GameObject>();
    List<GameObject> LWJScrolls = new List<GameObject>();
    List<GameObject> ULWScrolls = new List<GameObject>();
    bool startOfGame = true;

    // Start is called before the first frame update
    void Start()
    {
        startScroll.SetActive(true);
        MHScrolls.Add(mainHallScroll1);
        MHScrolls.Add(mainHallScroll2);
        MHScrolls.Add(mainHallScroll3);
        MHScrolls.Add(mainHallScroll4);
        MHScrolls.Add(mainHallScroll5);
        MHScrolls.Add(mainHallScroll6);
        RWScrolls.Add(RightWingScroll1);
        RWScrolls.Add(RightWingScroll2);
        RWScrolls.Add(RightWingScroll3);
        RWScrolls.Add(RightWingScroll4);
        RWScrolls.Add(RightWingScroll5);
        RWScrolls.Add(RightWingScroll6);
        RWJScrolls.Add(RightWingJailsScroll1);
        RWJScrolls.Add(RightWingJailsScroll2);
        RWJScrolls.Add(RightWingJailsScroll3);
        RWJScrolls.Add(RightWingJailsScroll4);
        RWJScrolls.Add(RightWingJailsScroll5);
        RWJScrolls.Add(RightWingJailsScroll6);
        LLWScrolls.Add(LowerLeftWingScroll1);
        LLWScrolls.Add(LowerLeftWingScroll2);
        LLWScrolls.Add(LowerLeftWingScroll3);
        LLWScrolls.Add(LowerLeftWingScroll4);
        LLWScrolls.Add(LowerLeftWingScroll5);
        LWJScrolls.Add(LeftWingJailsScroll1);
        LWJScrolls.Add(LeftWingJailsScroll2);
        LWJScrolls.Add(LeftWingJailsScroll3);
        LWJScrolls.Add(LeftWingJailsScroll4);
        LWJScrolls.Add(LeftWingJailsScroll5);
        ULWScrolls.Add(UpperLeftWingScroll1);
        ULWScrolls.Add(UpperLeftWingScroll2);
        ULWScrolls.Add(UpperLeftWingScroll3);
        ULWScrolls.Add(UpperLeftWingScroll4);
        ULWScrolls.Add(UpperLeftWingScroll5);
        ULWScrolls.Add(UpperLeftWingScroll6);
        ULWScrolls.Add(UpperLeftWingScroll7);
        ULWScrolls.Add(UpperLeftWingScroll8);
    }

    void Update(){
        if(startScroll == null & startOfGame){
            spawnScrolls();
            startOfGame = false;
        }
    }

    void spawnScrolls(){
        pickMHScrolls();
        pickRWScrolls();
        pickRWJScrolls();
        pickLLWScrolls();
        pickLWJScrolls();
        pickULWScrolls();
    }

    void pickMHScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, MHScrolls.Count);
        MHScrolls[s1].SetActive(true);
        MHScrolls.RemoveAt(s1);
        int s2 = random.Next(0, MHScrolls.Count);
        MHScrolls[s2].SetActive(true);
        MHScrolls.RemoveAt(s2);
        int s3 = random.Next(0, MHScrolls.Count);
        MHScrolls[s3].SetActive(true);
        MHScrolls.RemoveAt(s3);
    }

    void pickRWScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, RWScrolls.Count);
        RWScrolls[s1].SetActive(true);
        RWScrolls.RemoveAt(s1);
        int s2 = random.Next(0, RWScrolls.Count);
        RWScrolls[s2].SetActive(true);
        RWScrolls.RemoveAt(s2);
        int s3 = random.Next(0, RWScrolls.Count);
        RWScrolls[s3].SetActive(true);
        RWScrolls.RemoveAt(s3);
    }

    void pickRWJScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, RWJScrolls.Count);
        RWJScrolls[s1].SetActive(true);
        RWJScrolls.RemoveAt(s1);
        int s2 = random.Next(0, RWJScrolls.Count);
        RWJScrolls[s2].SetActive(true);
        RWJScrolls.RemoveAt(s2);
        int s3 = random.Next(0, RWJScrolls.Count);
        RWJScrolls[s3].SetActive(true);
        RWJScrolls.RemoveAt(s3);
    }

    void pickLLWScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, LLWScrolls.Count);
        LLWScrolls[s1].SetActive(true);
        LLWScrolls.RemoveAt(s1);
        int s2 = random.Next(0, LLWScrolls.Count);
        LLWScrolls[s2].SetActive(true);
        LLWScrolls.RemoveAt(s2);
        int s3 = random.Next(0, LLWScrolls.Count);
        LLWScrolls[s3].SetActive(true);
        LLWScrolls.RemoveAt(s3);
    }

    void pickLWJScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, LWJScrolls.Count);
        LWJScrolls[s1].SetActive(true);
        LWJScrolls.RemoveAt(s1);
        int s2 = random.Next(0, LWJScrolls.Count);
        LWJScrolls[s2].SetActive(true);
        LWJScrolls.RemoveAt(s2);
        int s3 = random.Next(0, LWJScrolls.Count);
        LWJScrolls[s3].SetActive(true);
        LWJScrolls.RemoveAt(s3);
    }

    void pickULWScrolls(){
        System.Random random = new System.Random();
        int s1 = random.Next(0, ULWScrolls.Count);
        ULWScrolls[s1].SetActive(true);
        ULWScrolls.RemoveAt(s1);
        int s2 = random.Next(0, ULWScrolls.Count);
        ULWScrolls[s2].SetActive(true);
        ULWScrolls.RemoveAt(s2);
        int s3 = random.Next(0, ULWScrolls.Count);
        ULWScrolls[s3].SetActive(true);
        ULWScrolls.RemoveAt(s3);
        int s4 = random.Next(0, ULWScrolls.Count);
        ULWScrolls[s4].SetActive(true);
        ULWScrolls.RemoveAt(s4);
        int s5 = random.Next(0, ULWScrolls.Count);
        ULWScrolls[s5].SetActive(true);
        ULWScrolls.RemoveAt(s5);
    }
}
