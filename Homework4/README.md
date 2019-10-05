## 作业内容
- 编写一个简单的鼠标打飞碟（Hit UFO）游戏
- 构建游戏的基本```SSActionManager```和```SSAction```等代码直接使用上次的[牧师与恶魔](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework3/)中的相应代码即可，相应的飞碟动作类型的实现亦参考上述代码中的```SSAction、SSActionManager```子类的实现，其中只需要额外的记录飞碟的数量以确保游戏的正常结束。
- 按照题目要求，构建```DiskFactory```工厂类对飞碟进行处理，主要包含生成和释放游戏对象。由于飞碟运作的过程中，需要保存游戏对象直至被销毁或达到某种条件而消失，建立两个```STL```容器分别保存空闲和正在使用中的飞碟对象。
```c
private  Dictionary<int, Disk> used  =  new  Dictionary<int, Disk>();
private  List<Disk> free  =  new  List<Disk>();
```
- 通过函数```GetDisk()```获取飞碟对象，需要对游戏对象属性进行相关的赋值，其中包含根据```Round```而对对象的速度、颜色进行更改等，其中采用随机数随机赋值速度。生成游戏对象后将游戏对象应用到场景中即加入到使用容器中。
```c
newDisk.SetActive(true);
diskdata  =  newDisk.AddComponent<Disk>();// add detail
int  swith  =  Random.Range(round, round  *  2);
float  s  =  Random.Range(round  *  10, round  *  20);
float  RanX  =  UnityEngine.Random.Range(-1f, 1f) <  0  ?  -1  :  1;
diskdata.Direction  =  new  Vector3(RanX, 1, 0);
diskdata.StartPoint  =  new  Vector3(Random.Range(-110, -130), Random.Range(30,90), Random.Range(110,140));
diskdata.speed  =  round  *  5  +  s;
int  choose  =  round  %  3; // change the disk color according to round
switch (choose)
{
  case  1:
  {
    diskdata.color  =  Color.yellow;
    break;
  }
  case  2:
  {
    diskdata.color  =  Color.red;
    break;
  }
  case  0:
  {
    diskdata.color  =  Color.black;
    break;
  }
}
used.Add(diskdata.GetInstanceID(), diskdata); //添加到使用中
diskdata.name  =  diskdata.GetInstanceID().ToString();
```
- 由于```hit```中的封装函数```Destroy()```需要占用大量的内存，因此并不采用该函数释放对象，而是从上述的使用容器中移除相关的对象即可，后续的```CCManger```会根据相关的容器内对象更i性能游戏场景。因此，释放游戏对象只需要调用```Remove```函数即可。
```c
public  void  FreeDisk()
{
  foreach (Disk  x  in  used.Values)
  {
    if (!x.gameObject.activeSelf)
    {
      free.Add(x);
      used.Remove(x.GetInstanceID());
      return;
    }
  }
}
```
- 在场景控制器中，需要对鼠标点击的事件进行处理，其中利用课件中的```Hit```类监控点击，由于滞后性的问题，需要先将飞碟移除场景视线后下一个```Update```才能真正移除游戏对象。
```c
public  void  Hit(Vector3  pos) // hit then judge score add
{
  Ray  ray  =  Camera.main.ScreenPointToRay(pos);
  RaycastHit[] hits;
  hits  =  Physics.RaycastAll(ray);
  for (int  i  =  0; i  <  hits.Length; i++)
  {
    RaycastHit  hit  =  hits[i];
    if (hit.collider.gameObject.GetComponent<Disk>() !=  null)
    {
    Color  c  =  hit.collider.gameObject.GetComponent<Renderer>().material.color;
    if(c  ==  Color.yellow)
      score  +=  1  +  round  /  3;
    else  if(c  ==  Color.red)
      score  +=  2  +  round  /  3;
    else
      score  +=  3  +  round  /  3 ;
    hit.collider.gameObject.transform.position  =  new  Vector3(0, -5, 0);//not destroy, just to manager to free disk
  }
  }
}
```
- 根据需求，由于类对象都是采用静态类的方法获取对象，因此采用```Singleton.cs```模板类获取静态对象。
```c
public  class  Singleton<T> : MonoBehaviour  where  T : MonoBehaviour
{
  protected  static  T  instance;
  public  static  T  Instance
  {
    get
    {
    if (instance  ==  null)
    {
      instance  = (T)FindObjectOfType(typeof(T));
      if (instance  ==  null)
      {
        Debug.LogError("An instance of "  +  typeof(T)
        +  " is needed in the scene, but there is none.");
      }
    }
    return  instance;
    }
  }
}
```
- 设置用户界面```GUI```，显示游戏状态，此处采用10回合制，每回合飞碟颜色固定且以三回合为一轮轮换颜色，具体操作请看 [演示视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework4/demontration.mp4)