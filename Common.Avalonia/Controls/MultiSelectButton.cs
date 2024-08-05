using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Common.Avalonia.Controls;

[PseudoClasses(NotSelect)]
[PseudoClasses(PartSelect)]
[PseudoClasses(AllSelect)]
public class MultiSelectButton : Button
{
    public const string NotSelect = ":not-select";
    public const string PartSelect = ":part-select";
    public const string AllSelect = ":all-select";
    public SelectStatus SelectStatus { get; private set; } = SelectStatus.NotSelect;

    public MultiSelectButton()
    {
        Classes.Add("Default");
        ChangeSelectStatus(SelectStatus.NotSelect);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
       
    }

    protected override void OnClick()
    {
        switch (SelectStatus)
        {
            case SelectStatus.NotSelect:
                SelectStatus = SelectStatus.AllSelect;
                break;
            case SelectStatus.PartSelect:
                SelectStatus = SelectStatus.AllSelect;
                break;
            case SelectStatus.AllSelect:
                SelectStatus = SelectStatus.NotSelect;
                break;
        }
        base.OnClick();
    }

    public void ChangeSelectStatus(SelectStatus status)
    {
        SelectStatus = status;
        SetPseudoClasses();
    }

    private void SetPseudoClasses()
    {
        PseudoClasses.Set(NotSelect, SelectStatus == SelectStatus.NotSelect);
        PseudoClasses.Set(PartSelect, SelectStatus == SelectStatus.PartSelect);
        PseudoClasses.Set(AllSelect, SelectStatus == SelectStatus.AllSelect);
    }
}

public enum SelectStatus
{
    /// <summary>
    /// 都未选择
    /// </summary>
    NotSelect,

    /// <summary>
    /// 部分选择
    /// </summary>
    PartSelect,

    /// <summary>
    /// 全部选择
    /// </summary>
    AllSelect
}