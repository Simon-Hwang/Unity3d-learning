## 作业内容
1. 基本操作演练
  - 在AssetStore中下载Fantasy SkyBox Free并导入到项目中，其输出的图像如下所示
   ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/scene.png)
- 其中，点击Assets->create->Material，在新建项目的Inspector视图中选择Shader->Skybox->6Sided，出现如下内容，可以从导入的项目中选择相应图片分别填充，之后就能获得SkyBox，拉入到项目中即可应用，此处直接采用Fantasy SkyBox的成品
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/skybox.png)
- 将Demo拉入到项目中，选择平面，其后在Inspector视图中观察Terrian界面，有5个选项，分别为扩展平面、修改平面、增添树、增添细节、调整平面细节。此篇主讲中间3个内容，即:
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/create.png)
- 修改平面：
  - 选择```Raise or Loser Terrian```，按照提示内容，```左键```提高海拔，```左键+shift```降低海拔，其中调节功能栏能选择更多改变平面的功能，其中只展示提高海拔。
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/paint_before.png)
   ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/paint_after.png)
- 增添树
  - 选择```Paint Trees->Edit Trees```，在其后弹出的```Add Tree```框里选择```Tree Prefab```添加项目中的预设项目，如下图所示。
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/tree_before.png)
  -  其后通过更改```Settings```中的选项改变画圈大小可以设置增添树的范围，下图为输出样式：
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/tree_after.png)
- 增添细节
  - 增添细节的按钮与修改平面类似，只需要更改Details信息，如下图：
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/detail_before.png)
  - 其后选择范围增添即可：
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/detail_after.png)
2. 编程实践
- 牧师与魔鬼 动作分离版
  - 这次的作业区别与上周，需要用到动作管理器和动作控制器，先描述一下上周的代码：
   - 上周的牧师与恶魔中，恶魔、牧师、船这三个游戏对象，用了独立的类来实现操作，分别创建类对象后，通过用户与用户界面的交互调用不同对象的函数独立实现相对应的游戏对象的运动
   - 而本周的动作分离版，需要用到动作管理器和动作控制器，使用动作控制器控制所有游戏对象的运动样式，而用户通过用户界面交互通知动作管理器调用传入的控制器并实现其内的游戏对象的动作。即动作分离后由管理器、控制器两个类管理动作。
   - 建立管理器：
     ```c
      public  class  SSAction : ScriptableObject  
      {
          public  bool  enable  =  true;
          public  bool  destroy  =  false;
          public  GameObject  gameobject;
          public  Transform  transform;
          public  ISSActionCallback  callback;
          protected  SSAction() {}
          public  virtual  void  Start()
          {
              throw  new  System.NotImplementedException();
          }
          public  virtual  void  Update()
          {
              throw  new  System.NotImplementedException();
          }
      }
     ```
   - 建立管理器后，需要通过继承实现管理器需要处理的游戏对象的管理，如建立```SSMoveToAction```，该子类管理了游戏对象```船```的动作，代码如下：
      ```c
        public  class  SSMoveToAction : SSAction  //创建船移动的动作
          {
              public  Vector3  target;
              public  float  speed;
              private  SSMoveToAction(){}
              public  static  SSMoveToAction  GetSSAction(Vector3  target, float  speed)
              {
                  SSMoveToAction  action  =  ScriptableObject.CreateInstance<SSMoveToAction>();
                  action.target  =  target;
                  action.speed  =  speed;
                  return  action;
              }
              public  override  void  Update()
              {
                  this.transform.position  =  Vector3.MoveTowards(this.transform.position, target, speed*Time.deltaTime);
                  if (this.transform.position  ==  target)
                  {
                      this.destroy  =  true;
                      this.callback.SSActionEvent(this);
                  }
              }
              public  override  void  Start()
              {
              }
          }
      ```
    - 由于管理器需要控制多个游戏对象的运动，需要使用```SequenceAction```实现```SSAction, ISSActionCallback```，使得管理器可以按照序列依次实现传入到管理器中的动作。
    ```c
        public  interface  ISSActionCallback
      {
          void  SSActionEvent(SSAction  source, SSActionEventType  events  =  SSActionEventType.Competeted,
              int  intParam  =  0, string  strParam  =  null, Object  objectParam  =  null);
      }
        public  static  SequenceAction  GetSSAcition(int  repeat, int  start, List<SSAction> sequence)
          {
              SequenceAction  action  =  ScriptableObject.CreateInstance<SequenceAction>();
              action.repeat  =  repeat;
              action.sequence  =  sequence;
              action.start  =  start;
              return  action;
          }
          public  override  void  Update()
          {
              if (sequence.Count  ==  0) return;
              if (start  <  sequence.Count)
              {
                  sequence[start].Update();
              }
          }
          public  void  SSActionEvent(SSAction  source, SSActionEventType  events  =  SSActionEventType.Competeted,
              int  intParam  =  0, string  strParam  =  null, Object  objectParam  =  null)
          {
              source.destroy  =  false; 
              this.start++;
              if (this.start  >=  sequence.Count)
              {
                  this.start  =  0;
                  if (repeat  >  0) repeat--;
                  if (repeat  ==  0)
                  {
                      this.destroy  =  true;
                      this.callback.SSActionEvent(this); 
                  }
              }
          }
          public  override  void  Start()
          {
              foreach(SSAction  action  in  sequence)
              {
                  action.gameobject  =  this.gameobject;
                  action.transform  =  this.transform;
                  action.callback  =  this;
                  action.Start();
              }
          }
          void  OnDestroy()
          {
          }
      }
    ```
    - 上述代码为管理器做准备，因此还建立```SSActionManager```进行同一管理。其中，需要建立```动作列表```保存传入的需要完成的子类对象，即：
     ```c
      private  Dictionary<int, SSAction> actions  =  new  Dictionary<int, SSAction>(); 
      private  List<SSAction> waitingAdd  =  new  List<SSAction>(); //等待去执行的动作列表
      private  List<int> waitingDelete  =  new  List<int>();
     ```
     - 其后在```update```函数中依次处理位于```List<SSAction>```中的动作。而后续的控制器中的添加动作则是由```RunAction```函数负责条件到列表中，即：
    ```c
       public  void  RunAction(GameObject  gameobject, SSAction  action, ISSActionCallback  manager) //添加到动作到动作管理器的成员中 后由update自动执行列表中的动作并清空
      {
           action.gameobject  =  gameobject;
           action.transform  =  gameobject.transform;
           action.callback  =  manager;
           waitingAdd.Add(action);
           action.Start();
      }
     ```
   - 管理器建立完成后需要建立控制器负责对象的建立以及游戏对象动作的添加（实现由管理器完成）。而该类的具体实现同上周的游戏代码中的游戏对象类，只需要添加额外的成员变量```private  MyCharacterController[] passengerPlaner;```以供管理器去实现游戏对象动作。
   - 2019年新需求为裁判类，用于游戏结束后禁止所有动作，此处调用```Director```类，建立```cn_move```成员变量，用于在管理器与控制器中状态的判断，即:
     ```c
     public  class  Director: System.Object
      {
          public  static  int  cn_move  =  0;//0->move, 1->pause
          private  static  Director  _instance;
          public  SceneController  curren{ get; set;} //{get;set;}默认生成了SceneController curren变量
          public  static  Director  get_Instance(){
              if (_instance  ==  null)
              {
                  _instance  =  new  Director();
              }
              return  _instance;
          }
      }
     ```
    - 当管理器控制动作的过程中满足游戏结束的条件时，通过改变成员变量```if_win_or_not```告知```UserGUI```调用```pause```函数，将裁判类```Director```中的```cn_move```值置为1，而管理器在处理动作时发现```cn_move==1```时，暂停其所有动作。
        ```c
          public  void  pause(){
              Director.cn_move  =  1;
          }
        ```
     - 动作分离版本的核心是动作添加函数，当控制器发现动作时，调用管理器中的特定函数如```moveBoat```，由管理器本身调用函数```RunAction```将该动作添加到类本身的成员变量动作列表中
        ```c
        this.RunAction(boat, move_boat, this); //RunAction添加动作对象和执行传入的动作
        ```
- 游戏胜利后的结果：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/win.png) 
- 游戏输后的结果：
 ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/images/over.png)
 - 尝试在结束/胜利后点击事件，发现事件不发生任何动作，以此认为裁判类实现正确。
 [演示视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/demontration.mp4)
