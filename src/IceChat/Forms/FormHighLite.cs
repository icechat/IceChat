using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IceChat2009
{
    public partial class FormHighLite : Form
    {

        public delegate void SaveHighLiteDelegate(HighLiteItem hli, int listIndex);
        public event SaveHighLiteDelegate SaveHighLite;

        private ColorButtonArray colorPicker;
        private HighLiteItem highLiteItem;
        private int listIndex = 0;

        public FormHighLite(HighLiteItem hli, int index)
        {
            InitializeComponent();

            colorPicker = new ColorButtonArray(panelColorPicker);
            colorPicker.OnClick += new ColorButtonArray.ColorSelected(colorPicker_OnClick);
            
            this.highLiteItem = hli;
            this.listIndex = index;

            textHiLite.Text = highLiteItem.Match;

            textCommand.Text = highLiteItem.Command;
            if (highLiteItem.Color == 0)
            {
                colorPicker.SelectedColor = 1;
                highLiteItem.Color = 1;
            }
            else
            {
                colorPicker.SelectedColor = highLiteItem.Color;
            }
            
            textHiLite.ForeColor = IrcColor.colors[highLiteItem.Color];
            textHiLite.Tag = highLiteItem.Color;

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            highLiteItem.Match = textHiLite.Text;
            highLiteItem.Command = textCommand.Text;
            highLiteItem.Color = (int)textHiLite.Tag;
            
            //update or add new item
            if (SaveHighLite != null)
                SaveHighLite(this.highLiteItem, listIndex);

            this.Close();     
        }

        private void colorPicker_OnClick(int colorSelected)
        {
            //change the color of the textbox
            textHiLite.ForeColor = IrcColor.colors[colorSelected];
            textHiLite.Tag = colorSelected;
        }
    }

}
