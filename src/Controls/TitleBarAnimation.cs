using Avalonia;
using Avalonia.Controls;
using System;
using System.Diagnostics;

namespace SourceGit.Controls
{
    /// <summary>
    /// TitleBar动画辅助类 - 已禁用（通过XAML静态定义）
    /// </summary>
    public static class TitleBarAnimation
    {
        public static readonly AttachedProperty<bool> EnableAnimationProperty =
            AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("EnableAnimation", typeof(TitleBarAnimation), false);

        static TitleBarAnimation()
        {
            EnableAnimationProperty.Changed.AddClassHandler<Border>(OnEnableAnimationChanged);
        }

        public static void SetEnableAnimation(AvaloniaObject element, bool value)
            element.SetValue(EnableAnimationProperty, value);
        }

        public static bool GetEnableAnimation(AvaloniaObject element)
        {
            return element.GetValue(EnableAnimationProperty);
        }

        private static void OnEnableAnimationChanged(Border border, AvaloniaPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"[TitleBarAnimation] OnEnableAnimationChanged called: {e.NewValue}");
            // 完全不修改Border，避免InvalidOperationException
            Debug.WriteLine("[TitleBarAnimation] Animation disabled - using XAML static gradient instead");
        }
    }
}
