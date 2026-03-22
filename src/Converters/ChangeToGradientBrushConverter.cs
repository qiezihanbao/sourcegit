using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SourceGit.Converters
{
    public class ChangeToGradientBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // 检查是否启用渐变效果
            if (!ViewModels.Preferences.Instance.EnableRowGradientEffects)
            {
                return null;
            }

            if (value is Models.Change change)
            {
                var state = change.Index != Models.ChangeState.None ? change.Index : change.WorkTree;
                var baseColor = GetGradientColor(state);

                // 使用40%透明度 (100/255 ≈ 40%)
                var gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0.5, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(0.3, 0.5, RelativeUnit.Relative),
                    GradientStops =
                    {
                        new GradientStop { Color = Color.FromArgb(100, baseColor.R, baseColor.G, baseColor.B), Offset = 0 },
                        new GradientStop { Color = Color.FromArgb(0, baseColor.R, baseColor.G, baseColor.B), Offset = 1 }
                    }
                };

                return gradientBrush;
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color GetGradientColor(Models.ChangeState state)
        {
            return state switch
            {
                Models.ChangeState.Added => Color.FromRgb(76, 175, 80),      // 绿色
                Models.ChangeState.Deleted => Color.FromRgb(229, 115, 115),    // 红色
                Models.ChangeState.Modified => Color.FromRgb(255, 183, 77),   // 黄色
                Models.ChangeState.Untracked => Color.FromRgb(129, 212, 250),   // 蓝色
                Models.ChangeState.Conflicted => Color.FromRgb(255, 138, 101),  // 橙红色
                Models.ChangeState.Renamed => Color.FromRgb(149, 117, 205),    // 紫色
                Models.ChangeState.Copied => Color.FromRgb(77, 182, 172),     // 青色
                Models.ChangeState.TypeChanged => Color.FromRgb(240, 98, 146), // 粉色
                _ => Colors.Transparent
            };
        }
    }
}
