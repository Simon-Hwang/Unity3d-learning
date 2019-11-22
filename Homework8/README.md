## 作业内容
### 背包系统
-----
- 点击[视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/demontration.mp4.mp4)查看演示视频
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/result.png)
- 建立背包系统前需要在当前界面下创建画布```Canvas```，并在该对象的```Inspector```下调节```Canvas```属性，在```Render Camera```中加入主摄像头以此达到画布主体随摄像头移动而同步移动，即如下图红框所示
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/1.png)
- 画布下需要建立再建立一层```Canvas```对象，才能在其上建立对象，并命名为```Inventory```
- 在其上建立```Panel```，并根据需求在```Inspector```界面下的```Add Component```中添加```Grid Layout Group```属性信息，在其上课设置信息使得```Panel```添加对象时自动分割成一定间隔的方框块。为显示方框见的间隔，需要额外添加```Image```属性，设置```color```后设置颜色变化，即可展示效果，配置图如下：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/2.png)
- 在上述的```Panel```下再次添加多个```Panel```对象，按照上述所提的设置信息，添加的对象自动排序并分割，其效果图如下所示：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/3.png)
- 建立了基本的UI界面后，添加```image```对象作为物品的形象。点击图片，进入```Inspector```界面，调节```Texture Type```属性为```Sprite(2D and UI)```，即下图所示：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/4.png)
- 图像设置好属性后，拖拽到```image```对象的```Source Image```框处，即下图所示
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework8/images/5.png)
- 此时，匹配好背包框和图像大小后，背包系统的UI界面设置完成，此时配置脚本信息。
- 为使背包系统随鼠标移动，需要对最先设置的```Canvas```对象添加脚本。通过获取鼠标信息，调用```Mathf.Clamp```计算坐标信息后调用```Lerp、Euler```函数实现面板的运动，相应的代码如下:
```cs
void  Update()
{
    Vector3  pos  =  Input.mousePosition; //get the mouse information
    float  halfWidth  =  Screen.width  *  0.5f;
    float  halfHeight  =  Screen.height  *  0.5f; // get the center of screen
    float  x  =  Mathf.Clamp((pos.x  -  halfWidth) /  halfWidth, -0.4f, 0.4f);
    float  y  =  Mathf.Clamp((pos.y  -  halfHeight) /  halfHeight, -0.4f, 0.4f);
    mRot  =  Vector2.Lerp(mRot, new  Vector2(x, y), Time.deltaTime  *  2f); // set the position
    trans.localRotation  =  startRot  *  Quaternion.Euler(-mRot.y  *  range.y, -mRot.x  *  range.x, 0f);
}
```
- 实现物品图像移动到物品栏中，需要对```image```对象添加脚本，具体是通过```OnDrag```等接口函数监听对图像的鼠标拖拽以及通过运动函数实现图像移动。当且仅当鼠标移动到物品栏所在区域即显示为```Panel```时，认为图像移动到该物品栏上，此时判断对象是否为```null```，若是，当鼠标松动即调用```OnEndDrag```时，将物品移动到对应物品栏上，否则返回默认位置，其对应的处理代码如下：
```cs
public  void  OnBeginDrag(PointerEventData  eventData)
{
    canvasGroup.blocksRaycasts  =  false;// event trigger ignore itself so that it can detect lowwer object
    lastEnter  =  eventData.pointerEnter;
    lastEnterNormalColor  =  lastEnter.GetComponent<Image>().color;
    originalPosition  =  trans.position;
    gameObject.transform.SetAsLastSibling();///sibling first
}

public  void  OnDrag(PointerEventData  eventData)
{
    Vector3  globalMousePos;
    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out  globalMousePos))
    {
        rectTransform.position  =  globalMousePos;
    }
    GameObject  curEnter  =  eventData.pointerEnter;
    bool  inItemGrid  =  EnterItemGrid(curEnter);
    if (inItemGrid)
    {
        Image  img  =  curEnter.GetComponent<Image>();
        lastEnter.GetComponent<Image>().color  =  lastEnterNormalColor;
        if (lastEnter  !=  curEnter)
        {
            lastEnter.GetComponent<Image>().color  =  lastEnterNormalColor;
            lastEnter  =  curEnter;//record current good
        }
        img.color  =  highLightColor; // highligh
    }
}

public  void  OnEndDrag(PointerEventData  eventData)
{
    GameObject  curEnter  =  eventData.pointerEnter;
    if (curEnter  ==  null)
    {
        trans.position  =  originalPosition; // reset
    }
    else
    {
        if (curEnter.name  ==  "Panel")// move to the bag
        {
            trans.position  =  curEnter.transform.position;
            originalPosition  =  trans.position;
            curEnter.GetComponent<Image>().color  =  lastEnterNormalColor;//recover its color
        }
        else
        {
            if (curEnter.name  ==  eventData.pointerDrag.name  &&  curEnter  !=  eventData.pointerDrag)
            {
                Vector3  targetPostion  =  curEnter.transform.position;
                curEnter.transform.position  =  originalPosition;
                trans.position  =  targetPostion;
                originalPosition  =  trans.position;
            }
            else//move to another bag
            {
                trans.position  =  originalPosition;
            }
        }
    }
    lastEnter.GetComponent<Image>().color  =  lastEnterNormalColor;//reset color
    canvasGroup.blocksRaycasts  =  true;//reset
}

bool  EnterItemGrid(GameObject  go) //judge whether point at specific bag
{
    if (go  ==  null)
    {
        return  false;
    }
    return  go.name  ==  "Panel";
}
```
