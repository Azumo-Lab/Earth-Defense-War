//    < Earth - Defense - War >
//    Copyright(C) < 2022 >  < Azumo - Lab >
//    <https://github.com/Azumo-Lab/Earth-Defense-War>

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Earth_Defend_War_NetCore3_1.GameProcess;
using Earth_Defend_War_NetCore3_1.GameProcess.UI;
using Earth_Defend_War_NetCore3_1.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Earth_Defend_War_NetCore3_1
{
    /// <summary>
    /// 一个小文档
    /// 
    /// 首先，这个文件是由若干类构成的。
    /// MainWindow 类的内容是全部的游戏界面控制，游戏逻辑判断代码。
    /// 
    /// 各个类的详细说明将会在各个类的顶部进行说明，这里不再进行详细叙述。
    /// 
    /// ================================
    /// 本类的用途
    /// ================================
    /// 这个类的用途是控制整个图形界面，和游戏逻辑的。是整个程序中最核心的类。
    /// 其他的类基本上是工具类，用来辅助游戏逻辑的。
    /// 
    /// ================================
    /// 本类的结构
    /// ================================
    /// 结构比较简单，但我写的挺乱的，主要分为两大块。
    /// 
    /// 界面显示的方法群
    /// 游戏逻辑的方法群
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// 游戏分数
        /// </summary>
        public int Scoll
        {
            get { return GameData.Scoll; }
            set
            {
                GameData.Scoll = value;
                Title = $"{GameData.Title} 当前分数：{GameData.Scoll}";
            }
        }

        /// <summary>
        /// 整体画面停止
        /// </summary>
        private bool screenstop;
        /// <summary>
        /// 整体画面停止
        /// </summary>
        public bool ScreenStop
        {
            get
            {
                return screenstop;
            }
            set
            {
                screenstop = value;
                if (screenstop)
                {
                    Canvas.Cursor = Cursors.AppStarting;
                    //TimerTools.StopAll();
                }
                else
                {
                    Canvas.Cursor = Cursors.None;
                    //TimerTools.StartAll();
                }
            }
        }

        private Timer Timer { get; } = new Timer();

        private ScriptActuator ScriptActuator { get; set; } = null;

        /// <summary>
        /// BGM
        /// </summary>
        private static MediaPlayer BGM = new MediaPlayer();

        /// <summary>
        /// 全屏元素的集合
        /// </summary>
        private List<Base> BaseModels { get; } = new List<Base>();

        /// <summary>
        /// 所有敌机的位置
        /// </summary>
        private List<TekiData> TekiModels { get; } = new List<TekiData>();

        /// <summary>
        /// 所有子弹的位置
        /// </summary>
        private List<KillsPoint> KillsPoints { get; } = new List<KillsPoint>();

        ~MainWindow()
        {
            //销毁时的事件
        }

        #region 游戏开始时的初始化

        /// <summary>
        /// 界面初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            #region 初始化加载资源

            //设置标题
            Title = GameData.Title;
            //全局资源文件的加载
            Global.Init();
            //加载菜单UI
            var menu = GameProcess.UI.Menu.GetMenu();

            #endregion

            #region 启动之后的必要事件绑定

            //菜单按钮的事件
            menu.DebugClickEven += new GameProcess.UI.Menu.DebugClick(Debug);
            menu.EndClickEven += new GameProcess.UI.Menu.EndClick(Close);
            menu.StartClickEven += new GameProcess.UI.Menu.StartClick(InitGame);
            menu.SettingClickEven += new GameProcess.UI.Menu.SettingClick(SettingDialog);

            //显示是否要关闭的确认框
            Closing += new System.ComponentModel.CancelEventHandler((obj, e) =>
            {
                if (MessageBox.Show("确定要结束游戏吗？", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            });

            //退出时的资源释放
            Application.Current.Exit += new ExitEventHandler((obj, e) =>
            {
                menu.GameStartMenuClose();
                Timer.Dispose();
            });

            #endregion

            #region 启动画面的延迟
#if !DEBUG
            //因为启动画面太好看了，我想多欣赏会儿
            System.Threading.Thread.Sleep(3000);
#endif
            #endregion

            #region 画面加载

            menu.GameStartMenu(Canvas);

            #endregion
        }

        /// <summary>
        /// 路径动画
        /// </summary>
        /// <param name="cvs">画板</param>
        /// <param name="path">路径</param>
        /// <param name="target">动画对象</param>
        /// <param name="duration">时间</param>
        private void AnimationByPath(Canvas cvs, System.Windows.Shapes.Path path, double targetWidth, int duration = 5)
        {
            #region 创建动画对象
            DropShadowEffect effect = new DropShadowEffect
            {
                Color = Colors.Red,
                BlurRadius = 5,
                ShadowDepth = 0,
                Direction = 0
            };
            System.Windows.Shapes.Rectangle target = new System.Windows.Shapes.Rectangle
            {
                Width = targetWidth,
                Height = targetWidth,
                Fill = new SolidColorBrush(Colors.Orange),
                Effect = effect
            };

            cvs.Children.Add(target);
            Canvas.SetLeft(target, -targetWidth / 2);
            Canvas.SetTop(target, -targetWidth / 2);
            target.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            #endregion

            MatrixTransform matrix = new MatrixTransform();
            TransformGroup groups = new TransformGroup();
            groups.Children.Add(matrix);
            target.RenderTransform = groups;
            string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
            this.RegisterName(registname, matrix);
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath
            {
                PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(path.Data.ToString())),
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                DoesRotateWithTangent = true,//跟随路径旋转
                RepeatBehavior = RepeatBehavior.Forever//循环
            };
            Storyboard story = new Storyboard();
            story.Children.Add(matrixAnimation);
            Storyboard.SetTargetName(matrixAnimation, registname);
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            story.FillBehavior = FillBehavior.Stop;
            story.Begin(target, true);
        }

        #endregion

        /// <summary>
        /// 显示设置选单项目
        /// </summary>
        public void SettingDialog()
        {
            Setting settingWindows = new Setting();
            settingWindows.SetMusic(ref BGM);
            settingWindows.ShowDialog();
        }

        #region 游戏主逻辑

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void InitGame()
        {
            Timer.Interval = 5;
            ScriptActuator = new ScriptActuator(Timer);

            //清空数据
            BaseModels.Clear(TekiModels, KillsPoints);

            //将鼠标隐藏
            Canvas.Cursor = Cursors.None;

            //清空界面
            Canvas.Clear();
            ///========================
            /// 背景设置
            ///========================
            //设置背景
            Canvas.Background = System.Windows.Media.Brushes.Black;
            //添加星空背景图片
            Canvas.Add(() =>
            {
                ModelData.BackPic1.SetXY(0, 0);
                ModelData.BackPic1.SetZIndex(StaticUtils.UIZindex.BACK);
                return ModelData.BackPic1;
            });
            Canvas.Add(() =>
            {
                ModelData.BackPic2.SetXY(0, ModelData.BackPic2.Source.Height);
                ModelData.BackPic2.SetZIndex(StaticUtils.UIZindex.BACK);
                return ModelData.BackPic2;
            });
            ///========================
            /// 加载Model数据
            ///========================
            Hero hero = Hero.GetHero();

            ///========================
            /// 主角设置
            ///========================
            //添加主角
            Canvas.Add(() =>
            {
                //设置主角位置
                hero.Image.SetXY(350, 300);
                hero.Image.SetZIndex(StaticUtils.UIZindex.UIElementTop);
                return hero.Image;
            });
            //添加主角的移动事件
            Canvas.MouseMove += new MouseEventHandler((obj, e) =>
            {
                System.Windows.Point point = e.GetPosition(Canvas);
                Canvas.SetLeft(hero.Image, point.X - 25);
                Canvas.SetTop(hero.Image, point.Y - 25);
                //主角的子弹发射
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (GameData.BulletsIsTooHot == false)
                    {
                        System.Windows.Controls.Image kill = new System.Windows.Controls.Image
                        {
                            Source = ModelData.KillPoints.Source
                        };
                        var PointX = Canvas.GetLeft(hero.Image);
                        var PointY = Canvas.GetTop(hero.Image);
                        Canvas.SetLeft(kill, PointX + hero.Image.Source.Width / 2);
                        Canvas.SetTop(kill, PointY + hero.Image.Source.Height / 2);
                        Canvas.Children.Add(kill);

                        //添加到子弹列表
                        BaseModels.Add(new KillsPoint() { IsMe = true, Image = kill });

                        var newList = new List<Base>(BaseModels);
                        foreach (var item in newList)
                        {
                            if (item is TekiData)
                            {
                                //主角发射导弹的同时，敌人也发射导弹，正所谓以眼还眼，以牙还牙
                                System.Windows.Controls.Image killTeki = new System.Windows.Controls.Image
                                {
                                    Source = ModelData.TekiKillPoints.Source
                                };
                                //将敌人发射的子弹设置在敌人附近
                                Canvas.SetLeft(killTeki, Canvas.GetLeft(item.Image) + killTeki.Source.Width);
                                Canvas.SetTop(killTeki, Canvas.GetTop(item.Image) + killTeki.Source.Height);
                                Canvas.Children.Add(killTeki);
                                //添加到子弹列表
                                BaseModels.Add(new KillsPoint() { IsMe = false, Image = killTeki });
                            }
                        }
                    }
                    GameData.BulletsIsTooHot = true;
                }
            });

            ScriptActuator.Add(new Script(0, true, 0.2.SecondsTo(), "fire", () =>
            {
                GameData.BulletsIsTooHot = false;
            }));

            ///========================
            /// 对话框
            ///========================
            GameTextBox gameTextBox = GameTextBox.Create(Canvas);
            Canvas.Add(() =>
            {
                gameTextBox.StopScreen += new GameTextBox.ScreenStop((stop) => { ScreenStop = stop; });
                gameTextBox.GameTextBoxButtonClickEvent += new GameTextBox.GameTextBoxButtonClick((button) =>
                {
                    switch (button.Name)
                    {
                        case "Menu":
                            GameProcess.UI.Menu.GetMenu().GameStartMenu(Canvas);
                            break;
                        case "Setting":
                            SettingDialog();
                            break;
                    }
                });
                gameTextBox.ClickCanvas += gameTextBox.GetClickCanvas(Canvas);
                gameTextBox.SetXY(gameTextBox.GetCenter(Canvas.Width), Canvas.Height - 200);
                gameTextBox.SetZIndex(StaticUtils.UIZindex.UITop);
                BaseModels.Add(new GameTextBoxModel { FrameworkElement = gameTextBox, Remove = false });
                return gameTextBox;
            });
            ///========================
            /// 添加脚本程序
            ///========================
            ScriptActuator.Add(new Script(2.SecondsTo(), "teki1", () =>
            {
                Canvas.Add(AddtoList: BaseModels, action: () =>
                {
                    TekiData tekiData = new TekiData(TekiData.Fine.LV1)
                    {
                        MovePoints = PointsData.Points[0]
                    };
                    Canvas.SetTop(tekiData.Image, 200);
                    Canvas.SetLeft(tekiData.Image, 200);

                    return tekiData;
                });
            }));
            int i = 0;
            ScriptActuator.Add(new Script(10.SecondsTo(), true, 2.SecondsTo(), "teki2", () =>
            {
                Canvas.Add(AddtoList: BaseModels, action: () =>
                {
                    var re = new TekiData(TekiData.Fine.LV2)
                    {
                        MovePoints = PointsData.Points[i]
                    };
                    i++;
                    return re;
                });
            }));

            ///========================
            /// 逻辑判定
            ///========================
            var elsp = new ElapsedEventHandler((obj, e) =>
            {
                //对全屏幕进行刷新，射击判定，敌人及星际陨石的移动
                Dispatcher.Invoke(() =>
                {

                    ScriptActuator.Next();
                    ScriptActuator.RunScript();

                    //星空背景的移动
                    double backpicTop = Canvas.GetTop(ModelData.BackPic1);
                    double BackPicTwoTop = Canvas.GetTop(ModelData.BackPic2);
                    Canvas.SetTop(ModelData.BackPic1, backpicTop + 0.5);
                    Canvas.SetTop(ModelData.BackPic2, BackPicTwoTop + 0.5);
                    if (BackPicTwoTop > Canvas.Height)
                    {
                        double newtop = backpicTop + ModelData.BackPic2.Source.Height;
                        Canvas.SetTop(ModelData.BackPic2, backpicTop - newtop);
                    }
                    if (backpicTop > Canvas.Height)
                    {
                        double newtop = BackPicTwoTop + ModelData.BackPic1.Source.Height;
                        Canvas.SetTop(ModelData.BackPic1, BackPicTwoTop - newtop);
                    }
                    //** 以下是元素
                    TekiModels.Clear();
                    //随机添加陨石
                    Random random = new Random();
                    int randomNum = random.Next();
                    GameData.RandomWidth += 1;
                    if (GameData.RandomWidth >= Canvas.Width)
                    {
                        GameData.RandomWidth = 0;
                    }
                    //对随机数筛选
                    if (randomNum > 0 && randomNum < 2000000)
                    {
                        var IshiImage = Canvas.Add(() =>
                        {
                            System.Windows.Controls.Image Ishi = new System.Windows.Controls.Image();
                            if (randomNum > 1500000)
                            {
                                Ishi.Source = ModelData.IShi1.Source;
                            }
                            else
                            if (randomNum > 1000000)
                            {
                                Ishi.Source = ModelData.IShi2.Source;
                            }
                            else
                            if (randomNum > 500000)
                            {
                                Ishi.Source = ModelData.IShi3.Source;
                            }
                            else
                            if (randomNum > 0)
                            {
                                Ishi.Source = ModelData.IShi4.Source;
                            }
                            Ishi.SetXY(GameData.RandomWidth, 0);
                            //将陨石设置到最底部
                            Ishi.SetZIndex(StaticUtils.UIZindex.BACKTop);
                            return Ishi;
                        });
                        BaseModels.Add(new IShi { Image = IshiImage, Remove = false });
                    }

                    //得到主角的位置
                    double heroX = Canvas.GetLeft(hero.Image);
                    double heroY = Canvas.GetTop(hero.Image);
                    //生成主角的碰撞判定框
                    Rect heroRect = new Rect(heroX, heroY, hero.Image.Source.Width, hero.Image.Source.Height);

                    //对屏幕全体元素进行遍历，刷新
                    foreach (var item in BaseModels)
                    {
                        switch (item.BaseType)
                        {
                            case BaseType.NONE:
                                break;
                            case BaseType.GameTextBoxModel://游戏对话框
                                if (Scoll >= 200)
                                {
                                    var gametextbox = item.FrameworkElement as GameTextBox;
                                    gametextbox.Str = Str.Strs["Win"];
                                    gametextbox.Lock = true;
                                    gametextbox.Show = true;
                                }
                                break;
                            case BaseType.IShi://飞翔的陨石
                                double tops = Canvas.GetTop(item.Image);
                                Canvas.SetTop(item.Image, tops + 1);
                                if (tops > Canvas.Height)
                                {
                                    Canvas.Children.Remove(item.Image);
                                    item.Remove = true;
                                }
                                break;
                            case BaseType.Boom:
                                Boom booms = item as Boom;
                                if (booms.Time <= 0)
                                {
                                    Canvas.Children.Remove(item.Image);
                                    item.Remove = true;
                                }
                                else
                                {
                                    booms.Time -= 1;
                                }
                                break;
                            case BaseType.TekiData:
                                TekiData tekiData = item as TekiData;
                                //刷新敌人的移动，根据事先输入好的坐标集合进行移动
                                var Points = tekiData.MovePoints;
                                Canvas.SetLeft(tekiData.Image, Points[tekiData.Point].X);
                                Canvas.SetTop(tekiData.Image, Points[tekiData.Point].Y);

                                tekiData.Rect = new Rect(Points[tekiData.Point], new System.Windows.Point(Points[tekiData.Point].X + tekiData.Image.Source.Width, Points[tekiData.Point].Y + tekiData.Image.Source.Height));
                                //添加敌机的位置范围信息
                                TekiModels.Add(tekiData);

                                tekiData.Point += 1;//移动到下一个坐标
                                if (tekiData.Point >= Points.Count)
                                {
                                    //最后一个坐标时，敌机消失
                                    item.Remove = true;
                                    tekiData.Remove = true;
                                    Canvas.Children.Remove(item.Image);
                                }
                                break;
                            case BaseType.KillsPoint:
                                KillsPoint killsPoint = item as KillsPoint;
                                //获取导弹所处位置
                                double top = Canvas.GetTop(killsPoint.Image);
                                double left = Canvas.GetLeft(killsPoint.Image);
                                //导弹消失的判定
                                if (top > Height || top < 0 || left < 0 || left > Width)
                                {
                                    //导弹飞出边界，消失
                                    Canvas.Children.Remove(killsPoint.Image);
                                    //删除这个
                                    killsPoint.Remove = true;
                                    continue;
                                }
                                //判定导弹是否射中！
                                Rect rect = new Rect(new System.Windows.Point(left, top), new System.Windows.Point(left + killsPoint.Image.Source.Width, top + killsPoint.Image.Source.Height));
                                if (!killsPoint.IsMe)
                                {
                                    if (heroRect.IntersectsWith(rect))
                                    {
                                        //自机爆炸
                                        Canvas.Children.Remove(hero.Image);
                                        System.Windows.Controls.Image boom = new System.Windows.Controls.Image()
                                        {
                                            Source = ModelData.Boom.Source
                                        };
                                        Canvas.SetLeft(boom, Canvas.GetLeft(hero.Image));
                                        Canvas.SetTop(boom, Canvas.GetTop(hero.Image));
                                        Canvas.Children.Add(boom);

                                        return;
                                    }
                                }
                                foreach (var p in TekiModels)
                                {
                                    //设定上只能一个导弹打一个
                                    if (killsPoint.IsMe)
                                    {
                                        if (p.Rect.IntersectsWith(rect))
                                        {
                                            //敌机受伤或爆炸
                                            p.IamFine--;
                                            item.Remove = true;
                                            Canvas.Children.Remove(killsPoint.Image);
                                            if (p.IamFine <= 0)
                                            {
                                                p.Remove = true;//从临时存储中移除
                                                var tekiItem = BaseModels[BaseModels.IndexOf(p)];//从整个列表中移除
                                                tekiItem.Remove = true;
                                                Canvas.Children.Remove(tekiItem.Image);
                                                System.Windows.Controls.Image boom = new System.Windows.Controls.Image()
                                                {
                                                    Source = ModelData.Boom.Source
                                                };
                                                Canvas.SetLeft(boom, p.Rect.X);
                                                Canvas.SetTop(boom, p.Rect.Y);
                                                BaseModels.Add(new Boom { Image = boom, Remove = false });
                                                Canvas.Children.Add(boom);
                                                Scoll += 100;
                                                goto DeleteData; //因为改变kills所以重新进入循环
                                            }
                                        }
                                    }
                                }
                                if (killsPoint.IsMe)
                                {
                                    //我方导弹
                                    Canvas.SetTop(killsPoint.Image, top - 2);
                                }
                                else
                                {
                                    //敌方导弹
                                    Canvas.SetTop(killsPoint.Image, top + 2);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                DeleteData:
                    //界面整体刷新之后，删除全部的不需要的元素
                    BaseModels.RemoveAll(objA => objA.Remove);
                    TekiModels.RemoveAll(objA => objA.Remove);
                    KillsPoints.RemoveAll(objA => objA.Remove);
                });
            });

            Timer.Elapsed += elsp;
            Timer.Start();
            ///========================
            /// 最后的准备工作
            ///========================
            gameTextBox.Str = Str.Strs["StartStr"];
            gameTextBox.Show = true;
        }

        #endregion

        #region DEBUG 相关
#if DEBUG
        /// <summary>
        /// 用来对游戏进行设置与微调,用
        /// </summary>
        private void Debug()
        {
            MessageBox.Show("Debug开始");
            Canvas.Children.Clear();
            Canvas.Background = System.Windows.Media.Brushes.AliceBlue;
            Canvas.MouseMove += new MouseEventHandler((obj, e) =>
            {
                var point = e.GetPosition(Canvas);
                using StreamWriter fileStream = File.AppendText(@"D:\Points.txt");
                fileStream.WriteLine($"new System.Windows.Point({point.X},{point.Y}),");
            });
        }
#endif
        #endregion DEBUG 相关

        #region 游戏界面控制

        /// <summary>
        /// 停止所有界面的刷新
        /// </summary>
        public void StopAll()
        {

        }

        /// <summary>
        /// 重新开始所有界面的刷新
        /// </summary>
        public void StartAll()
        {

        }

        /// <summary>
        /// 显示游戏的对话框
        /// </summary>
        public void ShowGameTextBox()
        {

        }

        #endregion
    }


    /// ============================================================================================================
    /// 脚本执行器
    /// ============================================================================================================

    /// <summary>
    /// 这是一个脚本执行器
    /// </summary>
    public class ScriptActuator
    {
        /// <summary>
        /// 脚本执行事件
        /// </summary>
        private double Time { get; set; } = 0d;

        /// <summary>
        /// 脚本执行的周期
        /// </summary>
        private double Interval { get; set; }

        /// <summary>
        /// 需要执行的脚本集合
        /// </summary>
        private List<Script> Scripts { get; } = new List<Script>();

        /// <summary>
        /// 初始化一个脚本执行器
        /// </summary>
        /// <param name="timer">这是用于得到执行周期的</param>
        public ScriptActuator(System.Timers.Timer timer)
        {
            Interval = timer.Interval;
        }

        /// <summary>
        /// 初始化一个脚本执行器
        /// </summary>
        /// <param name="timer">这是用于得到执行周期的</param>
        public ScriptActuator(double Interval)
        {
            this.Interval = Interval;
        }

        /// <summary>
        /// 转到下一个执行周期
        /// </summary>
        public void Next()
        {
            Time += Interval;
        }

        public void Add(Script script)
        {
            Scripts.Add(script);
        }

        public void RunScript()
        {
            Scripts.ForEach(script => 
            {
                if (Time >= script.Time)
                {
                    switch (script.MethodTypes)
                    {
                        case Script.MethodType.Action:
                            script.Action.Invoke();
                            break;
                        case Script.MethodType.Function:
                            break;
                        default:
                            break;
                    }
                    if (script.Loop)
                    {
                        script.Time += script.SUKIMA;
                    }
                    else
                    {
                        script.REMOVE = true;
                    }
                }
            });
            Scripts.RemoveAll(sc => sc.REMOVE);
        }
    }

    public class Script
    {
        /// <summary>
        /// 脚本触发事件
        /// </summary>
        public double Time { get; set; }

        public bool REMOVE { get; set; }

        /// <summary>
        /// 循环触发的时间间隔
        /// </summary>
        public double SUKIMA { get; }

        /// <summary>
        /// 循环触发
        /// </summary>
        public bool Loop { get; }

        /// <summary>
        /// 脚本名称
        /// </summary>
        /// 
        public string Name { get; }

        /// <summary>
        /// 执行的方法
        /// </summary>
        public Action Action { get; }

        public MethodType MethodTypes { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Name"></param>
        /// <param name="Action"></param>
        public Script(double Time, string Name, Action Action)
        {
            this.Time = Time;
            this.Name = Name;
            this.Action = Action;
            this.Loop = false;
            this.SUKIMA = double.NaN;
            MethodTypes = MethodType.Action;
            REMOVE = false;
        }

        public Script(double Time, bool Loop, double SUKIMA, string Name, Action Action)
        {
            this.Time = Time;
            this.Name = Name;
            this.Action = Action;
            this.Loop = Loop;
            this.SUKIMA = SUKIMA;
            MethodTypes = MethodType.Action;
            REMOVE = false;
        }

        public enum MethodType
        {
            Action,
            Function
        }
    }
    /// <summary>
    /// ============================================================================================================
    /// 游戏数据类
    /// ============================================================================================================
    public static class GameData
    {
        /// <summary>
        /// 游戏分数
        /// </summary>
        public static int Scoll { get; set; } = 0;

        /// <summary>
        /// 这个程序的路径
        /// </summary>
        public static string ExePath { get; } = AppContext.BaseDirectory;

        /// <summary>
        /// 子弹冷却
        /// </summary>
        public static bool BulletsIsTooHot { get; set; } = false;

        /// <summary>
        /// 陨石的随机生成位置
        /// </summary>
        public static int RandomWidth { get; set; } = 0;

        /// <summary>
        /// 游戏标题
        /// </summary>
        public const string Title = "Earth Defend War";

        /// <summary>
        /// 初始化
        /// </summary>
        static GameData()
        {
        }

        /// <summary>
        /// 将全部数据重置
        /// </summary>
        public static void ReSetAll()
        {
            Scoll = 0;
        }
    }

    /// ============================================================================================================
    /// 基础数据Model类
    /// ============================================================================================================
    public static class ModelData
    {
        /// <summary>
        /// 主角的资源文件
        /// </summary>
        public static System.Windows.Controls.Image Hero { get; } = Properties.Resources.Hero.GetImage();
        /// <summary>
        /// 爆炸
        /// </summary>
        public static System.Windows.Controls.Image Boom { get; } = Properties.Resources.Boom.GetImage();
        /// <summary>
        /// 石头1
        /// </summary>
        public static System.Windows.Controls.Image IShi1 { get; } = Properties.Resources.IShi1.GetImage();
        /// <summary>
        /// 石头2
        /// </summary>
        public static System.Windows.Controls.Image IShi2 { get; } = Properties.Resources.IShi2.GetImage();
        /// <summary>
        /// 石头3
        /// </summary>
        public static System.Windows.Controls.Image IShi3 { get; } = Properties.Resources.IShi3.GetImage();
        /// <summary>
        /// 石头4
        /// </summary>
        public static System.Windows.Controls.Image IShi4 { get; } = Properties.Resources.IShi4.GetImage();
        /// <summary>
        /// 我方子弹
        /// </summary>
        public static System.Windows.Controls.Image KillPoints { get; } = Properties.Resources.KillPoints.GetImage();
        /// <summary>
        /// 敌方Boss
        /// </summary>
        public static System.Windows.Controls.Image TekiBoss { get; } = Properties.Resources.TekiBoss.GetImage();
        /// <summary>
        /// 敌方LV1
        /// </summary>
        public static System.Windows.Controls.Image TekiLV1 { get; } = Properties.Resources.TekiLV1.GetImage();
        /// <summary>
        /// 敌方LV2
        /// </summary>
        public static System.Windows.Controls.Image TekiLV2 { get; } = Properties.Resources.TekiLV2.GetImage();
        /// <summary>
        /// 敌方子弹，
        /// </summary>
        public static System.Windows.Controls.Image TekiKillPoints { get; } = Properties.Resources.TekiKillPoints.GetImage();
        /// <summary>
        /// 星空背景1
        /// </summary>
        public static System.Windows.Controls.Image BackPic1 { get; } = Properties.Resources.StarBack1.GetImage();
        /// <summary>
        /// 星空背景2
        /// </summary>
        public static System.Windows.Controls.Image BackPic2 { get; } = Properties.Resources.StarBack2.GetImage();
    }

    /// <summary>
    /// 子弹
    /// </summary>
    public class KillsPoint : Base
    {
        public KillsPoint() : base(BaseType.KillsPoint)
        {

        }

        private bool isme;
        /// <summary>
        /// 子弹的敌我识别
        /// </summary>
        public bool IsMe
        {
            get { return isme; }
            set
            {
                isme = value;
                if (isme)
                {
                    Image = ModelData.KillPoints;
                }
                else
                {
                    Image = ModelData.TekiKillPoints;
                }
            }
        }
    }

    public class Hero : Base
    {
        /// <summary>
        /// 初始化的判断
        /// </summary>
        private static bool Init { get; set; } = false;
        private Hero() : base(BaseType.Hero)
        {
            lock (this)
            {
                if (!Init)
                {
                    Init = true;
                    Image = ModelData.Hero;
                }
                else
                {
                    throw new ArgumentException("产生了多个主角");
                }
            }
        }

        /// <summary>
        /// 设置主角的生命值
        /// </summary>
        public int Life { get; set; } = 5;

        /// <summary>
        /// 主角登场
        /// </summary>
        /// <returns></returns>
        public static Hero GetHero()
        {
            if (!Init)
            {
                return new Hero();
            }
            else
            {
                throw new ArgumentException("产生了多个主角");
            }
        }
    }

    /// <summary>
    /// 敌机的数据
    /// </summary>
    public class TekiData : Base
    {
        public TekiData(Fine fine) : base(BaseType.TekiData)
        {
            Image = fine switch
            {
                Fine.LV1 => ModelData.TekiLV1,
                Fine.LV2 => ModelData.TekiLV2,
                Fine.Boss => ModelData.TekiBoss,
                _ => throw new ArgumentException("未指定的参数"),
            };
            IamFine = (int)fine;
        }
        /// <summary>
        /// 敌机的移动数据
        /// </summary>
        public List<System.Windows.Point> MovePoints { get; set; }

        /// <summary>
        /// 敌机的生命值
        /// </summary>
        public int IamFine { get; set; }
        /// <summary>
        /// 用于指示当前移动位置在坐标集中的位置
        /// </summary>
        public int Point { get; set; } = 0;
        public Rect Rect { get; set; }
        public enum Fine
        {
            LV1 = 4,
            LV2 = 8,
            Boss = 12
        }
    }

    /// <summary>
    /// 爆炸的烟火
    /// </summary>
    public class Boom : Base
    {
        public Boom() : base(BaseType.Boom)
        {
            Image = ModelData.Boom;
        }
        /// <summary>
        /// 爆炸持续时间
        /// </summary>
        public int Time { get; set; } = 30;
    }

    /// <summary>
    /// 飞翔的陨石（陨石没啥害处，不能打爆，但能把我们撞爆（以后的事情了，现在就是移动的背景））
    /// </summary>
    public class IShi : Base
    {
        public IShi() : base(BaseType.IShi) { }
    }

    /// <summary>
    /// 对话框
    /// </summary>
    public class GameTextBoxModel : Base
    {
        public GameTextBoxModel() : base(BaseType.GameTextBoxModel) { }
    }

    /// <summary>
    /// 基础类型
    /// </summary>
    public class Base
    {
        public Base(BaseType baseType)
        {
            BaseType = baseType;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Remove { get; set; } = false;

        public BaseType BaseType { get; } = BaseType.NONE;

        /// <summary>
        /// 元素
        /// </summary>
        public System.Windows.Controls.Image Image { get; set; }

        public FrameworkElement FrameworkElement { get; set; }
    }

    /// <summary>
    /// 基础类型
    /// </summary>
    public enum BaseType
    {
        NONE,
        GameTextBoxModel,
        IShi,
        Boom,
        TekiData,
        Hero,
        KillsPoint,
    }


    /// ============================================================================================================
    /// 扩展方法
    /// ============================================================================================================




    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class StaticUtils
    {
        public static T RedomNumber<T>(this List<T> ts)
        {
            var seed = Guid.NewGuid().GetHashCode();
            Random r = new Random(seed);
            int i = r.Next(0, 1000000);
            double val = (double)i / 1000000;
            int Value = (int)Math.Round(ts.Count * val);
            if (Value >= ts.Count)
            {
                Value--;
            }
            return ts[Value];
        }
        public static System.Windows.Controls.Image GetImage(this Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, bitmap.RawFormat);
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return new System.Windows.Controls.Image
            {
                Source = bitmapImage
            };
        }

        public static void SetXY(this UIElement uIElement, double Left, double Top)
        {
            Canvas.SetLeft(uIElement, Left);
            Canvas.SetTop(uIElement, Top);
        }

        public static double GetCenter(this UIElement uIElement, double CanvasWeight)
        {
            var UIEL = uIElement as FrameworkElement;
            return (CanvasWeight - UIEL.Width) / 2;
        }

        public static void Add(this Canvas canvas, UIElement uIElement)
        {
            canvas.Children.Add(uIElement);
        }

        public static void Add(this Canvas canvas, Func<UIElement> action)
        {
            canvas.Children.Add(action.Invoke());
        }

        public static T Add<T>(this Canvas canvas, Func<T> action) where T : UIElement
        {
            var returnvalue = action.Invoke();
            canvas.Children.Add(returnvalue);
            return returnvalue;
        }

        public static T Add<T>(this Canvas canvas, List<T> AddtoList, Func<T> action) where T : Base
        {
            var returnvalue = action.Invoke();
            if (returnvalue.Image != null)
            {
                canvas.Children.Add(returnvalue.Image);
            }
            if (returnvalue.FrameworkElement != null)
            {
                canvas.Children.Add(returnvalue.FrameworkElement);
            }
            AddtoList.Add(returnvalue);
            return returnvalue;
        }

        public static void Remove(this Canvas canvas, UIElement uIElement)
        {
            canvas.Children.Remove(uIElement);
        }

        public static void Clear(this System.Collections.IList list, params System.Collections.IList[] ClaerList)
        {
            list.Clear();
            foreach (var item in ClaerList)
            {
                item.Clear();
            }
        }

        public static void Clear(this Canvas canvas)
        {
            canvas.Children.Clear();
        }

        public static void SetZIndex(this UIElement uIElement, UIZindex iZindex)
        {
            switch (iZindex)
            {
                case UIZindex.UITop:
                case UIZindex.UIElementTop:
                case UIZindex.BACKTop:
                case UIZindex.BACK:
                    Panel.SetZIndex(uIElement, (int)iZindex);
                    break;
                case UIZindex.NONE:
                    break;
                default:
                    break;
            }
        }

        private static readonly Dictionary<RoutedEvent, Delegate> Delegates = new Dictionary<RoutedEvent, Delegate>();

        public static void AddHandlerAndList(this Canvas canvas, RoutedEvent routedEvent, Delegate handler)
        {
            Delegates[routedEvent] = handler;
            canvas.AddHandler(routedEvent, Delegates[routedEvent]);
        }

        public static void RemoveHandlerAndList(this Canvas canvas, RoutedEvent routedEvent)
        {
            if (Delegates.ContainsKey(routedEvent))
            {
                canvas.RemoveHandler(routedEvent, Delegates[routedEvent]);
                Delegates.Remove(routedEvent);
            }
        }

        public static int SecondsTo(this int Seconds)
        {
            return Seconds * 1000;
        }

        public static int SecondsTo(this double Seconds)
        {
            return (int)(Seconds * 1000);
        }

        /// <summary>
        /// UI的层级
        /// </summary>
        public enum UIZindex
        {
            /// <summary>
            /// UI级别的Top
            /// </summary>
            UITop = 999,
            /// <summary>
            /// 游戏所有元素的TOP
            /// </summary>
            UIElementTop = 200,
            /// <summary>
            /// 一般的元素添加顺序
            /// </summary>
            NONE = 0,
            /// <summary>
            /// 背景元素上面
            /// </summary>
            BACKTop = -500,
            /// <summary>
            /// 最下面的背景元素
            /// </summary>
            BACK = -999,
        }
    }


    /// ============================================================================================================
    /// 
    /// ============================================================================================================

}
