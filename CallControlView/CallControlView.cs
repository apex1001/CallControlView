/*
 * Call Control toolbar for IE.
 * 
 * @Author: V. Vogelesang
 * @email : apex1001@home.nl
 * 
 * Based on the tutorials and fixes @
 * http://cgeers.com/2008/02/16/internet-explorer-toolbar/#bandobjects
 * http://www.codeproject.com/Articles/2219/Extending-Explorer-with-Band-Objects-using-NET-and
 * http://weblogs.com.pk/kadnan/articles/1500.aspx
 * http://www.codeproject.com/Articles/19820/Issues-faced-while-extending-IE-with-Band-Objects
 * 
 * Many thanx to aforementioned coders :-).
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;

using BandObjectLib;
using System.Runtime.InteropServices;

/**
 * Call control view class
 * controlled by BHOController
 * 
 */
namespace CallControlView
{
    [Guid("E0DE0DE0-46D4-4a98-AF68-0333EA26E113")]
    [BandObject("CallControlView", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar, HelpText = "Call Control bar")]
     public class CallControlView : BandObject
    {
        private System.ComponentModel.Container components = null;        
        private System.Windows.Forms.ToolStripButton offHookButton;
        private System.Windows.Forms.ToolStripButton onHookButton;
        private System.Windows.Forms.ToolStripButton transferButton;
        private System.Windows.Forms.ToolStripComboBox comboBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton dropMenu;
        private System.Windows.Forms.ToolStripMenuItem settings;
        private System.Windows.Forms.ToolStripMenuItem history;

        //SendMessage from Win32 API. Needed for handling accelerator/control keys in text/combobox
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, Int32 lParam);
        [DllImport("user32.dll")]
        public static extern int TranslateMessage(ref MSG lpMsg);
        [DllImport("user32", EntryPoint = "DispatchMessage")]
        static extern bool DispatchMessage(ref MSG msg);

        /**
         * CallControlView constructor
         */
        public CallControlView()
        {
            InitializeComponent();

            // Special renderer for transparent toolstrip border
            this.toolStrip.Renderer = new NoBorderToolStripRenderer();  
        }

        /**
         * Call controller on offHook/dial event. 
         * Looks for active/selected number in combobox
         * 
         */
        private void offHookButton_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("offHook!" + this.comboBox.Text);
        }

        /**
         * Call controller on onHook/hangup event. 
         * Looks for active/selected number in combobox
         *       
         */
        private void onHookButton_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("onHook!" + this.comboBox.Text);
        }

        /**
         * Call controller on transfer event. 
         * Looks for from(0)/to(1) items in combobox Items list
         *       
         */
        private void transferButton_Click(object sender, System.EventArgs e)
        {
            if (this.comboBox.Items.Count == 2)
            {
                MessageBox.Show("Transfer from: " + this.comboBox.Items[0].ToString() + " to: " + this.comboBox.Items[1].ToString());
            }
        }

        /**
         * Send combobox focus to parent BandObject
         */
        private void comboBox_GotFocus(object sender, System.EventArgs e)
        {
            this.OnGotFocus(e);
        }


        /**
         * Open the settings view
         * 
         */
        private void settings_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("settings!");
        }

        /**
         * Open the history view
         * 
         */
        private void history_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("history!" );           
        } 


        
        /*
         * ToolStripSystemRenderer 
         * Overrides OnRenderToolStripBorder to avoid painting the borders.
         */
        internal class NoBorderToolStripRenderer : ToolStripSystemRenderer
        {
            // Do nothing i.e. don't draw the toolstripborder at all
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        }

        /**
         * Catch focus event on text/combobox
         * used for accelerator keys in text/combobox.
         * 
         */
        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            this.OnGotFocus(e);
        }

        /**
         * Override accelerator key method for text/combobox
         * 
         */
        public override int TranslateAcceleratorIO(ref MSG msg)
        {
            //const int WM_CHAR = 0x0102;
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
            return 0; //S_OK
        }

        /**
         * Create all GUI elements
         * 
         */
        private void InitializeComponent()
        {
            // Init objects
            this.toolStrip = new ToolStrip();
            this.offHookButton = new ToolStripButton();
            this.onHookButton = new ToolStripButton();
            this.transferButton = new ToolStripButton();
            this.comboBox = new ToolStripComboBox();

            System.Drawing.Size buttonSize = new System.Drawing.Size(27, 27);

            // Suspend layout
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();

            // Initialize buttons
            this.offHookButton.AutoSize = false;
            this.offHookButton.Size = buttonSize;
            this.offHookButton.BackgroundImageLayout = ImageLayout.Stretch;
            this.offHookButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.landline_offhook));
            this.offHookButton.Click += new System.EventHandler(this.offHookButton_Click);

            this.onHookButton.AutoSize = false;
            this.onHookButton.Size = buttonSize;
            this.onHookButton.BackgroundImageLayout = ImageLayout.Stretch;
            this.onHookButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.landline_onhook));
            this.onHookButton.Click += new System.EventHandler(this.onHookButton_Click);

            this.transferButton.AutoSize = false;
            this.transferButton.Size = buttonSize;           
            this.transferButton.BackgroundImageLayout = ImageLayout.Stretch;
            this.transferButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.transfer));
            this.transferButton.Click += new System.EventHandler(this.transferButton_Click);

            // Initialize combobox
            this.comboBox = new ToolStripComboBox();
            this.comboBox.AutoSize = false;
            this.comboBox.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.comboBox.Size = new System.Drawing.Size(115, 28);
            this.comboBox.Items.AddRange(new object[] { "220", "230" });            
            this.comboBox.GotFocus += new EventHandler(comboBox_GotFocus);

            // Initialize drop menu
            var menuWidth = 80;            

            this.settings = new System.Windows.Forms.ToolStripMenuItem();             
            this.settings.Name = "settingsMenuItem"; 
            this.settings.Size = new System.Drawing.Size(menuWidth, 22);
            this.settings.Text = "Instellingen";
            this.settings.Click += new EventHandler(settings_Click);
            
            this.history = new System.Windows.Forms.ToolStripMenuItem();
            this.history.Name = "historyMenuItem";           
            this.history.Size = new System.Drawing.Size(menuWidth, 22);
            this.history.Text = "Historie";
            this.history.Click += new EventHandler(history_Click);

            this.dropMenu = new ToolStripDropDownButton();
            this.dropMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { settings, history });

            // Initialize toolstrip and add buttons
            this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip.Dock = DockStyle.None;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.AutoSize = false;
            this.toolStrip.Size = new System.Drawing.Size(220, 32);
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Items.AddRange(new ToolStripItem[] { comboBox, offHookButton, onHookButton, transferButton, dropMenu });

            // Add everything to CallControl toolbar
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.toolStrip });
            this.MinSize = new System.Drawing.Size(210, 32);
            this.BackColor = System.Drawing.Color.Transparent;

            // Perform final layout
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /**
         * Dispose of toolbar
         * 
         */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}