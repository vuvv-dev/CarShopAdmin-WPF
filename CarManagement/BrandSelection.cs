using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarManagement;

public static class BrandSelection
{
    public static readonly DependencyProperty IsHoveredProperty =
        DependencyProperty.RegisterAttached(
            "IsHovered",
            typeof(bool),
            typeof(BrandSelection),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender)
        );

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.RegisterAttached(
            "IsSelected",
            typeof(bool),
            typeof(BrandSelection),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender)
        );

    public static bool GetIsHovered(DependencyObject obj) => (bool)obj.GetValue(IsHoveredProperty);

    public static void SetIsHovered(DependencyObject obj, bool value) =>
        obj.SetValue(IsHoveredProperty, value);

    public static bool GetIsSelected(DependencyObject obj) =>
        (bool)obj.GetValue(IsSelectedProperty);

    public static void SetIsSelected(DependencyObject obj, bool value) =>
        obj.SetValue(IsSelectedProperty, value);
}
