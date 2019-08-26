using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Perfiles_vegetacion_test_1
{
    class CustomTickBar_V : System.Windows.Controls.Primitives.TickBar
    {
        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            
            double num = this.Maximum - this.Minimum;
            double y = this.ReservedSpace * 0.5;
            FormattedText formattedText = null;
            double x = 0;
            for (double i = 0; i <= num; i += this.TickFrequency)
            {
                formattedText = new FormattedText(i.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black, 1);
                if (this.Minimum == i)
                    x = 0;
                else
                    x += this.ActualHeight / (num / this.TickFrequency);
                dc.DrawText(formattedText, new Point(0,this.ActualHeight - x));
            }
            /*
            Size size = new Size(base.ActualWidth, base.ActualHeight);
            int tickCount = (int)((this.Maximum - this.Minimum) / this.TickFrequency) + 1;
            if ((this.Maximum - this.Minimum) % this.TickFrequency == 0)
            {
                tickCount -= 1;
            }
            Double tickFrequencySize;
            // Calculate tick's setting
            tickFrequencySize = (size.Height * this.TickFrequency / (this.Maximum - this.Minimum));
            string text = "";
            FormattedText formattedText = null;
            double num = this.Maximum - this.Minimum;
            int i = 0;
            // Draw each tick text
            for (i = 0; i <= tickCount; i++)
            {
                text = Convert.ToString(Convert.ToInt32(this.Minimum + this.TickFrequency * i), 10);
                //g.DrawString(text, font, brush, drawRect.Left + tickFrequencySize * i, drawRect.Top + drawRect.Height/2, stringFormat);
                formattedText = new FormattedText(text, System.Globalization.CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black,1);
                dc.DrawText(formattedText, new Point(0, (tickFrequencySize * i)));
            }
            */
        }
    }
    class CustomTickBar_H : System.Windows.Controls.Primitives.TickBar
    {
        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            RotateTransform RT = new RotateTransform();
            RT.Angle = -90;
            double num = this.Maximum - this.Minimum;
            double y = this.ReservedSpace * 0.5;
            FormattedText formattedText = null;
            double x = 0;
            for (double i = 0; i <= num; i += this.TickFrequency)
            {
                formattedText = new FormattedText(i.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black, 1);
                if (this.Minimum == i)
                    x = 0;
                else
                    x += this.ActualWidth / (num / this.TickFrequency);
                
            dc.PushTransform(RT);
                dc.DrawText(formattedText, new Point(-20,x));
            dc.Pop();
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /* for (var tabIndex = MainTabControl.Items.Count - 1; tabIndex >= 0; tabIndex--)
             {
                 MainTabControl.SelectedIndex = tabIndex;
                 MainTabControl.UpdateLayout();
             }*/
            PreloadTabs(tabcontrol);
        }

        /// <summary>
        /// Preloads tab items of a tab control in sequence.
        /// </summary>
        /// <param name="tabControl">The tab control.</param>
        public static void PreloadTabs(TabControl tabControl)
        {
            // Evaluate
            if (tabControl.Items != null)
            {
                // The first tab is already loaded
                // so, we will start from the second tab.
                if (tabControl.Items.Count > 1)
                {
                    // Hide tabs
                    tabControl.Opacity = 0.0;

                    // Last action
                    Action onComplete = () =>
                    {
                        // Set index to first tab
                        tabControl.SelectedIndex = 0;

                        // Show tabs
                        tabControl.Opacity = 1.0;
                    };

                    // Second tab
                    var firstTab = (tabControl.Items[1] as TabItem);
                    if (firstTab != null)
                    {
                        PreloadTab(tabControl, firstTab, onComplete);
                    }
                }
            }
        }

        /// <summary>
        /// Preloads an individual tab item.
        /// </summary>
        /// <param name="tabControl">The tab control.</param>
        /// <param name="tabItem">The tab item.</param>
        /// <param name="onComplete">The onComplete action.</param>
        private static void PreloadTab(TabControl tabControl, TabItem tabItem, Action onComplete = null)
        {
            // On update complete
            tabItem.Loaded += delegate
            {
                // Update if not the last tab
                if (tabItem != tabControl.Items.GetItemAt(tabControl.Items.Count-1))
                {
                    // Get next tab
                    var nextIndex = tabControl.Items.IndexOf(tabItem) + 1;
                    var nextTabItem = tabControl.Items[nextIndex] as TabItem;

                    // Preload
                    if (nextTabItem != null)
                    {
                        PreloadTab(tabControl, nextTabItem, onComplete);
                    }
                }

                else
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                }
            };

            // Set current tab context
            tabControl.SelectedItem = tabItem;
        }

        public class Tree
        {
            public int ID{ get; set; }
            public string sp { get; set; }
            public double htot { get; set; }
            public double hfust{ get; set; }
            public double dap { get; set; }
            public double wcopa_x { get; set; }
            public double wcopa_y { get; set; }
            public double x { get; set; }
            public double y { get; set; }

            public Tree(int id, string sp, double htot, double hfust, double dap, double wcopax, double wcopay, double x, double y)
            {
                Tree tree = this;
                tree.ID = id;
                tree.sp = sp;
                tree.htot = htot;
                tree.hfust = hfust;
                tree.dap = dap;
                tree.wcopa_x = wcopax;
                tree.wcopa_y = wcopay;
                tree.x = x;
                tree.y = y;
            }
        }
        private void Button_Click_CSV(object sender, RoutedEventArgs e)
        {
            Canvas_perfil.Children.Clear();
            Canvas_aerea.Children.Clear();
            Datagrid_table.Items.Clear();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv|txt Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Label_file.Content = filename;

                StreamReader reader = new StreamReader(filename);
                //reader.ReadLine(); //The headers don't matter!

                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    string[] vals = currentLine.Split(';');
                    Tree newInfo = new Tree(int.Parse(vals[0]), vals[1], double.Parse(vals[2]), double.Parse(vals[3]), double.Parse(vals[4]), double.Parse(vals[5]), double.Parse(vals[6]), double.Parse(vals[7]), double.Parse(vals[8]));
                    //Actual parsing left up to the reader; String.Split is your friend.
                    Datagrid_table.Items.Add(newInfo);
                }
            }

            draw_perfil();
            draw_aerea();

            add_tickbars(Canvas_perfil, slider_H_perfil, slider_W_perfil, TickFrecuency_perfil);
            add_tickbars(Canvas_aerea, slider_H_aerea, slider_W_aerea, TickFrecuency_aerea);
        }

        private void draw_perfil()
        {
            double canvas_width = Canvas_perfil.ActualWidth / slider_W_perfil.Value;
            double canvas_height = Canvas_perfil.ActualHeight / slider_H_perfil.Value;

            foreach (Tree item in Datagrid_table.Items)
            {
                Rectangle treeTrunk = new Rectangle();
                Ellipse treecopa = new Ellipse();
                TextBlock textblo = new TextBlock();

                treeTrunk.Width = item.dap * canvas_width;
                treeTrunk.Height = item.htot * 100 * canvas_height;
                treecopa.Width = item.wcopa_x * 100 * canvas_width;
                treecopa.Height = (item.htot - item.hfust) * 100 * canvas_height;
                textblo.Text = item.ID.ToString();

                treeTrunk.Stroke = new SolidColorBrush(Colors.Black);
                treeTrunk.StrokeThickness = 1;
                treecopa.Stroke = new SolidColorBrush(Colors.Black);
                treecopa.StrokeThickness = 1;
                //treeTrunk.Fill = new SolidColorBrush(Colors.Red);

                string input = item.sp;
                char[] values = input.ToCharArray();
                int sum = 0;
                foreach (char letter in values)
                {
                    int value = Convert.ToInt32(letter);
                    sum += value;
                    //string hexOutput = String.Format("{0:X}", value);
                }
                string temp = "0." + sum.ToString();

                int temp2 = (int)Math.Floor((double.Parse(temp)) * 16777215);

                string color_string = "#" + String.Format("{0:X}", temp2);

                //Brush brush2 = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 233)));
                Color color = (Color)ColorConverter.ConvertFromString(color_string);
                color.A = (byte)180;
                Brush brush2 = new SolidColorBrush(color);
                treecopa.Fill = brush2;

                Canvas.SetLeft(treecopa, item.x * 100 * canvas_width);
                Canvas.SetTop(treecopa, treeTrunk.Height-treecopa.Height);
                Canvas.SetZIndex(treecopa, (int)item.y * 100);
                treecopa.RenderTransform = new TranslateTransform(-(treecopa.Width) / 2, 0);
                Canvas_perfil.Children.Add(treecopa);
                double coef = 0.5;
                Color color2 = Color.FromArgb(color.A, (byte)(color.R * coef), (byte)(color.G * coef), (byte)(color.B * coef));
                Brush brush = new SolidColorBrush(color2);
                treeTrunk.Fill = brush;
                Canvas.SetLeft(treeTrunk, item.x * 100 * canvas_width);
                //Canvas.SetTop(treeTrunk, treeTrunk.Height);
                Canvas.SetZIndex(treeTrunk, (int)item.y * 100);
                treeTrunk.RenderTransform = new TranslateTransform(-(treeTrunk.Width) / 2, 0);
                Canvas_perfil.Children.Add(treeTrunk);

                Canvas.SetLeft(textblo, item.x * 100 * canvas_width);
                Canvas.SetTop(textblo, (treeTrunk.Height));
                Canvas.SetZIndex(textblo, (int)item.y * 100);
                TransformGroup text_transform = new TransformGroup();
                text_transform.Children.Add(new ScaleTransform(1, -1));
                text_transform.Children.Add(new RotateTransform(90));
                textblo.RenderTransform = text_transform;
                Canvas_perfil.Children.Add(textblo);
                treecopa.Tag = treeTrunk;
                textblo.Tag = treeTrunk;
            }
        }

        private void draw_aerea()
        {
            double canvas_width = Canvas_aerea.ActualWidth / slider_W_aerea.Value;
            double canvas_height = Canvas_aerea.ActualHeight / slider_H_aerea.Value;

            foreach (Tree item in Datagrid_table.Items)
            {
                Ellipse treecopa = new Ellipse();
                TextBlock textblo = new TextBlock();

                treecopa.Width = item.wcopa_x * 100 * canvas_width;
                treecopa.Height = item.wcopa_y * 100 * canvas_height;
                textblo.Text = item.ID.ToString();

                treecopa.Stroke = new SolidColorBrush(Colors.Black);
                treecopa.StrokeThickness = 1;
                //treeTrunk.Fill = new SolidColorBrush(Colors.Red);

                string input = item.sp;
                char[] values = input.ToCharArray();
                int sum = 0;
                foreach (char letter in values)
                {
                    int value = Convert.ToInt32(letter);
                    sum += value;
                    //string hexOutput = String.Format("{0:X}", value);
                }
                string temp = "0." + sum.ToString();

                int temp2 = (int)Math.Floor((double.Parse(temp)) * 16777215);

                string color_string = "#" + String.Format("{0:X}", temp2);

                //Brush brush2 = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 233)));
                Color color = (Color)ColorConverter.ConvertFromString(color_string);
                color.A = (byte)180;
                Brush brush2 = new SolidColorBrush(color);
                treecopa.Fill = brush2;

                Canvas.SetLeft(treecopa, item.x * 100 * canvas_width);
                Canvas.SetTop(treecopa, (item.y * 100 * canvas_height));
                Canvas.SetZIndex(treecopa, (int)item.htot * 10);
                treecopa.RenderTransform = new TranslateTransform(-(treecopa.Width) / 2, -(treecopa.Height) / 2);
                Canvas_aerea.Children.Add(treecopa);
                
                //double coef = 0.5;
                //Color color2 = Color.FromArgb(color.A, (byte)(color.R * coef), (byte)(color.G * coef), (byte)(color.B * coef));
                //Brush brush = new SolidColorBrush(color2);

                Canvas.SetLeft(textblo, item.x * 100 * canvas_width);
                Canvas.SetTop(textblo,  (item.y * 100 * canvas_height));
                Canvas.SetZIndex(textblo, (int)item.htot * 10);
                //textblo.RenderTransform = new RotateTransform(-90);
                textblo.RenderTransform = new ScaleTransform(1, -1);
                textblo.Tag = treecopa;
                Canvas_aerea.Children.Add(textblo);

            }
        }




        private void add_tickbars(Canvas canvas, Slider slider_H, Slider slider_W, Xceed.Wpf.Toolkit.IntegerUpDown TickFrecuency)
        {
            CustomTickBar_H Tickbar_H = new CustomTickBar_H();
            Tickbar_H.Height = 20;
            Tickbar_H.Minimum = 0;

            Binding binding_slider_w_value = new Binding();
            binding_slider_w_value.Source = slider_W;
            binding_slider_w_value.Path = new PropertyPath("Value");
            binding_slider_w_value.Mode = BindingMode.OneWay;
            binding_slider_w_value.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Tickbar_H, CustomTickBar_H.MaximumProperty, binding_slider_w_value);

            Binding binding_tickfrecuency = new Binding();
            binding_tickfrecuency.Source = TickFrecuency;
            binding_tickfrecuency.Path = new PropertyPath("Value");
            binding_tickfrecuency.Mode = BindingMode.OneWay;
            binding_tickfrecuency.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Tickbar_H, CustomTickBar_H.TickFrequencyProperty, binding_tickfrecuency);
            
            Tickbar_H.RenderTransformOrigin = new Point(0.5, 0.5);
            Tickbar_H.UseLayoutRounding = false;

            Binding binding_canvas_width = new Binding();
            binding_canvas_width.Source = canvas;
            binding_canvas_width.Path = new PropertyPath("ActualWidth");
            binding_canvas_width.Mode = BindingMode.OneWay;
            binding_canvas_width.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Tickbar_H, CustomTickBar_H.WidthProperty, binding_canvas_width);

            Canvas.SetTop(Tickbar_H, 0);
            Tickbar_H.RenderTransformOrigin = new Point(0.5, 0.5);
            Tickbar_H.RenderTransform = new ScaleTransform(1, -1);

            canvas.Children.Add(Tickbar_H);

            //////////////////////////////////////////////////////////////////////////////////////
            CustomTickBar_V Tickbar_V = new CustomTickBar_V();
            Tickbar_V.Width = 20;
            Tickbar_V.Minimum = 0;

            Binding binding_slider_h_value = new Binding();
            binding_slider_h_value.Source = slider_H;
            binding_slider_h_value.Path = new PropertyPath("Value");
            binding_slider_h_value.Mode = BindingMode.OneWay;
            binding_slider_h_value.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Tickbar_V, CustomTickBar_V.MaximumProperty, binding_slider_h_value);


            BindingOperations.SetBinding(Tickbar_V, CustomTickBar_V.TickFrequencyProperty, binding_tickfrecuency);


            Tickbar_V.UseLayoutRounding = false;

            Binding binding_canvas_height = new Binding();
            binding_canvas_height.Source = canvas;
            binding_canvas_height.Path = new PropertyPath("ActualHeight");
            binding_canvas_height.Mode = BindingMode.OneWay;
            binding_canvas_height.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Tickbar_V, CustomTickBar_V.HeightProperty, binding_canvas_height);

            Canvas.SetBottom(Tickbar_V, 0);
            Canvas.SetLeft(Tickbar_V, 0);
            Tickbar_V.RenderTransformOrigin = new Point(0.5, 0.5);
            Tickbar_V.RenderTransform = new ScaleTransform(1, -1);
            canvas.Children.Add(Tickbar_V);
        }

        private void Datagrid_table_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "ID";
            c1.Binding = new Binding("ID");
            c1.Width = 110;
            Datagrid_table.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "sp";
            c2.Width = 110;
            c2.Binding = new Binding("sp");
            Datagrid_table.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "htot";
            c3.Width = 110;
            c3.Binding = new Binding("htot");
            Datagrid_table.Columns.Add(c3);

            DataGridTextColumn c4 = new DataGridTextColumn();
            c4.Header = "hfust";
            c4.Width = 110;
            c4.Binding = new Binding("hfust");
            Datagrid_table.Columns.Add(c4);

            DataGridTextColumn c5 = new DataGridTextColumn();
            c5.Header = "dap";
            c5.Width = 110;
            c5.Binding = new Binding("dap");
            Datagrid_table.Columns.Add(c5);

            DataGridTextColumn c6 = new DataGridTextColumn();
            c6.Header = "wcopa_x";
            c6.Width = 110;
            c6.Binding = new Binding("wcopa_x");
            Datagrid_table.Columns.Add(c6);

            DataGridTextColumn c7 = new DataGridTextColumn();
            c7.Header = "wcopa_y";
            c7.Width = 110;
            c7.Binding = new Binding("wcopa_y");
            Datagrid_table.Columns.Add(c7);

            DataGridTextColumn c8 = new DataGridTextColumn();
            c8.Header = "x";
            c8.Width = 110;
            c8.Binding = new Binding("x");
            Datagrid_table.Columns.Add(c8);

            DataGridTextColumn c9 = new DataGridTextColumn();
            c9.Header = "y";
            c9.Width = 110;
            c9.Binding = new Binding("y");
            Datagrid_table.Columns.Add(c9);
        }

        
        private void draw_gridlines(Canvas canvas, double canvas_width, double canvas_height)
        {
                VisualBrush brush3 = new VisualBrush
                {
                    Viewport = new Rect(0, 0, canvas_width * 100, canvas_height * 100),
                    ViewboxUnits = BrushMappingMode.Absolute,
                    ViewportUnits = BrushMappingMode.Absolute,
                    TileMode = TileMode.Tile,
                    Stretch = Stretch.Fill
                };
                Grid grid = new Grid { Width = canvas.ActualWidth, Height = canvas.ActualHeight };
                grid.Children.Add(new Rectangle
                {
                    Width = 1,
                    Height = 0.03,
                    Fill = new SolidColorBrush(Colors.LightGray),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                });
                grid.Children.Add(new Rectangle
                {
                    Height = 1,
                    Width = 0.03,
                    Fill = new SolidColorBrush(Colors.LightGray),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                });
                grid.Background = Brushes.AliceBlue;
                brush3.Visual = grid;
                canvas.Background = brush3;            
        }

        private void Canvas_perfil_Loaded(object sender, RoutedEventArgs e)
        {
            if ((Canvas_perfil != null) && (slider_H_perfil != null && slider_H_perfil.IsLoaded) && (slider_W_perfil != null && slider_W_perfil.IsLoaded))
            {
                double canvas_width = Canvas_perfil.ActualWidth / slider_W_perfil.Value;
                double canvas_height = Canvas_perfil.ActualHeight / slider_H_perfil.Value;
                draw_gridlines(Canvas_perfil,canvas_width, canvas_height);
            }
        }
        private void Canvas_aerea_Loaded(object sender, RoutedEventArgs e)
        {
            if ((Canvas_aerea != null) && (slider_H_aerea != null && slider_H_aerea.IsLoaded) && (slider_W_aerea != null && slider_W_aerea.IsLoaded))
            {
                double canvas_width = Canvas_aerea.ActualWidth / slider_W_aerea.Value;
                double canvas_height = Canvas_aerea.ActualHeight / slider_H_aerea.Value;
                draw_gridlines(Canvas_aerea, canvas_width, canvas_height);
            }
        }

        private void slider_W_perfil_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((Canvas_perfil != null) && (slider_H_perfil != null && slider_H_perfil.IsLoaded) && (slider_W_perfil != null && slider_W_perfil.IsLoaded))
            {
                double canvas_width = Canvas_perfil.ActualWidth / slider_W_perfil.Value;
                double canvas_height = Canvas_perfil.ActualHeight / slider_H_perfil.Value;
                draw_gridlines(Canvas_perfil, canvas_width, canvas_height);

                double old_scale = Canvas_perfil.ActualWidth / e.OldValue;
                double new_scale = Canvas_perfil.ActualWidth / e.NewValue;
                rescale_trees(old_scale, new_scale, true, Canvas_perfil);
            }
        }
        private void slider_H_perfil_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((Canvas_perfil != null) && (slider_H_perfil != null && slider_H_perfil.IsLoaded) && (slider_W_perfil != null && slider_W_perfil.IsLoaded))
            {
                double canvas_width = Canvas_perfil.ActualWidth / slider_W_perfil.Value;
                double canvas_height = Canvas_perfil.ActualHeight / slider_H_perfil.Value;
                draw_gridlines(Canvas_perfil, canvas_width, canvas_height);

                double old_scale = Canvas_perfil.ActualHeight / e.OldValue;
                double new_scale = Canvas_perfil.ActualHeight / e.NewValue;
                rescale_trees(old_scale, new_scale, false, Canvas_perfil);
            }
        }
        private void slider_W_aerea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((Canvas_aerea != null) && (slider_H_aerea != null && slider_H_aerea.IsLoaded) && (slider_W_aerea != null && slider_W_aerea.IsLoaded))
            {
                double canvas_width = Canvas_aerea.ActualWidth / slider_W_aerea.Value;
                double canvas_height = Canvas_aerea.ActualHeight / slider_H_aerea.Value;
                draw_gridlines(Canvas_aerea, canvas_width, canvas_height);

                double old_scale = Canvas_aerea.ActualWidth / e.OldValue;
                double new_scale = Canvas_aerea.ActualWidth / e.NewValue;
                rescale_trees(old_scale, new_scale, true, Canvas_aerea);
            }
        }
        private void slider_H_aerea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((Canvas_aerea != null) && (slider_H_aerea != null && slider_H_aerea.IsLoaded) && (slider_W_aerea != null && slider_W_aerea.IsLoaded))
            {
                double canvas_width = Canvas_aerea.ActualWidth / slider_W_aerea.Value;
                double canvas_height = Canvas_aerea.ActualHeight / slider_H_aerea.Value;
                draw_gridlines(Canvas_aerea, canvas_width, canvas_height);

                double old_scale = Canvas_aerea.ActualHeight / e.OldValue;
                double new_scale = Canvas_aerea.ActualHeight / e.NewValue;
                rescale_trees(old_scale, new_scale, false, Canvas_aerea);
            }
        }
        private void rescale_trees(double old_scale, double new_scale, bool width, Canvas canvas)
        {
            if (canvas != null)
            {
                if (canvas.Children.Count > 0)
                {
                    double proportion = new_scale / old_scale;
                    foreach (var item in canvas.Children)
                    {
                        Shape el = new Rectangle();
                        TextBlock el2 = new TextBlock();
                        bool istext = false;
                        bool canchange = false;
                        if (item is Ellipse)
                        {
                            el = item as Ellipse;
                            canchange = true;
                        }
                        if (item is Rectangle)
                        {
                            el = item as Rectangle;
                            canchange = true;
                        }
                        if (item is TextBlock)
                        {
                            el2 = item as TextBlock;
                            istext = true;
                            canchange = true;
                        }
                        if (!double.IsInfinity(proportion) && canchange)
                        {
                            if (width)
                            {
                                el.Width = el.Width * proportion;
                                //Canvas.SetLeft(el, Canvas.GetLeft(el) + (((Canvas.GetLeft(el) / temp)+(Canvas.GetLeft(el)/ canvas_width) + (changev*1)) * canvas_width));
                                //Canvas.SetLeft(el, ((Canvas.GetLeft(el) / temp) + changev / 100) * canvas_width);
                                Canvas.SetLeft(el, Canvas.GetLeft(el) * proportion);
                                if (canvas == Canvas_perfil)
                                {
                                    el.RenderTransform = new TranslateTransform(-(el.Width) / 2, 0);
                                }
                                else
                                {
                                    el.RenderTransform = new TranslateTransform(-(el.Width) / 2, -(el.Height) / 2);
                                }

                                Canvas.SetLeft(el2, Canvas.GetLeft(el2) * proportion);
                            }
                            else
                            {
                                el.Height = el.Height * proportion;
                                if (el is Ellipse)
                                {
                                    if (el.Tag is Rectangle)
                                    {
                                        Canvas.SetTop(el, (((el.Tag as Rectangle).ActualHeight - el.ActualHeight) * proportion));
                                    }
                                    else
                                    {
                                        //el.Tag = ((double)el.Tag) * proportion;
                                        el.RenderTransform = new TranslateTransform(-(el.Width) / 2, -(el.Height) / 2);
                                        Canvas.SetTop(el, Canvas.GetTop(el) * proportion);
                                    }
                                }
                                else
                                {//el is rectangle
                                    //Canvas.SetTop(el,  ( el.ActualHeight * proportion));
                                }
                                if (istext)
                                {
                                    if (el2.Tag is Rectangle)
                                    {
                                        Canvas.SetTop(el2, ((el2.Tag as Rectangle).ActualHeight * proportion));
                                    }
                                    else
                                    {                                        
                                        Canvas.SetTop(el2, Canvas.GetTop(el2.Tag as Ellipse) );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Canvas_perfil_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((Canvas_perfil != null) && (slider_H_perfil != null && slider_H_perfil.IsLoaded) && (slider_W_perfil != null && slider_W_perfil.IsLoaded))
            {
                double canvas_width = Canvas_perfil.ActualWidth / slider_W_perfil.Value;
                double canvas_height = Canvas_perfil.ActualHeight / slider_H_perfil.Value;
                draw_gridlines(Canvas_perfil, canvas_width, canvas_height);

                if (e.HeightChanged)
                {
                    double old_scale = e.PreviousSize.Height / slider_H_perfil.Value;
                    double new_scale = e.NewSize.Height / slider_H_perfil.Value;
                    rescale_trees(old_scale, new_scale, false, Canvas_perfil);
                }
                if (e.WidthChanged)
                {
                    double old_scale = e.PreviousSize.Width / slider_W_perfil.Value;
                    double new_scale = e.NewSize.Width / slider_W_perfil.Value;
                    rescale_trees(old_scale, new_scale, true, Canvas_perfil);
                }
            }
        }

        private void Canvas_aerea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((Canvas_aerea != null) && (slider_H_aerea != null && slider_H_aerea.IsLoaded) && (slider_W_aerea != null && slider_W_aerea.IsLoaded))
            {
                double canvas_width = Canvas_aerea.ActualWidth / slider_W_aerea.Value;
                double canvas_height = Canvas_aerea.ActualHeight / slider_H_aerea.Value;
                draw_gridlines(Canvas_aerea, canvas_width, canvas_height);

                if (e.HeightChanged)
                {
                    double old_scale = e.PreviousSize.Height / slider_H_aerea.Value;
                    double new_scale = e.NewSize.Height / slider_H_aerea.Value;
                    rescale_trees(old_scale, new_scale, false, Canvas_aerea);
                }
                if (e.WidthChanged)
                {
                    double old_scale = e.PreviousSize.Width / slider_W_aerea.Value;
                    double new_scale = e.NewSize.Width / slider_W_aerea.Value;
                    rescale_trees(old_scale, new_scale, true, Canvas_aerea);
                }
            }
        }

        private void Button_Click_Export(object sender, RoutedEventArgs e)
        {
            string filename = "perfil.png";
            export_image_png(filename, Canvas_perfil);
            filename = "aerea.png";
            export_image_png(filename, Canvas_aerea);
        }

        private void export_image_png(string filename, Canvas canvas)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();

                System.IO.File.WriteAllBytes(filename, ms.ToArray());
                MessageBox.Show("Sucssefully created "+filename,"Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
    
