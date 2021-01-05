/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 2:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StockMarketWrapper;
using System.Linq;

namespace StockMarketSimulator {
	
	/// <summary>
	/// The main form
	/// </summary>
	public partial class MainForm : Form {
		
		private Int32 cash=0;
		private MarketWatcher mw;
		private Byte markets=0;
		private Boolean changedDelay,watchingMarket;
		
		public MainForm () {
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.InitializeComponent();
			
			this.addCashTextBox.Text="100";
			this.addMarketTextBox.Text="GOOGL";
			this.offsetCash(100);
			this.mw=new MarketWatcher(new []{"OSPTX","AAPL","BBY"},30000);
			this.markets=3;
			this.changedDelay=false;
			this.watchingMarket=true;
			
		}
		
		private void AddCashBtnClick (Object sender,EventArgs e) {
			
			Int32 num=0;
			if (Int32.TryParse(addCashTextBox.Text,out num))
				this.offsetCash(num);
			
			this.addCashTextBox.Text="";
			
		}
		
		private void offsetCash (Int32 value) {
			
			this.cash+=value;
			if ((this.cash>999999)&&(this.yourCashLabel.Font.Size==18F)) {
				this.yourCashLabel.Font=new Font("Microsoft Sans Serif",12F);
				this.yourCashLabel.Location=new Point(yourCashLabel.Location.X,yourCashLabel.Location.Y+6);
			}
			this.yourCashLabel.Text="Your cash: "+this.cash.ToString();
			
		}
		
		private void addMarket (String market) {
			
			if (this.markets>10) {
				
				MessageBox.Show("Market limit reached!");
				return;
				
			}
			
			if (this.marketTabs.TabPages.Cast<TabPage>().Where(x=>x.Name==market).Count()>0) {
				
				MessageBox.Show("Market already exists!");
				return;
				
			}
			
			this.mw.addMarket(market);
			this.addMarketTabPage(market);
			++markets;
			if ((this.markets>6)&&(!this.changedDelay)) {
				this.mw.setDelay(60000);
				this.changedDelay=true;
			}
			
		}
		
		private void addMarketTabPage (String market) {
			
			TabPage tp=new TabPage(){Name=market,Text=market};
			tp.Controls.Add(new Label(){Name=market+"_PriceLbl",Text="Price: (Waiting for update..)",Font=new Font("Verdana",12F),Size=new Size(200,16),Location=new Point(2,2)});
			
			
			this.marketTabs.TabPages.Add(new TabPage(){Name=market,Text=market});
			
		}
		
		private void MainFormLoad (Object sender, EventArgs e) {
			
			this.addMarketTabPage("OSPTX");
			this.addMarketTabPage("AAPL");
			this.addMarketTabPage("BBY");
			this.mw.startWatching();
			
		}
		
		private void AddMarketBtnClick (Object sender, EventArgs e) { this.addMarket(this.addMarketTextBox.Text); }
		
		private void ToggleWatchingBtnClick(Object sender, EventArgs e) {
			
			if (this.watchingMarket) {
				
				
				
			}
			
			else {
				
				
				
			}
			
			this.watchingMarket=!this.watchingMarket;
			
		}
		
	}
	
}