using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Layout;
    using Avalonia.Media;
    
    namespace TreeApp;
    
    public static class Extensions
    {
        #region Add Shapes to a Canvas
    
        // Add a Line to a Canvas.
        public static Line DrawLine(this Canvas canvas,
            Point point1, Point point2,
            Brush stroke, double stroke_thickness)
        {
            var line = new Line
            {
                StartPoint = point1,
                EndPoint = point2
            };
    
            line.SetShapeProperties(null, stroke, stroke_thickness);
            canvas?.Children?.Add(line);
            return line;
        }
    
        // Add a Rectangle to a Canvas.
        public static Rectangle DrawRectangle(this Canvas canvas,
            Rect bounds,
            Brush fill, Brush stroke, double stroke_thickness)
        {
            var rectangle = new Rectangle();
            rectangle.SetElementBounds(bounds);
            rectangle.SetShapeProperties(fill, stroke, stroke_thickness);
            canvas.Children.Add(rectangle);
            return rectangle;
        }
    
        // Add an Ellipse to a Canvas.
        public static Ellipse DrawEllipse(this Canvas canvas,
            Rect bounds,
            Brush fill, Brush stroke, double stroke_thickness)
        {
            var ellipse = new Ellipse();
            ellipse.SetElementBounds(bounds);
            ellipse.SetShapeProperties(fill, stroke, stroke_thickness);
            canvas?.Children?.Add(ellipse);
            return ellipse;
        }
    
        // Add a Label to a Canvas.
        public static Label DrawLabel(this Canvas canvas,
            Rect bounds, object content,
            Brush background, Brush foreground,
            HorizontalAlignment h_align,
            VerticalAlignment v_align,
            double font_size, double padding)
        {
            var label = new Label
            {
                Content = content,
                Foreground = foreground,
                Background = background,
                HorizontalContentAlignment = h_align,
                VerticalContentAlignment = v_align,
                FontSize = font_size,
                Padding = new Thickness(padding)
            };
            
            label.SetElementBounds(bounds);
            canvas?.Children?.Add(label);
            return label;
        }
    
        #endregion Add Shapes to a Canvas
    
        #region Set Shape Properties
    
        // Set an element's Canvas.Left, Canvas.Top, Width, and Height properties.
        public static void SetElementBounds(this Control element,
            Rect bounds)
        {
            Canvas.SetLeft(element, bounds.Left);
            Canvas.SetTop(element, bounds.Top);
            element.Width = bounds.Width;
            element.Height = bounds.Height;
        }
    
        // Set fill and outline drawing properties.
        public static void SetShapeProperties(this Shape shape,
            Brush fill, Brush stroke, double stroke_thickness)
        {
            shape.Fill = fill;
            shape.Stroke = stroke;
            shape.StrokeThickness = stroke_thickness;
        }
    
        #endregion Set Shape Properties
    }