using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Common.Avalonia.Controls;

public class ToggleRadioButton : RadioButton
{
    protected override void Toggle()
    {
        var isChecked = IsChecked.GetValueOrDefault();
        SetCurrentValue(IsCheckedProperty, isChecked != true);
    }

    protected override Type StyleKeyOverride => typeof(CheckBox);
}