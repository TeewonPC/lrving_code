using AnimalEnclosure.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimalEnclosure
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 字段

        // 投放动物的时间间隔
        private double squabTimeInterval = 3;
        private double crabTimeInterval = 4;
        private double snakeTimeInterval = 5;
        private double pigTimeInterval = 8;

        private int maxBloodPoints = 10000;  //玩家最大血点

        // 动物逃离围场造成的伤害点
        private int squabDamagePoints = 100;
        private int crabDamagePoints = 200;
        private int snakeDamagePoints = 500;
        private int pigDamagePoints = 1000;

        // 逃离动物和被杀动物的个数
        private int escapeCount;
        private int killedCount;
        private int maxReleasedAnimals = 50;

        private Dictionary<AnimalsTemplate, Storyboard> storyboards = new Dictionary<AnimalsTemplate, Storyboard>();
        DispatcherTimer animalsTimer = new DispatcherTimer();
        string releasedAnimal = string.Empty;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            animalsTimer.Tick += AnimalTimer_Tick;
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            startGame.IsEnabled = false;

            // 重置游戏
            escapeCount = 0;
            killedCount = 0;

            releasedAnimal = RandomSelectedAnimal();
            
            // 启动释放动物计时器
            animalsTimer.Interval = TimeSpan.FromSeconds(2);
            animalsTimer.Start();

        }

        /// <summary>
        /// 动物在投放到围场前进行准备
        /// </summary>
        /// <param name="animalType"></param>    
        private void AnimalTimer_Tick(object sender, EventArgs e)
        {         
            releasedAnimal = RandomSelectedAnimal();

            // 创建动物   
            AnimalsTemplate animalsTemplate = new AnimalsTemplate();
            animalsTemplate.IsMoving = true;

            animalsTemplate.MouseLeftButtonDown += animalsTemplate_MouseLeftButtonDown;

            // 随机设置动物进入围场的位置
            Random random = new Random();
            Canvas.SetLeft(animalsTemplate, random.Next(100, 700));
            Canvas.SetTop(animalsTemplate, 50.0);

            Storyboard storyboard = new Storyboard();

            #region 乳鸽

            if (releasedAnimal.Equals("Squab"))
            {               
                animalsTemplate.animalImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/鸽子-01.png"));

                //创建X轴方向动画
                DoubleAnimation moveAnimation = new DoubleAnimation(
                  Canvas.GetLeft(animalsTemplate),
                  canvasBackground.ActualWidth,
                  new Duration(TimeSpan.FromSeconds(10))
                );

                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Left)"));
                storyboard.Children.Add(moveAnimation);

                //创建Y轴方向动画
                moveAnimation = new DoubleAnimation(
                  Canvas.GetTop(animalsTemplate),
                  canvasBackground.ActualHeight,
                  new Duration(TimeSpan.FromSeconds(10))
                );

                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Top)"));
                storyboard.Children.Add(moveAnimation);
            }

            #endregion

            #region 螃蟹

            if (releasedAnimal.Equals("Crab"))
            {
                animalsTemplate.animalImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/螃蟹-01.png"));

                DoubleAnimation moveAnimation = new DoubleAnimation();
                moveAnimation.To = canvasBackground.ActualWidth;
                moveAnimation.Duration = TimeSpan.FromSeconds(10);

                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Left)"));

                storyboard.Children.Add(moveAnimation);
            }

            #endregion

            #region 蛇

            if (releasedAnimal.Equals("Snake"))
            {
                animalsTemplate.animalImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/蛇-01.png"));

                //创建X轴方向动画
                DoubleAnimation moveAnimation = new DoubleAnimation();
                moveAnimation.From = Canvas.GetLeft(animalsTemplate);
                moveAnimation.To = Canvas.GetLeft(animalsTemplate)/2;

                moveAnimation.AutoReverse = true;  //设置动画播放完后反向在播放
                moveAnimation.RepeatBehavior = RepeatBehavior.Forever;  //设置为循环播放

                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Left)"));
                storyboard.Children.Add(moveAnimation);

                //创建Y轴方向动画
                moveAnimation = new DoubleAnimation(
                    Canvas.GetTop(animalsTemplate),
                    canvasBackground.ActualHeight,
                    new Duration(TimeSpan.FromSeconds(10))
                );

                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Top)"));
                storyboard.Children.Add(moveAnimation);
            }

            #endregion

            #region 猪

            if (releasedAnimal.Equals("Pig"))
            {
                animalsTemplate.animalImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/猪-01.png"));

                DoubleAnimation moveAnimation = new DoubleAnimation();
                moveAnimation.To = canvasBackground.ActualHeight;

                moveAnimation.Duration = TimeSpan.FromSeconds(10);
                Storyboard.SetTarget(moveAnimation, animalsTemplate);
                Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Top)"));

                storyboard.Children.Add(moveAnimation);
            }

            #endregion

            // 将动物投放到围场
            canvasBackground.Children.Add(animalsTemplate);
            // 将故事板添加到追踪容器中            
            storyboards.Add(animalsTemplate, storyboard);

            // 配置和启动故事板
            storyboard.Completed += storyboard_Completed;
            storyboard.Begin();
        }

        /// <summary>
        /// 随机挑选动物
        /// </summary>
        /// <returns></returns>
        private string RandomSelectedAnimal()
        {
            string[] animals = { "Squab", "Crab", "Snake", "Pig" };
            Random random = new Random();
            string animal = animals[random.Next(0, animals.Length)];
            return animal;
        }

        /// <summary>
        /// 动画完成触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storyboard_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = (ClockGroup)sender;

            DoubleAnimation completedAnimation = (DoubleAnimation)clockGroup.Children[0].Timeline;
            AnimalsTemplate completedAnimals = (AnimalsTemplate)Storyboard.GetTarget(completedAnimation);

            if (completedAnimals.IsMoving)
            {
                escapeCount++;
            }       

            // 检查游戏是否结束
            if ((escapeCount + killedCount) > maxReleasedAnimals)
            {
                animalsTimer.Stop();

                // 找到所有正在进行的故事板
                foreach (KeyValuePair<AnimalsTemplate, Storyboard> item in storyboards)
                {
                    Storyboard storyboard = item.Value;
                    AnimalsTemplate animalsTemplate = item.Key;

                    storyboard.Stop();
                    canvasBackground.Children.Remove(animalsTemplate);
                }

                storyboards.Clear();
                startGame.IsEnabled = true;

                //if (maxBloodPoints <= 0)
                //{
                //    MessageBox.Show("你已收到10000点伤害，去地狱忏悔吧！");
                //}
                //else
                //{
                //    if (maxBloodPoints >= 9000) MessageBox.Show("骚年，你已获得三颗星奖励！");
                //    if (maxBloodPoints >= 6000 && maxBloodPoints < 9000) MessageBox.Show("骚年，你已获得两颗星奖励！");
                //    if (maxBloodPoints < 6000) MessageBox.Show("骚年，你已获得一颗星奖励！");
                //}
            }
            else
            {
                tbEscapedAnimals.Text = String.Format("逃跑动物：{0}个.", escapeCount);            
                switch (releasedAnimal)
                {
                    case "Squab":
                        if (animalsTimer.Interval.TotalSeconds < squabTimeInterval)
                        {
                            double duringSeconds = squabTimeInterval - animalsTimer.Interval.TotalSeconds;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }
                        else
                        {
                            double duringSeconds = animalsTimer.Interval.TotalSeconds - squabTimeInterval;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }

                        maxBloodPoints = maxBloodPoints - squabDamagePoints;
                        if (maxBloodPoints < 0) maxBloodPoints = 0;
                        tbBloodPoint.Text = String.Format("玩家血量：{0}点.", maxBloodPoints);
                        break;
                    case "Crab":
                        if (animalsTimer.Interval.TotalSeconds < crabTimeInterval)
                        {
                            double duringSeconds = crabTimeInterval - animalsTimer.Interval.TotalSeconds;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }
                        else
                        {
                            double duringSeconds = animalsTimer.Interval.TotalSeconds - crabTimeInterval;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }

                        maxBloodPoints = maxBloodPoints - crabDamagePoints;
                        if (maxBloodPoints < 0) maxBloodPoints = 0;
                        tbBloodPoint.Text = String.Format("玩家血量：{0}点.", maxBloodPoints);
                        break;
                    case "Snake":
                        if (animalsTimer.Interval.TotalSeconds < snakeTimeInterval)
                        {
                            double duringSeconds = snakeTimeInterval - animalsTimer.Interval.TotalSeconds;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }
                        else
                        {
                            double duringSeconds = animalsTimer.Interval.TotalSeconds - snakeTimeInterval;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }
                        
                        maxBloodPoints = maxBloodPoints - snakeDamagePoints;
                        if (maxBloodPoints < 0) maxBloodPoints = 0;
                        tbBloodPoint.Text = String.Format("玩家血量：{0}点.", maxBloodPoints);
                        break;
                    case "Pig":
                        if (animalsTimer.Interval.TotalSeconds < pigTimeInterval)
                        {
                            double duringSeconds = pigTimeInterval - animalsTimer.Interval.TotalSeconds;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }
                        else
                        {
                            double duringSeconds = animalsTimer.Interval.TotalSeconds - pigTimeInterval;
                            animalsTimer.Interval = TimeSpan.FromSeconds(duringSeconds);
                        }

                        maxBloodPoints = maxBloodPoints - pigDamagePoints;
                        if (maxBloodPoints < 0) maxBloodPoints = 0;
                        tbBloodPoint.Text = String.Format("玩家血量：{0}点.", maxBloodPoints);
                        break;
                }

                

                Storyboard storyboard = (Storyboard)clockGroup.Timeline;
                storyboard.Stop();

                storyboards.Remove(completedAnimals);
                canvasBackground.Children.Remove(completedAnimals);
            }
        }

        /// <summary>
        /// 鼠标点击猎杀动物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void animalsTemplate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            killedCount++;

            tbKilledAnimals.Text = String.Format("击杀动物：{0}个.", killedCount);

            // 创建动物
            AnimalsTemplate animalsTemplate = (AnimalsTemplate)sender;
            animalsTemplate.IsMoving = false;

            // 获取动物的当前位置
            Storyboard storyboard = storyboards[animalsTemplate];
            double currentTop = Canvas.GetTop(animalsTemplate);

            storyboard.Completed -= storyboard_Completed;

            // 猎杀正在移动的动物
            storyboard.Stop();

            // 清空故事板
            storyboard.Children.Clear();

            DoubleAnimation riseAnimation = new DoubleAnimation();
            riseAnimation.From = currentTop;
            riseAnimation.To = 0;
            riseAnimation.Duration = TimeSpan.FromSeconds(2);

            Storyboard.SetTarget(riseAnimation, animalsTemplate);
            Storyboard.SetTargetProperty(riseAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(riseAnimation);

            DoubleAnimation slideAnimation = new DoubleAnimation();
            double currentLeft = Canvas.GetLeft(animalsTemplate);
            // Throw the bomb off the closest side.
            if (currentLeft < canvasBackground.ActualWidth / 2)
            {
                slideAnimation.To = -100;
            }
            else
            {
                slideAnimation.To = canvasBackground.ActualWidth + 100;
            }
            slideAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(slideAnimation, animalsTemplate);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(slideAnimation);

            // 启动新的动画
            storyboard.Duration = slideAnimation.Duration;
            storyboard.Begin();
        }

    }
}
