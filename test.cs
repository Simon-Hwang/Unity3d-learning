using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private int[,] status = new int[3, 3];
    private int turn = 1;
    private int count = 0;
    private int result = 0;
    void Start()
    {
        restart();
    }
    void OnGUI (){
        chessboard();
        checkStatus();
    }
    void update(){
        
    }
    public void restart(){
        turn = 1;
        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                status[i,j] = 0;
            }
        }
        count = 0;
        result = 0;
    }
    void chessboard(){
        if(GUI.Button(new Rect(100, 300, 100, 50), "ReStart")){
            restart();
        }
        if(result != 0){
            string output = result == 1 ? "先手胜" : "后手胜";
            GUI.Label(new Rect(125, 350, 100, 100), output);
        }else if(count != 9){
            GUI.Label(new Rect(125, 350, 100, 100), "对局开始");
        }else{
            GUI.Label(new Rect(125, 350, 100, 100), "平局");
        }

        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                if(status[i, j] == 1){
                    GUI.Button(new Rect(100 * i, 100 * j, 100, 100), "O");
                }
                else if(status[i,j] == 2){
                    GUI.Button(new Rect(100 * i, 100 * j, 100, 100), "X");
                }
                else if(result != 0){
                    GUI.Button(new Rect(100 * i, 100 * j, 100, 100), "");
                }
                else if(GUI.Button(new Rect(100 * i, 100 * j, 100, 100), "")){
                    status[i,j] = turn;
                    turn = turn == 1 ? 2 : 1;
                    ++count;
                }
                
            }
        }
    }
    void checkStatus(){
        if(status[1, 1] != 0 && (status[1, 1] == status[0, 0] && status[1, 1] == status[2, 2]) || (status[1, 1] == status[2,0] && status[1,1] == status[0, 2])){
            result = status[1, 1];
        }
        for(int i = 0; i < 3; ++i){
            if(result != 0){
                break;
            }
            if(status[i, 0] != 0 && status[i, 0] == status[i, 1] && status[i, 1] == status[i, 2]){
                result = status[i, 0];
            }
            else if(status[0, i] != 0 && status[0, i] == status[1, i] && status[1, i] == status[2, i]){
                result = status[0,i];
            }
        }
        
    }
}
