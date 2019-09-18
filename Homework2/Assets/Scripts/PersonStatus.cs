using UnityEngine;
using System.Collections;
using Com.MyGame;
 
public class PersonStatus : MonoBehaviour {
    private Vector3 originalPos;
    public bool onBoatLeft, onBoatRight;
    public bool onBankLeft, onBankRight;
 
    private IGameJudge gameJudge;
 
    void Start () {
        originalPos = this.transform.position;
        onBoatLeft = false;
        onBoatRight = false;
        onBankLeft = false;
        onBankRight = true;
 
        gameJudge = mainSceneController.getInstance() as IGameJudge;
    }
	
	void Update () {
	
	}
 
    //人上船（下岸）
    public void personSeatOnBoat(bool boatAtLeft, bool seatAtLeft) {
        if (seatAtLeft) {
            if (boatAtLeft)
                this.transform.position = LOCATION_SET.boatLeft_Pos_1;
            else
                this.transform.position = LOCATION_SET.boatRight_Pos_1;
            onBoatLeft = true;
        }
        else {
            if (boatAtLeft)
                this.transform.position = LOCATION_SET.boatLeft_Pos_2;
            else
                this.transform.position = LOCATION_SET.boatRight_Pos_2;
            onBoatRight = true;
        }
        onBankLeft = false;
        onBankRight = false;
 
        if (this.tag.Equals("Priest")) {
            gameJudge.modifyBoatPriestsNum(MODIFICATION.Add);
            gameJudge.modifyBankPriestsNum(boatAtLeft, MODIFICATION.Sub);
        }
        else {
            gameJudge.modifyBoatDevilsNum(MODIFICATION.Add);
            gameJudge.modifyBankDevilsNum(boatAtLeft, MODIFICATION.Sub);
        }
    }
 
    //人上岸（下船）
    public void landTheBank(bool boatAtLeft) {
        if (boatAtLeft) {
            this.transform.position = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
            onBankLeft = true;
        }
        else {
            this.transform.position = originalPos;
            onBankRight = true;
        }
        onBoatLeft = false;
        onBoatRight = false;
 
        if (this.tag.Equals("Priest")) {
            gameJudge.modifyBoatPriestsNum(MODIFICATION.Sub);
            gameJudge.modifyBankPriestsNum(boatAtLeft, MODIFICATION.Add);
        }
        else {
            gameJudge.modifyBoatDevilsNum(MODIFICATION.Sub);
            gameJudge.modifyBankDevilsNum(boatAtLeft, MODIFICATION.Add);
        }
    }
}
