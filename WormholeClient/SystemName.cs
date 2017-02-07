using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for SystemName.
	/// </summary>
	public class SystemName : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _tbSystemName;
		private System.Windows.Forms.Button _bnOK;
		private System.Windows.Forms.Button _bnCancel;
		public string SysName
		{
			get{return _tbSystemName.Text;}
		}
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SystemName()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.label1 = new System.Windows.Forms.Label();
			this._tbSystemName = new System.Windows.Forms.TextBox();
			this._bnOK = new System.Windows.Forms.Button();
			this._bnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 64);
			this.label1.TabIndex = 0;
			this.label1.Text = "You have achieved founding influence over this system, would you like to give it " +
				"a name?";
			// 
			// _tbSystemName
			// 
			this._tbSystemName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._tbSystemName.Location = new System.Drawing.Point(8, 72);
			this._tbSystemName.Name = "_tbSystemName";
			this._tbSystemName.Size = new System.Drawing.Size(232, 26);
			this._tbSystemName.TabIndex = 1;
			this._tbSystemName.Text = "";
			// 
			// _bnOK
			// 
			this._bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._bnOK.Location = new System.Drawing.Point(144, 112);
			this._bnOK.Name = "_bnOK";
			this._bnOK.Size = new System.Drawing.Size(72, 24);
			this._bnOK.TabIndex = 0;
			this._bnOK.Text = "OK";
			// 
			// _bnCancel
			// 
			this._bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._bnCancel.Location = new System.Drawing.Point(32, 112);
			this._bnCancel.Name = "_bnCancel";
			this._bnCancel.Size = new System.Drawing.Size(72, 24);
			this._bnCancel.TabIndex = 3;
			this._bnCancel.Text = "Cancel";
			// 
			// SystemName
			// 
			this.AcceptButton = this._bnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._bnCancel;
			this.ClientSize = new System.Drawing.Size(248, 148);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._bnCancel,
																		  this._bnOK,
																		  this._tbSystemName,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SystemName";
			this.Text = "System Name";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
