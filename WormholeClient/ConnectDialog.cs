using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for ConnectDialog.
	/// </summary>
	public class ConnectDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _bnConnect;
		private System.Windows.Forms.Button _bnCancel;
		private System.Windows.Forms.TextBox _textIP;
		private System.Windows.Forms.TextBox _textName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string TextIP
		{
			get{return this._textIP.Text;}
		}
		public string ClientName
		{
			get{return this._textName.Text;}
		}

		public ConnectDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._textIP = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._textName = new System.Windows.Forms.TextBox();
			this._bnConnect = new System.Windows.Forms.Button();
			this._bnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _textIP
			// 
			this._textIP.Location = new System.Drawing.Point(144, 24);
			this._textIP.Name = "_textIP";
			this._textIP.Size = new System.Drawing.Size(168, 20);
			this._textIP.TabIndex = 0;
			this._textIP.Text = "localhost";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Enter an IP Address:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Name to use:";
			// 
			// _textName
			// 
			this._textName.Location = new System.Drawing.Point(144, 64);
			this._textName.Name = "_textName";
			this._textName.Size = new System.Drawing.Size(168, 20);
			this._textName.TabIndex = 3;
			this._textName.Text = "";
			// 
			// _bnConnect
			// 
			this._bnConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._bnConnect.Location = new System.Drawing.Point(208, 104);
			this._bnConnect.Name = "_bnConnect";
			this._bnConnect.TabIndex = 4;
			this._bnConnect.Text = "Connect";
			// 
			// _bnCancel
			// 
			this._bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._bnCancel.Location = new System.Drawing.Point(64, 104);
			this._bnCancel.Name = "_bnCancel";
			this._bnCancel.TabIndex = 5;
			this._bnCancel.Text = "Cancel";
			// 
			// ConnectDialog
			// 
			this.AcceptButton = this._bnConnect;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._bnCancel;
			this.ClientSize = new System.Drawing.Size(344, 142);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._bnCancel,
																		  this._bnConnect,
																		  this._textName,
																		  this.label2,
																		  this.label1,
																		  this._textIP});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ConnectDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect to a Server";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
