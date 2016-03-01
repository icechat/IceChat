/* To Do List
 * Mydialog = CreateDialog("Caption",x,y,width,height)
 * ShowDialog MyDialog
 * CloseDialog MyDialog
 * ChangeCaption MyDialog, "Caption"
 * AddButton MyDialog,"Caption",x,y,w,h,id
 * AddEditBox MyDialog,"text",x,y,w,h,id
 * AddLabel MyDialog,"Caption",x,y,w,h,id
 * AddListBox MyDialog,x,y,w,h,id,0,Name
 * AddComboBox MyDialog,x,y,w,h,id,0,Name
 * AddCheckBox MyDialog,"Caption",x,y,w,h,id
 * AddRadioButton MyDialog,x,y,w,h,id
 * MyValue = GetListItem(id, Type)  //Type - 4 = listbox , 5 = combobox
 * MyValue = SetListItem(id,index,Type)
 * MyValue = GetText(id)
 * AddItem id, Type, "text"
 * ClearItems id, Type
 * RemoveItem id, Type, index
 * 
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IceChatPlugin
{

    public delegate void ButtonClickedDelegate(string controlName);
    public delegate void ListboxSelectedDelegate(string controlName);
    public delegate void ComboboxSelectedDelegate(string controlName);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ClassDialog : Form
    {
        public event ButtonClickedDelegate ButtonClicked;
        //public event 

        public ClassDialog(string caption, int x, int y, int w, int h)
        {
            InitializeComponent();

            this.Text = caption;
            this.Location = new System.Drawing.Point(x, y);
            this.ClientSize = new System.Drawing.Size(w, h);
        }

        public ClassDialog() { }

        public void AddControl(int iType, string caption, int x, int y, int w, int h, string id, string controlName)
        {
            // types
            // 1 = button
            // 2 = textbox
            // 3 = label
            // 4 = listbox
            // 5 = combobox
            // 6 = checkbox
            // 7 = radiobutton
            switch (iType)
            {
                case 1:
                    //add a button to this form
                    Button b = new Button();
                    b.Width = w;
                    b.Height = h;
                    b.Location = new System.Drawing.Point(x, y);
                    b.Text = caption;
                    b.Tag = id;
                    if (controlName.Length > 0)
                    {
                        b.Name = controlName;
                        b.Click += new EventHandler(Button_Click);
                    }

                    b.Visible = true;
                    this.Controls.Add(b);
                    break;
                case 2:
                    //add a textbox to this form
                    TextBox tb = new TextBox();
                    tb.Width = w;
                    tb.Height = h;
                    tb.Location = new System.Drawing.Point(x, y);
                    tb.Text = caption;
                    tb.Tag = id;

                    tb.Visible = true;
                    this.Controls.Add(tb);
                    break;
                case 3:
                    //add a label
                    Label lb = new Label();
                    lb.Width = w;
                    lb.Height = h;
                    lb.Location = new System.Drawing.Point(x, y);
                    lb.Text = caption;
                    lb.Tag = id;

                    lb.Visible = true;
                    this.Controls.Add(lb);
                    break;
                case 4:
                    //add a listbox
                    ListBox lbx = new ListBox();
                    lbx.Width = w;
                    lbx.Height = h;
                    lbx.Location = new System.Drawing.Point(x, y);
                    lbx.Tag = id;
                    if (controlName.Length > 0)
                    {
                        lbx.Name = controlName;
                        lbx.SelectedValueChanged += new EventHandler(Listbox_SelectedValueChanged);
                    }

                    lbx.Visible = true;
                    this.Controls.Add(lbx);
                    break;
                case 5:
                    //add a combo/dropdown box
                    ComboBox cbx = new ComboBox();
                    cbx.Width = w;
                    cbx.Height = h;
                    cbx.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbx.Location = new System.Drawing.Point(x, y);
                    cbx.Tag = id;
                    if (controlName.Length > 0)
                    {
                        cbx.Name = controlName;
                        cbx.SelectedValueChanged += new EventHandler(Combobox_SelectedValueChanged);
                    }

                    cbx.Visible = true;
                    this.Controls.Add(cbx);
                    break;
                case 6:
                    //add a checkbox
                    break;
                case 7:
                    //add a radio button
                    break;
            }
        }

        private void Combobox_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("cbx changed:" + ((ComboBox)sender).Name + "::" + ((ComboBox)sender).Text);
        }

        private void Listbox_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("lbx changed:" + ((ListBox)sender).Name + "::" + ((ListBox)sender).Text);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            //throw the event for a button clicked
            System.Diagnostics.Debug.WriteLine("clicked:" + ((Button)sender).Name);
            if (ButtonClicked != null)
                ButtonClicked(((Button)sender).Name);
        }

        public Control FindControl(string id)
        {
            Control c = null;
            foreach (Control cc in this.Controls)
            {
                if (cc.Tag.ToString() == id)
                    c = cc;

            }
            
            return c;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ClassDialog
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassDialog";
            this.ResumeLayout(false);

        }

    }
}
