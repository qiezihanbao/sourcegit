using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using SourceGit.Models;

namespace SourceGit.Views
{
    public partial class GradientTextBlock : UserControl
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<GradientTextBlock, string>(nameof(Text), defaultValue: string.Empty);

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly StyledProperty<Change> ChangeProperty =
            AvaloniaProperty.Register<GradientTextBlock, Change>(nameof(Change), defaultValue: null);

        public Change Change
        {
            get => GetValue(ChangeProperty);
            set => SetValue(ChangeProperty, value);
        }

        public GradientTextBlock()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ChangeProperty)
            {
                UpdateGradient();
            }
            else if (change.Property == TextProperty)
            {
                if (TextBlock != null)
                    TextBlock.Text = change.NewValue?.ToString();
            }
        }

        private void UpdateGradient()
        {
            if (GradientBorder == null || Change == null)
                return;

            var state = Change.Index != ChangeState.None ? Change.Index : Change.WorkTree;
            var gradientStopColor = GetGradientColor(state);

            // 创建带透明度的渐变
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative),
                EndPoint = new RelativePoint(0.3, 0.5, RelativeUnit.Relative),
                GradientStops =
                {
                    new GradientStop { Color = Color.FromArgb(25, gradientStopColor.R, gradientStopColor.G, gradientStopColor.B), Offset = 0 },
                    new GradientStop { Color = Color.FromArgb(0, gradientStopColor.R, gradientStopColor.G, gradientStopColor.B), Offset = 1 }
                }
            };

            GradientBorder.Background = gradientBrush;
        }

        private Color GetGradientColor(ChangeState state)
        {
            return state switch
            {
                ChangeState.Added => Color.FromRgb(76, 175, 80),      // 低饱和度绿色，带透明度
                ChangeState.Deleted => Color.FromRgb(229, 115, 115),    // 低饱和度红色
                ChangeState.Modified => Color.FromRgb(255, 183, 77),   // 低饱和度黄色
                ChangeState.Untracked => Color.FromRgb(129, 212, 250),   // 浅蓝色
                ChangeState.Conflicted => Color.FromRgb(255, 138, 101),  // 橙红色
                ChangeState.Renamed => Color.FromRgb(149, 117, 205),    // 浅紫色
                ChangeState.Copied => Color.FromRgb(77, 182, 172),     // 青色
                ChangeState.TypeChanged => Color.FromRgb(240, 98, 146), // 粉色
                _ => Colors.Transparent
            };
        }
    }
}
