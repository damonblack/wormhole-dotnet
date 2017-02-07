using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for WMASystemLogView.
	/// </summary>
	public class SystemLogView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView		_sysLogListView;
		private System.Windows.Forms.ColumnHeader	_systemName;
		private System.Windows.Forms.ColumnHeader	_mapIndex;
		private System.Windows.Forms.ColumnHeader	_starType;
		private System.Windows.Forms.ColumnHeader	_organic;
		private System.Windows.Forms.ColumnHeader	_energy;
		private System.Windows.Forms.ColumnHeader	_percentInfluence;
		private System.Windows.Forms.ColumnHeader	_mineralCollected;
		private System.Windows.Forms.ColumnHeader	_organicCollected;
		private System.Windows.Forms.ColumnHeader	_energyCollected;
		private System.Windows.Forms.ColumnHeader	_mineral;
		#region Private Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region Properties
		#endregion
		#region Constructors and Initialization
		public SystemLogView(Player playerData)
		{
			InitializeComponent();
            SystemLog sysLog = playerData.SystemLog;
			int numItems = sysLog.Count;

			for(int i = 0; i < numItems; i++)
			{
				_sysLogListView.Items.Add(sysLog[i].ListViewItem);
			}
			
		}
		#endregion
		#region Methods
		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
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
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._sysLogListView = new System.Windows.Forms.ListView();
			this._systemName = new System.Windows.Forms.ColumnHeader();
			this._mapIndex = new System.Windows.Forms.ColumnHeader();
			this._starType = new System.Windows.Forms.ColumnHeader();
			this._mineral = new System.Windows.Forms.ColumnHeader();
			this._organic = new System.Windows.Forms.ColumnHeader();
			this._energy = new System.Windows.Forms.ColumnHeader();
			this._percentInfluence = new System.Windows.Forms.ColumnHeader();
			this._mineralCollected = new System.Windows.Forms.ColumnHeader();
			this._organicCollected = new System.Windows.Forms.ColumnHeader();
			this._energyCollected = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// _sysLogListView
			// 
			this._sysLogListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this._systemName,
																							  this._mapIndex,
																							  this._starType,
																							  this._mineral,
																							  this._organic,
																							  this._energy,
																							  this._percentInfluence,
																							  this._mineralCollected,
																							  this._organicCollected,
																							  this._energyCollected});
			this._sysLogListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sysLogListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._sysLogListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._sysLogListView.Name = "_sysLogListView";
			this._sysLogListView.Size = new System.Drawing.Size(512, 228);
			this._sysLogListView.TabIndex = 0;
			this._sysLogListView.View = System.Windows.Forms.View.Details;
			// 
			// _systemName
			// 
			this._systemName.Text = "Star System Name";
			this._systemName.Width = 110;
			// 
			// _mapIndex
			// 
			this._mapIndex.Text = "Location";
			this._mapIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._mapIndex.Width = 70;
			// 
			// _starType
			// 
			this._starType.Text = "Star Type";
			this._starType.Width = 80;
			// 
			// _mineral
			// 
			this._mineral.Text = "Min. Available";
			this._mineral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._mineral.Width = 30;
			// 
			// _organic
			// 
			this._organic.Text = "Org. Available";
			this._organic.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._organic.Width = 30;
			// 
			// _energy
			// 
			this._energy.Text = "Eng. Available";
			this._energy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._energy.Width = 30;
			// 
			// _percentInfluence
			// 
			this._percentInfluence.Text = "Influence";
			this._percentInfluence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._percentInfluence.Width = 40;
			// 
			// _mineralCollected
			// 
			this._mineralCollected.Text = "Min. Collected";
			this._mineralCollected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._mineralCollected.Width = 30;
			// 
			// _organicCollected
			// 
			this._organicCollected.Text = "Org. Collected";
			this._organicCollected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._organicCollected.Width = 30;
			// 
			// _energyCollected
			// 
			this._energyCollected.Text = "Eng. Collected";
			this._energyCollected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this._energyCollected.Width = 30;
			// 
			// SystemLogView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 228);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._sysLogListView});
			this.Name = "SystemLogView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "System Logs";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
