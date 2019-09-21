## 作业内容
1. 简答题
- 游戏对象运动的本质是什么？
	-  游戏对象运动的本质是其对应的属性跟随着页面刷新而发生变化
- 请用三种方法以上方法，实现物体的抛物线运动
	-  对于物体的抛物线实现，具体而言就是通过一定的公式改变```transform.position```实现。由于```position```属性为```(x,y,z)```，需要对物体运动速度进行分解，大致为如下代码
    ```c
    void  Start()
    {
        init_speed  =  1f;
        x_speed  =  init_speed  *  Mathf.Cos(angle);
        y_speed  =  init_speed  *  Mathf.Sin(angle);//初始速度
    }
    ```
    - 此处采用了平抛公式，即```angle=0°```，而在```update```中只需要更新```y_speed  -=  gravity  *  Time.deltaTime```即可。
    - 初始完成后，对物体的```position```属性的更改有如下三种方法：
      1. 调用```transform.Translate()```函数，通过传入单位时间内变化的幅度为参数，该函数自动更改```position```属性
          ```c
          this.transform.Translate(new  Vector3(Time.deltaTime  *  x_speed, Time.deltaTime  *  y_speed, 0));
          ```
        2. 对```position```属性进行加法处理，与第一种类似，只是更改由用户本身实现
            ```c
              this.transform.position  +=  new  Vector3(Time.deltaTime  *  x_speed, Time.deltaTime  *  y_speed, 0);
             ```
       3. 调用```Vector3.Lerp()```函数，通过传入新旧```position```属性，实现两个坐标之间的平滑转变  
           ```c
           this.transform.position  =  Vector3.Lerp(this.transform.position, this.transform.position  +  new  Vector3(Time.deltaTime  *  x_speed, Time.deltaTime  *  y_speed, 0), 1);
           ```
       - 处理完成后，其效果图如下图所示，具体看演示视频。
       ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/parabola1.png)
       ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/parabola2.png)
- 实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上
  - 实现太阳系时，一开始没看清题意，采用了```GameObject.CreatePrimitive(PrimitiveType.Sphere)```方法创建星球，通过数组来分配各个星球的```position、localscale```属性，但后来发现需要对星球进行贴图，然而并不懂如何在代码中将贴图应用到对象上，因此只能老老实实的建立9个星球对象后逐个调用```Rotate、RotateAround```进行处理，代码如下：
    ```c
    public  Transform  moon;
    public  Transform  mercury;
    public  Transform  venus;
    public  Transform  earth;
    public  Transform  mars;
    public  Transform  jupiter;
    public  Transform  saturn;
    public  Transform  uranus;
    public  Transform  neptune;
    private  float[] pos_planets  = {2.11f, 3.23f,0.99f,4.34f,1.02f,0.98f,0.97f,0.96f};
    private  int[] ang_planets  = {47, 35,30,24,13,9,6,5};
    private  int[] spe_planets  = {300,280,250,220,180,160,150,140};
    void  Update()
    {
    mercury.Rotate (Vector3.up  *  spe_planets[0] *  Time.deltaTime);
    venus.Rotate (Vector3.up  *  spe_planets[1] *  Time.deltaTime);
    earth.Rotate (Vector3.up  *  spe_planets[2] *  Time.deltaTime);
    mars.Rotate (Vector3.up  *  spe_planets[3] *  Time.deltaTime);
    jupiter.Rotate (Vector3.up  *  spe_planets[4] *  Time.deltaTime);
    saturn.Rotate (Vector3.up  *  spe_planets[5] *  Time.deltaTime);
    uranus.Rotate (Vector3.up  *  spe_planets[6] *  Time.deltaTime);
    neptune.Rotate (Vector3.up  *  spe_planets[7] *  Time.deltaTime);
    
    mercury.RotateAround (this.transform.position, new  Vector3(0, pos_planets[0], 1), ang_planets[0] *  Time.deltaTime);
    venus.RotateAround (this.transform.position, new  Vector3(0, pos_planets[1], 1), ang_planets[1] *  Time.deltaTime);
    earth.RotateAround (this.transform.position, new  Vector3(0, pos_planets[2], 2), ang_planets[2] *  Time.deltaTime);
    mars.RotateAround (this.transform.position, new  Vector3(0, pos_planets[3], 5), ang_planets[3] *  Time.deltaTime);
    jupiter.RotateAround (this.transform.position, new  Vector3(0, pos_planets[4], 1), ang_planets[4] *  Time.deltaTime);
    saturn.RotateAround (this.transform.position, new  Vector3(0, pos_planets[5], 3), ang_planets[5] *  Time.deltaTime);
    uranus.RotateAround (this.transform.position, new  Vector3(0, pos_planets[6], 1), ang_planets[6] *  Time.deltaTime);
    neptune.RotateAround (this.transform.position, new  Vector3(0, pos_planets[7], 1), ang_planets[7] *  Time.deltaTime);
    
    moon.transform.RotateAround (earth.position, Vector3.up, 300  *  Time.deltaTime);
}
    ```
   - 实现完成后，其图示意如下图：
   ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/solar1.png)
   ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/solar2.png)
2. 编程题
    - **列出所有游戏对象：**
    	- 牧师、恶魔、船、河、左岸、右岸
    - **动作表**
   动作 | 条件 | 运动结果
   - | - | - 
   牧师/恶魔上船| 船静止 && 船有空位 && 岸上牧师/恶魔数量不为0 | 牧师/恶魔上船
   牧师/恶魔上船| 船静止 && 左/右岸有空位 && 船上牧师/恶魔数量不为0 | 牧师/恶魔下船
   开船| 船静止 && 船上至少有一个牧师/恶魔 | 船运动到对岸
    - 建立```BaseCode.cs```，定义接口、坐标等基础信息，建立```mainSceneController```类，实现接口，调用方法。其中``mainSceneController``类的代码如下：
    ```c
   public  class  mainSceneController : System.Object, IUserActions, IGameJudge {
      private  static  mainSceneController  instance;
      private  GenGameObjects  myGenGameObjects;

      private  int  BoatPriestsNum, BoatDevilsNum, BankLeftPriestsNum,BankRightPriestsNum, BankLeftDevilsNum, BankRightDevilsNum;
      public  static  mainSceneController  getInstance() {
          if (instance  ==  null)
          instance  =  new  mainSceneController();
          return  instance;
      }
      internal  void  setGenGameObjects(GenGameObjects  _myGenGameObjects) {
        if (myGenGameObjects  ==  null) {
            myGenGameObjects  =  _myGenGameObjects;
            BoatPriestsNum  =  BoatDevilsNum  =  BankLeftPriestsNum  =  BankLeftDevilsNum  =  0;
            BankRightPriestsNum  =  BankRightDevilsNum  =  3;
       }
      ……
      /*实现IUserActions接口*/
      ……

      ……
       /*实现IGameJudge接口*/
      ……
    }
   ```
    -  根据题意，建立```GenGameObjects.cs```挂载到```Main Camera```上负责生成对象，其中利用```BaseCode.cs```上的变量修改各个对象的```position```属性，并实现对对象的操作处理如```on、off```以及对状态的监视如```detect```顺带一提，对代码的修改中调用类似```(GameObject)Instantiate(Resources.Load("soil"))```利用```prefab```创建对象时，创建boat时由于部分函数实现的原因导致运行出错。代码中以部分对象的动作实现为例：
      ```c
    public  void  priestsGetOn() {
      if (myBoatBehaviour.isMoving)
        return;
      if (!myBoatBehaviour.isBoatAtLeftSide()) {
          for (int  i  =  0; i  <  Priests.Count; i++) {
              if (Priests[i].GetComponent<PersonStatus>().onBankRight) {
              detectEmptySeat(true, i, DIRECTION.Right);
              break;
              }
          }
      }
      else {
        for (int  i  =  0; i  <  Priests.Count; i++) {
          if (Priests[i].GetComponent<PersonStatus>().onBankLeft) {
          detectEmptySeat(true, i, DIRECTION.Left);
          break;
          }
        }
      }
    public  void  priestsGetOff() {
      if (myBoatBehaviour.isMoving)
          return;
      if (!myBoatBehaviour.isBoatAtLeftSide()) {
          for (int  i  =  Priests.Count  -  1; i  >=  0; i--) {
            if (detectIfPeopleOnBoat(true, i, DIRECTION.Right))
            break;
          }
      }
      else {
        for (int  i  =  Priests.Count  -  1; i  >=  0; i--) {
            if (detectIfPeopleOnBoat(true, i, DIRECTION.Left))
            break;
        }
      }
     ```
      - 根据题意，需要对人物的上下船行为做出处理，建立```PersonStatus.cs```具体是当且仅当船上有空位且不在运动时才能通过```position```属性改变对象属性使之实现上下船动作，以下船为例，当且仅当判断到船的状态时改变状态并修改相应属性：
     ```c
     public  void  landTheBank(bool  boatAtLeft) {
          if (boatAtLeft) {
            this.transform.position  =  new  Vector3(-originalPos.x, originalPos.y, originalPos.z);
            onBankLeft  =  true;
          }
          else {
            this.transform.position  =  originalPos;
            onBankRight  =  true;
          }
          onBoatLeft  =  false;
          onBoatRight  =  false;
          if (this.tag.Equals("Priest")) {
            gameJudge.modifyBoatPriestsNum(MODIFICATION.Sub);
            gameJudge.modifyBankPriestsNum(boatAtLeft, MODIFICATION.Add);
          }
          else {
            gameJudge.modifyBoatDevilsNum(MODIFICATION.Sub);
            gameJudge.modifyBankDevilsNum(boatAtLeft, MODIFICATION.Add);
          }
   }
     ```
     - 根据上述，需要对船本身状态进行管理，因此创建```BoatBehavior.cs```管理船的运动及状态，如船的移动、返回船的状态等，其中对船的移动涉及到对象```position```的改变，与上述太阳系的设计类似，判断船是否运动到指定坐标并给予更改属性
     ```c
     private  void  moveTheBoat() {
        if (isMoving) {
            if (!isMovingToEdge()) {
              this.transform.Translate(moveDir);
          }
        }
      }
     ```
     - 上述运动处理完成后，需要建立用户接口提供游戏服务，只需要建立```Button```并调用上述```mainSceneController```中实现的接口即可。
    ```c
    void  OnGUI() {
      if (GUI.Button(new  Rect(100, 350, btnWidth, btnHeight), "Priests GetOn")) {
          actions.priestsGetOn();
      }
      if (GUI.Button(new  Rect(225, 350, btnWidth, btnHeight), "Priests GetOff")) {
          actions.priestsGetOff();
      }
      if (GUI.Button(new  Rect(375, 350, btnWidth, btnHeight), "Go!")) {
          actions.boatMove();
      }
      if (GUI.Button(new  Rect(525, 350, btnWidth, btnHeight), "Devils GetOn")) {
          actions.devilsGetOn();
      }
      if (GUI.Button(new  Rect(675, 350, btnWidth, btnHeight), "Devils GetOff")) {
          actions.devilsGetOff();
      }
   }
    ```
    - 处理完成后，效果图如下图，但在代码的修改上，可能触及到某些函数导致最后的结果判断出错，能力上以及时间不上令我难以寻找错误，因此只能将就，准确的代码看本文最后链接。
    ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/game1.png)
    ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/images/game2.png)
    - 此题代码参考[CSDN博客](https://blog.csdn.net/qq_33000225/article/details/57086542)
    - 此作业[演示视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework2/demonstration.mp4)
   

