using UnityEngine;
using System.Collections;
using Com.MyGame;
 
public enum Boataction{ZERO, P, D, PP, DD, PD}
//ZERO:船空载
//P：船运载一个牧师
//D：船运载一个恶魔
//PP：船运载两个牧师
//DD：船运载两个恶魔
//PD：船运载一个牧师，一个恶魔
public class AI{
    public int rightDevil;
    public int rightPriest;
    public int leftDevil;
    public int leftPriest;
    public bool bankStatus; // true 为右侧
    public bool boatStatus; //true为满载
    public bool hasMove; //结合boatStatus判断是移动还是需要上下船
    public Boataction boataction;
    public AI(){
        rightDevil = rightPriest = 3;
        leftDevil = leftPriest = 0;
        boatStatus = hasMove = false;
        bankStatus = true;
    }    
}
public class UserInterface : MonoBehaviour {
    IUserActions actions;
    float btnWidth = (float)Screen.width / 8.0f;
    float btnHeight = (float)Screen.height / 8.0f;
    AI AI_status;
    void Start () {
        actions = mainSceneController.getInstance() as IUserActions;
        AI_status = new AI();
    }
	
	void Update () {
	
	}
    
    void OnGUI() {
        // if (GUI.Button(new Rect(100, 350, btnWidth, btnHeight), "Priests GetOn")) {
        //     actions.priestsGetOn();
        // }
        // if (GUI.Button(new Rect(225, 350, btnWidth, btnHeight), "Priests GetOff")) {
        //     actions.priestsGetOff();
        // }
        if (GUI.Button(new Rect(375, 350, btnWidth, btnHeight), "Next!")) {
            AI_move();
        }
        // if (GUI.Button(new Rect(525, 350, btnWidth, btnHeight), "Devils GetOn")) {
        //     actions.devilsGetOn();
        // }
        // if (GUI.Button(new Rect(675, 350, btnWidth, btnHeight), "Devils GetOff")) {
        //     actions.devilsGetOff();
        // }
    }

    void AI_move(){
        if(AI_status.boatStatus){//true 船满载 等待出发
            actions.boatMove();
            AI_status.boatStatus = false;
            AI_status.bankStatus = !AI_status.bankStatus;
            AI_status.hasMove = true; //等待下船
        }else if(AI_status.hasMove){//true 上次动作为船移动 此时需要下船操作
            if(AI_status.bankStatus){//true 船在右侧    
                if(AI_status.boataction == Boataction.D && (AI_status.leftDevil == 2 && AI_status.leftPriest == 0)){
                    actions.devilsGetOff();
                    AI_status.rightDevil++;
                }else if(AI_status.boataction == Boataction.PD){
                    actions.devilsGetOff();
                    AI_status.rightDevil++;
                }
            }else{
                if(AI_status.boataction == Boataction.DD){
                    AI_status.boatStatus = true;//无需再上船
                    AI_status.boataction = Boataction.D;
                    if(AI_status.rightDevil + AI_status.rightPriest == 0){
                        actions.devilsGetOff();
                        AI_status.leftDevil++;
                        AI_status.boataction = Boataction.ZERO;
                        AI_status.boatStatus = false; //结束标志
                    }
                    actions.devilsGetOff();
                    AI_status.leftDevil++;
                }else if(AI_status.boataction == Boataction.PP){
                    if(AI_status.rightPriest == 0){
                        actions.priestsGetOff();
                        AI_status.leftPriest++;
                    }
                    AI_status.leftPriest++;
                    actions.priestsGetOff();
                }
            }
            AI_status.hasMove = false; //等待上船
        }else{ //处理上船操作
            if(AI_status.bankStatus){//右侧
                if(AI_status.boataction == Boataction.ZERO){
                    actions.devilsGetOn();
                    actions.devilsGetOn();
                    AI_status.rightDevil -= 2;
                    AI_status.boataction = Boataction.DD;
                }else if(AI_status.boataction == Boataction.D){
                    if(AI_status.leftDevil == 2 && AI_status.leftPriest == 0){
                        actions.priestsGetOn();
                        actions.priestsGetOn();
                        AI_status.rightPriest -= 2;
                        AI_status.boataction = Boataction.PP;
                    }else{
                        actions.devilsGetOn();
                        AI_status.rightDevil--;
                        AI_status.boataction = Boataction.DD;
                    }
                }else if(AI_status.boataction == Boataction.PD){
                    actions.priestsGetOn();
                    AI_status.rightPriest--;
                    AI_status.boataction = Boataction.PP;
                }
            }else{
                if(AI_status.boataction == Boataction.PP){
                    actions.devilsGetOn();
                    AI_status.leftDevil--;
                    if(AI_status.rightPriest == 0){
                        AI_status.boataction = Boataction.D;
                    }else{
                        AI_status.boataction = Boataction.PD;
                    }  
                }
            }
            AI_status.boatStatus = true; //等待开船
        }
    }
}
