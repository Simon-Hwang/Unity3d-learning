## 作业内容

-----
### **P&D 过河游戏智能帮助实现**
- 参考[提供网址](https://blog.csdn.net/kiloveyousmile/article/details/71727667)中的状态转化图，即下图状态转化机示意图：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/1.jpg)
- 根据题目示意，需要程序按照状态机的状态自动运行游戏，此时按照[作业三](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2)的```牧师与恶魔```源码，将部分代码修改即可。
- 用户界面中只保留```Next```按钮，并在此调用```AI_move()```函数，即：
```c
if (GUI.Button(new Rect(375, 350, btnWidth, btnHeight), "Next!")) {
            AI_move();
}
```
- 其中需要记录状态。建立结构体```AI```记录游戏两岸、船的状态，并建立枚举变量```Boataction```表示船的状态值，即:
```c
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
```
- 根据状态机的示意图，建立```AI_move```函数，具体根据所记录的结构体```AI```信息，处理不同情况下的转变，如当船在右侧时可能需要处理的状态，并调用先前的函数使得游戏对象运动（上/下船）。为使动作具有连续性，此时有三种运动需要连续，即```上船-开船-下船```，相应的代码如下：
```c
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
```
- 游戏运行[示意视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/demontration.mp4.mp4)