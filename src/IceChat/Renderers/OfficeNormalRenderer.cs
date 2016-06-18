using System;
using System.Collections;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;

public class OfficeNormalColorTable : ProfessionalColorTable
{

    public override Color ButtonSelectedHighlight
    {
        get { return Color.White; }
    }

    public override Color ButtonSelectedHighlightBorder
    {
        get { return Color.White; }
    }

    public override Color ButtonPressedHighlight
    {
        get { return Color.White; }
    }

    public override Color ButtonPressedHighlightBorder
    {
        get { return Color.White; }
    }

    public override Color ButtonCheckedHighlight
    {
        get { return Color.White; }
    }

    public override Color ButtonCheckedHighlightBorder
    {
        get { return Color.White; }
    }

    public override Color ButtonPressedBorder
    {
        get { return Color.FromArgb(251, 140, 60); }
    }

    public override Color ButtonSelectedBorder
    {
        get { return Color.FromArgb(255, 189, 105); }
    }

    public override Color ButtonCheckedGradientBegin
    {
        get { return Color.FromArgb(255, 207, 146); }
    }

    public override Color ButtonCheckedGradientMiddle
    {
        get { return Color.FromArgb(255, 189, 105); }
    }

    public override Color ButtonCheckedGradientEnd
    {
        get { return Color.FromArgb(255, 175, 73); }
    }

    public override Color ButtonSelectedGradientBegin
    {
        get { return Color.FromArgb(255, 245, 204); }
    }

    public override Color ButtonSelectedGradientMiddle
    {
        get { return Color.FromArgb(255, 230, 162); }
    }

    public override Color ButtonSelectedGradientEnd
    {
        get { return Color.FromArgb(255, 218, 117); }
    }

    public override Color ButtonPressedGradientBegin
    {
        get { return Color.FromArgb(252, 151, 61); }
    }

    public override Color ButtonPressedGradientMiddle
    {
        get { return Color.FromArgb(255, 171, 63); }
    }

    public override Color ButtonPressedGradientEnd
    {
        get { return Color.FromArgb(255, 184, 94); }
    }

    public override Color CheckBackground
    {
        //UNSURE
        get { return Color.FromArgb(255, 171, 63); }
    }

    public override Color CheckSelectedBackground
    {
        //UNSURE
        get { return Color.FromArgb(252, 151, 61); }
    }

    public override Color CheckPressedBackground
    {
        get { return Color.FromArgb(252, 151, 61); }
    }

    public override Color GripDark
    {
        get { return Color.FromArgb(111, 157, 217); }
    }

    public override Color GripLight
    {
        get { return Color.White; }
    }

    public override Color ImageMarginGradientBegin
    {
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color ImageMarginGradientMiddle
    {
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color ImageMarginGradientEnd
    {
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color ImageMarginRevealedGradientBegin
    {
        //UNSURE
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color ImageMarginRevealedGradientMiddle
    {
        //UNSURE
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color ImageMarginRevealedGradientEnd
    {
        //UNSURE
        get { return Color.FromArgb(233, 238, 238); }
    }

    public override Color MenuStripGradientBegin
    {
        get { return Color.FromArgb(191, 219, 255); }
    }

    public override Color MenuStripGradientEnd
    {
        get { return Color.FromArgb(191, 219, 255); }
    }

    public override Color MenuItemSelected
    {
        get { return Color.FromArgb(255, 231, 162); }
    }

    public override Color MenuItemBorder
    {
        get { return Color.FromArgb(255, 189, 105); }
    }

    public override Color MenuBorder
    {
        get { return Color.FromArgb(101, 147, 207); }
    }

    public override Color MenuItemSelectedGradientBegin
    {
        get { return Color.FromArgb(252, 151, 61); }
    }

    public override Color MenuItemSelectedGradientEnd
    {
        get { return Color.FromArgb(255, 218, 117); }
    }

    public override Color MenuItemPressedGradientBegin
    {
        get { return Color.FromArgb(226, 239, 255); }
    }

    public override Color MenuItemPressedGradientMiddle
    {
        get { return Color.FromArgb(190, 215, 247); }
    }

    public override Color MenuItemPressedGradientEnd
    {
        get { return Color.FromArgb(153, 191, 240); }
    }

    public override Color RaftingContainerGradientBegin
    {
        //UNSURE
        get { return Color.White; }
    }

    public override Color RaftingContainerGradientEnd
    {
        //UNSURE
        get { return Color.White; }
    }

    public override Color SeparatorDark
    {
        get { return Color.FromArgb(154, 198, 255); }
    }

    public override Color SeparatorLight
    {
        get { return Color.White; }
    }

    public override Color ToolStripBorder
    {
        get { return Color.FromArgb(111, 157, 217); }
    }

    public override Color ToolStripDropDownBackground
    {
        get { return Color.FromArgb(246, 246, 246); }
    }

    public override Color ToolStripGradientBegin
    {
        get { return Color.FromArgb(227, 239, 255); }
    }

    public override Color ToolStripGradientMiddle
    {
        get { return Color.FromArgb(218, 234, 255); }
    }

    public override Color ToolStripGradientEnd
    {
        get { return Color.FromArgb(177, 211, 255); }
    }

    public override Color ToolStripContentPanelGradientBegin
    {
        //UNSURE
        get { return Color.FromArgb(215, 232, 255); }
    }

    public override Color ToolStripContentPanelGradientEnd
    {
        //UNSURE
        get { return Color.FromArgb(111, 157, 217); }
    }

    public override Color ToolStripPanelGradientBegin
    {
        get { return Color.FromArgb(191, 219, 255); }
    }

    public override Color ToolStripPanelGradientEnd
    {
        get { return Color.FromArgb(191, 219, 255); }
    }

    public override Color OverflowButtonGradientBegin
    {
        get { return Color.FromArgb(215, 232, 255); }
    }

    public override Color OverflowButtonGradientMiddle
    {
        get { return Color.FromArgb(167, 204, 251); }
    }

    public override Color OverflowButtonGradientEnd
    {
        get { return Color.FromArgb(111, 157, 217); }
    }

}
