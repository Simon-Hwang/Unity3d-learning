## 作业内容
### 粒子光环
----
- 粒子光环效果图如下：
![image](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework7/images/result.png)
- 点击[视频](https://github.com/Simon-Hwang/Unity3d-learning/blob/master/Homework7/demontration.mp4.mp4)查看演示视频
- 首先根据Unity3d的粒子类```ParticleSystem```获取对象信息，以及确立粒子系统中的基本信息如粒子数目等信息，调用```GetParticles()```获取并初始化完成后调用```SetPatricles()```生成粒子系统，即```start```内代码如下:
```c
void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particlesArray = new ParticleSystem.Particle[particleNumber];
        particleSystem.maxParticles = particleNumber;
        particleAngle = new float[particleNumber];
        particleRadius = new float[particleNumber];
        particleSystem.Emit(particleNumber);
        particleSystem.GetParticles(particlesArray);
        init();
        particleSystem.SetParticles(particlesArray, particlesArray.Length);  
    }
```
- 初始化函数```init```中则是设置粒子的信息，如旋转方向、旋转半径等，具体代码如下：
```c
for (int i = 0; i < particleNumber; i++)
        {
            float angle = Random.Range(0.0f, 360.0f);
            float rad = angle / 180 * Mathf.PI;
            float midRadius = (maxRadius + minRadius) / 2;
            float rate1 = Random.Range(1.0f, midRadius / minRadius);
            float rate2 = Random.Range(midRadius / maxRadius, 1.0f);
            float r = Random.Range(minRadius * rate1, maxRadius * rate2);
            particlesArray[i].size = size;
            particleAngle[i] = angle;
            particleRadius[i] = r;
            particlesArray[i].position = new Vector3(r * Mathf.Cos(rad), r * Mathf.Sin(rad), 0.0f);
        }
```
- 初始化完成后需要设置```Update```更新粒子信息，具体则是根据时间更改角度达到旋转效果，后调用```SetParticles```设置，即：
```c
for (int i = 0; i < particleNumber; i++)
        {
            time += Time.deltaTime;
            particleAngle[i] += speed * (i % 10 + 1);
            particleAngle[i] = (particleAngle[i] + 360) % 360; // turn into arc
            float rad = particleAngle[i] / 180 * Mathf.PI;
            particlesArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0f);
        }
        particleSystem.SetParticles(particlesArray, particleNumber);
```
