## 作业内容

[视频示范](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/demontration.mp4)

-----
- **准备Vuforia配置**
  - 由于```Unity```早期安装时未装```Vudoria```配件，因此此处重新安装```Unity```并在```component selection```部门勾选```Vuforia Augmented Reality Support```，即如下：
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/1.png)
  - 其后，由于版本更新问题，后续的步骤与教程有所出入。菜单栏中选中```Files->Project Settings->Player->XR Settings```勾选其中的```Vuforia Augmented Reality Supported```开启```Vuforia```服务
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/3.png)
  - 在重新安装```Unity```后，在```GameObject```中有```Vuforia Engine```选项，其中可导入```AR Camera```配件，由于无法截图，下图取自他人
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/2.png)
  - 选择上述生成的```AR Camera```组件，选择```Open Vuforia Engine configuration```选项，按照教程指导内容，将```Vuforia Database key```添加到```App License Key```处，其后相应的```Databases```处会自动导入对应的数据库，即下两图所示
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/4.png)
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/7.png)
  - 其后，在```Vuforia```处对相应的数据库进行```Download all```处理并选择```Unbity Editor```，获取对应的```unitypackage```包
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/5.png)
  - ```Assets->import package```导入相应的包，相应输出如下内容
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/6.png)
  - 由于版本更新，无需在官网上下载```Unbity Extension```扩展包，其相应的功能在导入```AR camera```后即自动导入完成。
 - **图片识别与建模**
   - 模型从```Asset Store```处下载即可
   - 从```Vuforia Engine```中加载```Camera Image```中的```Image Target```选项，并将相应的模型导入其中，调整```Camera```与模型和图片之间的大小关系到合适位置，即如下图所示，此时运行即可识别图片和生成对应模型
   ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/11.png)
 - **虚拟按钮**
   - 按照上述内容，在```Image Target```属性处展开```Advanced```选项即可添加```Add Virtual Button```添加虚拟按钮，此处添加两个并添加```3D->Plane```组件使得按钮在运行时可见，即
   ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/12.png)
   - 设置脚本信息，继承```IVirtualButtonEventHandler```类，此时需要重载```OnButtonPressed、OnButtonReleased```两个函数，此处设置两个按钮即使模型向左/右移动。
   ```c
       public class button : MonoBehaviour, IVirtualButtonEventHandler
    {
        // Start is called before the first frame update
        public Animator ani;
        public VirtualButtonBehaviour[] vbs;
        void Start()
        {
            vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            //VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
            for (int i = 0; i < vbs.Length; i++)
            {
                vbs[i].RegisterEventHandler(this);
            }
        }
  
      // Update is called once per frame
      void Update()
      {
          
      }
  
      public void OnButtonPressed(VirtualButtonBehaviour button)
      {
          if (button.VirtualButtonName == "button1")
          {
              ani.gameObject.transform.position += new Vector3(-1, 0, 0);
          }
          else {
              ani.gameObject.transform.position += new Vector3(1, 0, 0);
          }
          Debug.Log(button.VirtualButtonName);
      }
  
      public void OnButtonReleased(VirtualButtonBehaviour vb)
      {
  
      }
    }
    ```
  - 将脚本挂载到```Image Target```下并将对应模型作为参数传入，完成操作后即可进行虚拟按钮的操作
   ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/13.png)
   - 在操作过程中需要注意向虚拟按钮添加的```Plane```不宜过大，否则无法有效检测而不能正常运动，其输出示意图如下
 ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/14.png)
  ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/15.png)
   ![images](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework10/images/16.png)
