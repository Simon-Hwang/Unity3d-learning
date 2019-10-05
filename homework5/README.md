
## 作业内容
- 改进飞碟（Hit UFO）游戏
- 根据题意，按照```Adapter```设计模式，只需要在[原有代码](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework4)的基础上，增添相关以物理引擎为运动基础的关于游戏对象的运动处理的类即可，此处即为在```CCActionManger```的基础上添加```CCPhysicesActionManager```表示物理引擎并添相应的物理动作```CCPhysicsAction```即可。
- 由于设计模式的要求，允许用户选择采用的引擎，因此需要使用一个接口类调用不同引擎下的类对象。
```c
public  interface  IActionManager
{
  void  MoveDisk(Disk  disk);
  bool  IsAllFinished();
}
```
- 仿照```CCActionManager```，由于二者运动的调用并无差别，代码相同，生成```CCPhysicsActionManager```.
```c
public  class  CCPhysisActionManager : SSActionManager, SSActionCallback, IActionManager  // server for physice move
{
  int  count  =  0;
  public  SSActionEventType  Complete  =  SSActionEventType.Completed;
  public  void  MoveDisk(Disk  disk)
  {
    count++;
    Complete  =  SSActionEventType.Started;
    CCPhysisAction  action  =  CCPhysisAction.getAction(disk.speed);
    addAction(disk.gameObject, action, this);
  }
  public  void  SSActionCallback(SSAction  source)
  {
    count--;
    Complete  =  SSActionEventType.Completed;
    source.gameObject.SetActive(false);
  }
  public  bool  IsAllFinished()
  {
    if (count  ==  0) return  true;
    else  return  false;
  }
}
```
- 由于引擎不同下，游戏对象的属性的改变方式不同，因此还需要额外建立```CCPhysicsAction```采用物理引擎改变游戏对象属性，此处采用```Rigidbody```来模拟物体物理落体运动过程。对于程序保留的物理引擎，无需在```Update```中更新属性，只需要在其中监视游戏对象是否需要移除即可，并在```start()```函数内添加引擎。
```c
public  override  void  Start ()
{
  if (!this.gameObject.GetComponent<Rigidbody>())
  {
    this.gameObject.AddComponent<Rigidbody>();// add rigidboy to add physics move
  }
  this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up  *  9.8f  *  0.6f, ForceMode.Acceleration); //physics mode
  this.gameObject.GetComponent<Rigidbody>().AddForce(new  Vector3(speedx, 0, 0), ForceMode.VelocityChange);
}
override  public  void  Update ()
{
  if (transform.position.y  <=  3)
  {
    Destroy(this.gameObject.GetComponent<Rigidbody>()); //remove rigidbody
    destroy  =  true;
    CallBack.SSActionCallback(this);
  }
}
```
- 在场景控制器中添加允许用户更改的```public```变量从而选择引擎来操作对象，由于此处只存在两种，采用```bool```类型即可，而后在Unity3d界面对应的脚本上更改相关值即可，示意图如下， [演示视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/homework5/demontration.mp4)
- 物理引擎：
 ![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/homework5/images/physics.png)
- 非物理引擎： 
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/homework5/images/not_physics.png)