using UnityEngine;
using System.Collections;
using Com.MyGame;
 
public class BoatBehaviour : MonoBehaviour {
    private Vector3 moveDir = new Vector3(-0.1f, 0, 0);
    public bool isMoving;
    public bool atLeftSide;
    public bool leftPosEmpty, rightPosEmpty;
 
    private IGameJudge gameJudge;
 
    void Start () {
        isMoving = false;
        atLeftSide = DIRECTION.Right;
        leftPosEmpty = true;
        rightPosEmpty = true;
 
        gameJudge = mainSceneController.getInstance() as IGameJudge;
    }
	
	void Update () {
        moveTheBoat();
    }
 
    private void moveTheBoat() {
        if (isMoving) {
            if (!isMovingToEdge()) {
                this.transform.Translate(moveDir);
            }
        }
    }
    private bool isMovingToEdge() {
        if (moveDir.x < 0 && this.transform.position.x <= LOCATION_SET.boat_left_LOC.x) {  //向左，已到
            gameJudge.judgeTheGame(DIRECTION.Left);
            isMoving = false;
            atLeftSide = DIRECTION.Left;
            moveDir = new Vector3(-moveDir.x, 0, 0);
            return true;
        }
        else if (moveDir.x > 0 && this.transform.position.x >= LOCATION_SET.boat_right_LOC.x) {  //向右，已到
            gameJudge.judgeTheGame(DIRECTION.Right);
            isMoving = false;
            atLeftSide = DIRECTION.Right;
            moveDir = new Vector3(-moveDir.x, 0, 0);
            return true;
        }
        else { 
            return false;
        }
    }
 
    public void setBoatMove() {
        if (!isMoving && (!leftPosEmpty || !rightPosEmpty)) {
            isMoving = true;
        }
    }
    public bool isBoatAtLeftSide() {
        return atLeftSide;
    }
 
 
    public bool isLeftSeatEmpty() {
        return leftPosEmpty;
    }
    public bool isRightSeatEmpty() {
        return rightPosEmpty;
    }
 
 
    public void seatOnPos(bool isLeft) {
        if (isLeft)
            leftPosEmpty = false;
        else
            rightPosEmpty = false;
    }
    public void jumpOutOfPos(bool isLeft) {
        if (isLeft)
            leftPosEmpty = true;
        else
            rightPosEmpty = true;
    }
}
