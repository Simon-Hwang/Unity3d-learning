using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
 
namespace Com.MyGame {
    public class DIRECTION {
        public static bool Left = true;
        public static bool Right = false;
    }
    public class MODIFICATION {
        public static bool Add = true;
        public static bool Sub = false;
    }
    public class LOCATION_SET {
        public static Vector3 priests_1_LOC = new Vector3(5, 3, 0);
        public static Vector3 priests_2_LOC = new Vector3(6.3f, 3, 0);
        public static Vector3 priests_3_LOC = new Vector3(7.6f, 3, 0);
        public static Vector3 devils_1_LOC = new Vector3(9.5f, 3, 0);
        public static Vector3 devils_2_LOC = new Vector3(10.8f, 3, 0);
        public static Vector3 devils_3_LOC = new Vector3(12.1f, 3, 0);
        public static Vector3 boat_left_LOC = new Vector3(-3, 0, 0);
        public static Vector3 boat_right_LOC = new Vector3(3, 0, 0);
        public static Vector3 bank_left_LOC = new Vector3(-8.5f, 1.5f, 0);
        public static Vector3 bank_right_LOC = new Vector3(8.5f, 1.5f, 0);
        public static Vector3 boatLeft_Pos_1 = new Vector3(-3.7f, 1.5f, 0);
        public static Vector3 boatLeft_Pos_2 = new Vector3(-2.3f, 1.5f, 0);
        public static Vector3 boatRight_Pos_1 = new Vector3(2.3f, 1.5f, 0);
        public static Vector3 boatRight_Pos_2 = new Vector3(3.7f, 1.5f, 0);
    }
    public interface IUserActions {
        void boatMove();
        void priestsGetOn();
        void priestsGetOff();
        void devilsGetOn();
        void devilsGetOff();
    }
    public interface IGameJudge {
        void modifyBoatPriestsNum(bool isAdd);
        void modifyBoatDevilsNum(bool isAdd);
        void modifyBankPriestsNum(bool isLeftBank, bool isAdd);
        void modifyBankDevilsNum(bool isLeftBank, bool isAdd);
        void judgeTheGame(bool isBoatLeft);
    }
 
    public class mainSceneController : System.Object, IUserActions, IGameJudge {
        private static mainSceneController instance;
        private GenGameObjects myGenGameObjects;
        private int BoatPriestsNum, BoatDevilsNum, BankLeftPriestsNum,BankRightPriestsNum, BankLeftDevilsNum, BankRightDevilsNum;  
        public static mainSceneController getInstance() {
            if (instance == null)
                instance = new mainSceneController();
            return instance;
        }
        internal void setGenGameObjects(GenGameObjects _myGenGameObjects) {
            if (myGenGameObjects == null) {
                myGenGameObjects = _myGenGameObjects;
                BoatPriestsNum = BoatDevilsNum = BankLeftPriestsNum = BankLeftDevilsNum = 0;
                BankRightPriestsNum = BankRightDevilsNum = 3;
            }
        }
 
        public void boatMove() {
            myGenGameObjects.boatMove();
        }
 
        public void devilsGetOff() {
            myGenGameObjects.devilsGetOff();
        }
 
        public void devilsGetOn() {
            myGenGameObjects.devilsGetOn();
        }
 
        public void priestsGetOff() {
            myGenGameObjects.priestsGetOff();
        }
 
        public void priestsGetOn() {
            myGenGameObjects.priestsGetOn();
        }
 
        public void modifyBoatPriestsNum(bool isAdd) {
            if (isAdd)
                BoatPriestsNum++;
            else
                BoatPriestsNum--;
        }
 
        public void modifyBoatDevilsNum(bool isAdd) {
            if (isAdd)
                BoatDevilsNum++;
            else
                BoatDevilsNum--;
        }
        
        public void modifyBankDevilsNum(bool isLeftBank, bool isAdd) {
            if (isLeftBank) {
                if (isAdd)
                    BankLeftDevilsNum++;
                else
                    BankLeftDevilsNum--;
            }
            else {
                if (isAdd)
                    BankRightDevilsNum++;
                else
                    BankRightDevilsNum--;
            }
        }
 
        public void modifyBankPriestsNum(bool isLeftBank, bool isAdd) {
            if (isLeftBank) {
                if (isAdd)
                    BankLeftPriestsNum++;
                else
                    BankLeftPriestsNum--;
            }
            else {
                if (isAdd)
                    BankRightPriestsNum++;
                else
                    BankRightPriestsNum--;
            }
        }
 
        public void judgeTheGame(bool isBoatLeft) {
            if (isBoatLeft) {
                if ((BankLeftPriestsNum + BoatPriestsNum > 0 
                    && BankLeftDevilsNum + BoatDevilsNum > BankLeftPriestsNum + BoatPriestsNum)
                    || (BankRightDevilsNum > BankRightPriestsNum && BankRightPriestsNum > 0)) {
                    showGameText("Failed !");
                }
 
                if (BankLeftDevilsNum + BoatDevilsNum == 3 && BankLeftPriestsNum + BoatPriestsNum == 3) {
                    showGameText("Victory !");
                }
            }
            else {
                if ((BankRightPriestsNum + BoatPriestsNum > 0
                    && BankRightDevilsNum + BoatDevilsNum > BankRightPriestsNum + BoatPriestsNum)
                    || (BankLeftDevilsNum > BankLeftPriestsNum && BankLeftPriestsNum > 0)) {
                    showGameText("Failed !");
                }
            }
        }
        void showGameText(string textContent) {
            GameObject Canvas = Camera.Instantiate(Resources.Load("Canvas")) as GameObject;
            GameObject GameText = Camera.Instantiate(Resources.Load("GameText"), Canvas.transform) as GameObject;
            GameText.transform.position = Canvas.transform.position;
            GameText.GetComponent<Text>().text = textContent;
        }
    }
}
