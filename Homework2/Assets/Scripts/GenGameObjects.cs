using UnityEngine;
using System.Collections;
using Com.MyGame;
using System.Collections.Generic;
 
public class GenGameObjects : MonoBehaviour {
    public List<GameObject> Priests, Devils;
    public GameObject boat, bankLeft, bankRight;
 
    private BoatBehaviour myBoatBehaviour;
 
    void Start () {
        Priests = new List<GameObject>();
        for (int i = 0; i < 3; i++) {
            GameObject priests = (GameObject)Instantiate(Resources.Load("priest"));
            priests.name = "Priest " + (i + 1);
            priests.tag = "Priest";
            priests.AddComponent<PersonStatus>();
            Priests.Add(priests);
        }
        Priests[0].transform.position = LOCATION_SET.priests_1_LOC;
        Priests[1].transform.position = LOCATION_SET.priests_2_LOC;
        Priests[2].transform.position = LOCATION_SET.priests_3_LOC;
 
        Devils = new List<GameObject>();
        for (int i = 0; i < 3; i++) {
            GameObject devils = (GameObject)Instantiate(Resources.Load("evil"));
            devils.name = "Devil " + (i + 1);
            devils.tag = "Devil";
            devils.AddComponent<PersonStatus>();

            Devils.Add(devils);
        }
        Devils[0].transform.position = LOCATION_SET.devils_1_LOC;
        Devils[1].transform.position = LOCATION_SET.devils_2_LOC;
        Devils[2].transform.position = LOCATION_SET.devils_3_LOC;
 
        boat = GameObject.CreatePrimitive(PrimitiveType.Cube);//用prefab时会出错
        boat.name = "Boat";
        boat.AddComponent<BoatBehaviour>();
        myBoatBehaviour = boat.GetComponent<BoatBehaviour>();
        boat.transform.localScale = new Vector3(3, 1, 1);
        boat.transform.position = LOCATION_SET.boat_right_LOC;
 
        bankLeft = (GameObject)Instantiate(Resources.Load("soil"));
        bankLeft.name = "BankLeft";
        bankLeft.transform.Rotate(new Vector3(0, 0, 90));
        bankLeft.transform.localScale = new Vector3(1, 1, 7);
        bankLeft.transform.position = LOCATION_SET.bank_left_LOC;
 
        bankRight = (GameObject)Instantiate(Resources.Load("soil"));
        bankRight.name = "BankRight";
        bankRight.transform.Rotate(new Vector3(0, 0, 90));
        bankRight.transform.localScale = new Vector3(1, 1, 7);
        bankRight.transform.position = LOCATION_SET.bank_right_LOC;
 
        mainSceneController.getInstance().setGenGameObjects(this);
    }
	
	void Update () {
	
	}
 
    public void boatMove() {
        myBoatBehaviour.setBoatMove();
    }
 
    //牧师上船
    public void priestsGetOn() {
        if (myBoatBehaviour.isMoving)
            return;
 
        if (!myBoatBehaviour.isBoatAtLeftSide()) {  
            for (int i = 0; i < Priests.Count; i++) {
                if (Priests[i].GetComponent<PersonStatus>().onBankRight) {
                    detectEmptySeat(true, i, DIRECTION.Right);
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < Priests.Count; i++) {
                if (Priests[i].GetComponent<PersonStatus>().onBankLeft) {
                    detectEmptySeat(true, i, DIRECTION.Left);
                    break;
                }
            }
        }
    }
    //恶魔上船
    public void devilsGetOn() {
        if (myBoatBehaviour.isMoving)
            return;
 
        if (!myBoatBehaviour.isBoatAtLeftSide()) {
            for (int i = 0; i < Devils.Count; i++) {
                if (Devils[i].GetComponent<PersonStatus>().onBankRight) {
                    detectEmptySeat(false, i, DIRECTION.Right);
                    break;
                }
            }
        }
        else { 
            for (int i = 0; i < Devils.Count; i++) {
                if (Devils[i].GetComponent<PersonStatus>().onBankLeft) {
                    detectEmptySeat(false, i, DIRECTION.Left);
                    break;
                }
            }
        }
    }
    void detectEmptySeat(bool isPriests, int index, bool boatDir) {
        if (myBoatBehaviour.isLeftSeatEmpty()) {        
            seatThePersonAndModifyBoat(isPriests, index, boatDir, DIRECTION.Left);
        }
        else if (myBoatBehaviour.isRightSeatEmpty()) { 
            seatThePersonAndModifyBoat(isPriests, index, boatDir, DIRECTION.Right);
        }
    }
    void seatThePersonAndModifyBoat(bool isPriests, int index, bool boatDir, bool seatDir) {
        if (isPriests) {
            Priests[index].GetComponent<PersonStatus>().personSeatOnBoat(boatDir, seatDir);
            Priests[index].transform.parent = boat.transform;
        }
        else {
            Devils[index].GetComponent<PersonStatus>().personSeatOnBoat(boatDir, seatDir);
            Devils[index].transform.parent = boat.transform;
        }
        myBoatBehaviour.seatOnPos(seatDir);
    }
    public void priestsGetOff() {
        if (myBoatBehaviour.isMoving)
            return;
 
        if (!myBoatBehaviour.isBoatAtLeftSide()) { 
            for (int i = Priests.Count - 1; i >= 0; i--) {
                if (detectIfPeopleOnBoat(true, i, DIRECTION.Right))
                    break;
            }
        }
        else {
            for (int i = Priests.Count - 1; i >= 0; i--) {
                if (detectIfPeopleOnBoat(true, i, DIRECTION.Left))
                    break;
            }
        }
    }
    public void devilsGetOff() {
        if (myBoatBehaviour.isMoving)
            return;
        if (!myBoatBehaviour.isBoatAtLeftSide()) {  
            for (int i = Devils.Count - 1; i >= 0; i--) {
                if (detectIfPeopleOnBoat(false, i, DIRECTION.Right))
                    break;
            }
        }
        else { 
            for (int i = Devils.Count - 1; i >= 0; i--) {
                if (detectIfPeopleOnBoat(false, i, DIRECTION.Left))
                    break;
            }
        }
    }
    bool detectIfPeopleOnBoat(bool isPriests, int i, bool boatDir) {
        if (isPriests) {
            if (Priests[i].GetComponent<PersonStatus>().onBoatLeft
            || Priests[i].GetComponent<PersonStatus>().onBoatRight) {
                myBoatBehaviour.jumpOutOfPos(Priests[i].GetComponent<PersonStatus>().onBoatLeft);
                Priests[i].GetComponent<PersonStatus>().landTheBank(boatDir);
                Priests[i].transform.parent = boat.transform.parent;
                return true;
            }
            return false;
        }
        else {
            if (Devils[i].GetComponent<PersonStatus>().onBoatLeft
            || Devils[i].GetComponent<PersonStatus>().onBoatRight) {
                myBoatBehaviour.jumpOutOfPos(Devils[i].GetComponent<PersonStatus>().onBoatLeft);
                Devils[i].GetComponent<PersonStatus>().landTheBank(boatDir);
                Devils[i].transform.parent = boat.transform.parent;
 
                return true;
            }
            return false;
        }
    }
 
}
