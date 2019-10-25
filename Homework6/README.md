## 作业内容
### 智能巡逻兵
- 下载迷宫资源，迷宫如下。
 ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework6/images/maze.png)
 - 下载人物和动作资源，其中，玩家以及巡逻兵的预制图像分别如下，其中需要设置人物的```RigidBody```属性并在```Prop```预制上添加后面所述的碰撞脚本```Player Collide```即红框所示，否则无法产生碰撞。
 ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework6/images/player.png)
  ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework6/images/prop.png)
  - 设置人物动作对应动画，以行走为例，设置为循环动作，并添加到人物动画上
   ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework6/images/run.png)
  - 代码的```Basecode.cs```延用上次作业的管理器和控制器如```SSActionManager```。由于对象运动需要获取键盘输入，因此相应的```SSDirector```需要添加函数获取键盘输入的方向以此来控制对象。即
 ```c
public  int  getFPS()
{
    return  Application.targetFrameRate;
}
public  void  setFPS(int  fps)
{
    Application.targetFrameRate  =  fps;
}
 ```
 - ```Player```对象的运动操作是用户控制的，因此对应的```CCMoveToAction```的处理需要获得输入的参数而改变相应的方向，具体的代码如下：
 ```cs
 public  Vector3  target;
public  float  speed;
public  int  block;
private  CCMoveToAction() { }
public  static  CCMoveToAction  getAction(int  block,float  speed,Vector3  position)
{
    CCMoveToAction  action  =  ScriptableObject.CreateInstance<CCMoveToAction>();
    action.target  =  position;
    action.speed  =  speed;
    action.block  =  block;
    return  action;
}
public  override  void  Update()
{
    if (this.transform.position  ==  target)
    {
        destroy  =  true;
        CallBack.SSActionCallback(this);
    }
    this.transform.position  =  Vector3.MoveTowards(transform.position, target, speed  *  Time.deltaTime);
}
 ```
 - 而```Prop```对象运动方向由系统判断，当满足一定的条件即```Player```出现在```Prop```所在区域时，```Prop```的运动方向为```Player```所在位置，若不满足该条件，则随机方向赋值直至遇到障碍物。因此，```Prop```对象主要集中在```Update```中调用```MoveTowards```函数决定其运动方向，即
 ```cs
 public  override  void  Update()
{
    this.transform.position  =  Vector3.MoveTowards(transform.position, target.transform.position, speed  *  Time.deltaTime);
    Quaternion  rotation  =  Quaternion.LookRotation(target.transform.position  -  gameObject.transform.position, Vector3.up);
    gameObject.transform.rotation  =  rotation;
    if (gameObject.GetComponent<Prop>().follow_player  ==  false||transform.position  ==  target.transform.position)
    {
        destroy  =  true;
        CallBack.SSActionCallback(this);
    }
}
 ```
 - 上述所说的条件处理是需要监视```Player```对象属性，通过处理标志，当对象出现在特定区域时，设置变量，其中需要调用```OnTriggerEnter```函数处理区域标志以此决定```Prop```运动的方向，即：
 ```cs
 void  OnTriggerEnter(Collider  collider)
{
    if (collider.gameObject.tag  ==  "Player")
    {
        sceneController.SetPlayerArea(sign);
        GameEventManager.Instance.PlayerEscape();
    }
}
 ```
 - 对应的当```Prop```对象碰撞到```Player```时，调用```OnCollisionEnter```触发标记的改变，当监听到变化后，游戏终止。
 ```cs
 void  OnCollisionEnter(Collision  other)
{
    if (other.gameObject.tag  ==  "Player")
    {
        GameEventManager.Instance.PlayerGameover();
    }
}
 ```
 - 根据题目要求，需要采用工厂模式创建```Prop```巡逻兵对象，在此之前，需要创建```Prop.cs```用于创建相应的```Prop```对象。按照要求，需要添加```Rigidbody```属性并根据需求更改对象的最初运动方向和角度。
 ```cs
 private  void  Start()
{
    if (gameObject.GetComponent<Rigidbody>())
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation  =  true;
    }
}
void  Update()
{
    if (this.gameObject.transform.localEulerAngles.x  !=  0  ||  gameObject.transform.localEulerAngles.z  !=  0)
    {
        gameObject.transform.localEulerAngles  =  new  Vector3(0, gameObject.transform.localEulerAngles.y, 0);
    }
    if (gameObject.transform.position.y  !=  0)
    {
        gameObject.transform.position  =  new  Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }
}
 ```
 - 其后，模仿上述```DiskFactory```创建```PropFactory```工厂模式创建对象。初始化创建3*3的```Prop```对象后加入到字典中待用，即：
 ```cs
 private  Dictionary<int, GameObject> used  =  new  Dictionary<int, GameObject>();
int[] pos_x  = { -7, 1, 7 };
int[] pos_z  = { 8, 2, -8 };
public  Dictionary<int, GameObject> GetProp()
{
    for(int  i  =  0; i  <  3; i++)
    {
        for(int  j  =  0; j  <  3; j++)
        {
            GameObject  newProp  =  GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Prop"));
            newProp.AddComponent<Prop>();
            newProp.transform.position  =  new  Vector3(pos_x[j], 0, pos_z[i]);
            newProp.GetComponent<Prop>().block  =  i  *  3  +  j;
            newProp.SetActive(true);
            used.Add(i*3+j, newProp);
        }
    }
    return  used;
}
 ```
 - ```Player```对象运动过程中如果越过特定迷宫区域，则判定为逃脱成功，此时调用```ScoreChange```增加分数，如果发生碰撞则游戏结束。此时需要监视不同事件。
 ```cs
public  static  event  ScoreEvent  ScoreChange;
public  static  event  GameoverEvent  GameoverChange;
private  GameEventManager() { }
public  void  PlayerEscape()
    {
    if (ScoreChange  !=  null)
    {
        ScoreChange();
    }
} 
public  void  PlayerGameover()
{
    if (GameoverChange  !=  null)
    {
        GameoverChange();
    }
}
 ```
 - 按照需求，需要修改```FirsetSceneController.cs```。```Awake```函数中创建动作管理器且创建基本对象包含迷宫、巡逻者、Player。即
```cs
if(CCManager  ==  null) CCManager  =  gameObject.AddComponent<CCActionManager>();
if (player  ==  null  &&  allProp  ==  null)
{
    Instantiate(Resources.Load<GameObject>("Prefabs/Plane"), new  Vector3(0, 0, 0), Quaternion.identity);
    player  =  Instantiate(Resources.Load("Prefabs/Player"), new  Vector3(0, 0, 0), Quaternion.identity) as  GameObject;
    allProp  =  PF.GetProp();
}
if (player.GetComponent<Rigidbody>())
{
  player.GetComponent<Rigidbody>().freezeRotation  =  true;
}
```
- 而场景控制器还是订阅者，在初始化时将自身相应的事件处理函数提交给消息处理器，在相应事件发生时被自动调用。需要实现相应的接口函数。
```cs
public  bool  GetGameState()
{
    return  gameState;
}
public  void  SetPlayerArea(int  x)
{
    if (PlayerArea  !=  x  &&  gameState)
    {   allProp[PlayerArea].GetComponent<Animator>().SetBool("run", false);
        allProp[PlayerArea].GetComponent<Prop>().follow_player  =  false;
        PlayerArea  =  x;
    }
}
void  AddScore()
{
    if (gameState)
    {
        ++score;
        allProp[PlayerArea].GetComponent<Prop>().follow_player  =  true;
        CCManager.Tracert(allProp[PlayerArea], player);
        allProp[PlayerArea].GetComponent<Animator>().SetBool("run", true);
    }
}
void  Gameover()
{
    CCManager.StopAll();
    allProp[PlayerArea].GetComponent<Prop>().follow_player  =  false;
    player.GetComponent<Animator>().SetTrigger("death");
    gameState  =  false;
}

public  void  MovePlayer(float  translationX, float  translationZ)
{
    if (gameState&&player!=null)
    {
    if (translationX  !=  0  ||  translationZ  !=  0)
    {
        player.GetComponent<Animator>().SetBool("run", true);
    }
    else
    {
        player.GetComponent<Animator>().SetBool("run", false);
    }
        player.transform.Translate(0, 0, translationZ  *  4f  *  Time.deltaTime);
        player.transform.Rotate(0, translationX  *  50f  *  Time.deltaTime, 0);
    }
}
public  void  Restart()
{
    player.GetComponent<Animator>().Play("New State");
    PF.StopPatrol();
    gameState  =  true;
    score  =  0;
    player.transform.position  =  new  Vector3(0, 0, 0);
    allProp[PlayerArea].GetComponent<Prop>().follow_player  =  true;
    CCManager.Tracert(allProp[PlayerArea], player);
    foreach (GameObject  x  in  allProp.Values)
    {
        if (!x.GetComponent<Prop>().follow_player)
        {
            CCManager.GoAround(x);
        }
    }
}
```
- 设置完场景控制器后，创建```InterfaceGUI```设置界面，与上次作业的用户界面类似，获取开始按钮以及显示时间和分数。而后调用函数获取用户输入的方向键，相应的代码如下：
```cs
private  void  OnGUI()
{
    if(!ss) S  =  Time.time;
    GUI.Label(new  Rect(Screen.width  -160, 30, 150, 30),"Score: "  +  UserActionController.GetScore().ToString() +  " Time: "  + ((int)(Time.time  -  S)).ToString());
    if (ss)
    {
        if (!UserActionController.GetGameState())
        {
        ss  =  false;
        }
    }
    else
    {
        if (GUI.Button(new  Rect(Screen.width  /  2  -  30, Screen.height  /  2  -  30, 100, 50), "Start"))
        {
            ss  =  true;
            SceneController.LoadResources();
            S  =  Time.time;
            UserActionController.Restart();
        }
    }
}

private  void  Update()
{
    float  translationX  =  Input.GetAxis("Horizontal");
    float  translationZ  =  Input.GetAxis("Vertical");
    UserActionController.MovePlayer(translationX, translationZ);
}
```
- 参考[演示视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework6/demontration.mp4)