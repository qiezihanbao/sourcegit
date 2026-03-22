using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SourceGit.Converters
{
    public class CommitColorToTransparentBrushConverter : IMultiValueConverter
    {
        // Default graph colors matching CommitGraph.s_defaultPenColors
        private static readonly Color[] s_graphColors =
        [
            Colors.Orange,
            Colors.ForestGreen,
            Colors.Turquoise,
            Colors.Olive,
            Colors.Magenta,
            Colors.Red,
            Colors.Khaki,
            Colors.Lime,
            Colors.RoyalBlue,
            Colors.Teal,
        ];

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // 检查是否启用渐变效果
            if (!ViewModels.Preferences.Instance.EnableRowGradientEffects)
            {
                return null;
            }

            if (values.Count >= 2 && values[0] is Models.Commit commit && values[1] is Models.Branch currentBranch)
            {
                int colorIndex = commit.Color;

                // 获取基础颜色
                Color baseColor;
                if (colorIndex >= 0 && colorIndex < s_graphColors.Length)
                {
                    baseColor = s_graphColors[colorIndex];
                }
                else
                {
                    baseColor = Colors.Gray;
                }

                // 检查是否为 ahead 或 behind 状态，如果是则使用中性灰色
                bool isAheadOrBehind = currentBranch != null &&
                    (!string.IsNullOrEmpty(commit.SHA) &&
                     (currentBranch.Ahead.Contains(commit.SHA) || currentBranch.Behind.Contains(commit.SHA)));

                if (isAheadOrBehind)
                {
                    baseColor = Color.FromRgb(128, 128, 128); // 中性灰色
                }

                // Log曲线渐变 - 只覆盖前30%，使用对数衰减
                // 并且只覆盖高度的85%（上下各7.5%透明）
                var gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0.075, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(1, 0.075, RelativeUnit.Relative),
                    SpreadMethod = GradientSpreadMethod.Pad,
                    GradientStops =
                    {
                        // 使用对数衰减：alpha = 60 * (1 - log(1 + x*10))
                        new GradientStop { Color = Color.FromArgb(60, baseColor.R, baseColor.G, baseColor.B), Offset = 0 },
                        new GradientStop { Color = Color.FromArgb(35, baseColor.R, baseColor.G, baseColor.B), Offset = 0.05 },
                        new GradientStop { Color = Color.FromArgb(20, baseColor.R, baseColor.G, baseColor.B), Offset = 0.1 },
                        new GradientStop { Color = Color.FromArgb(10, baseColor.R, baseColor.G, baseColor.B), Offset = 0.15 },
                        new GradientStop { Color = Color.FromArgb(5, baseColor.R, baseColor.G, baseColor.B), Offset = 0.2 },
                        new GradientStop { Color = Color.FromArgb(2, baseColor.R, baseColor.G, baseColor.B), Offset = 0.25 },
                        new GradientStop { Color = Color.FromArgb(0, baseColor.R, baseColor.G, baseColor.B), Offset = 0.3 }
                    }
                };

                return gradientBrush;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
