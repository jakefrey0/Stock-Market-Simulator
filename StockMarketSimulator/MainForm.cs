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
		private Boolean changedDelay,watchingMarket,triedLockingCash;
		
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
			this.triedLockingCash=false;
			this.mw.onMarketUpdate+=this.gotMarketUpdate;
			
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
			tp.Controls.Add(new Label(){Name=market+"_PriceLbl",Text="Price: (Waiting for update..)",Font=new Font("Verdana",12F),Size=new Size(this.marketTabs.Size.Width-4,20),Location=new Point(2,2)});
			tp.Controls.Add(new Label(){Name=market+"_DifferenceLbl",Text="Difference: (Waiting for update..)",Font=new Font("Verdana",12F),Size=new Size(this.marketTabs.Size.Width-4,20),Location=new Point(2,24)});
			tp.Controls.Add(new Label(){Name=market+"_UpdateTimeLbl",Text="Last update time: (Waiting for update..)",Font=new Font("Verdana",12F),Size=new Size(this.marketTabs.Size.Width-4,20),Location=new Point(2,46)});
			
			
			this.marketTabs.TabPages.Add(tp);
			
		}
		
		private void MainFormLoad (Object sender, EventArgs e) {
			
			this.addMarketTabPage("OSPTX");
			this.addMarketTabPage("AAPL");
			this.addMarketTabPage("BBY");
			this.mw.startWatching();
			
			this.marketTabs.Anchor=AnchorStyles.Left|AnchorStyles.Top|AnchorStyles.Bottom;
			this.portfolioPanel.Anchor=AnchorStyles.Right|AnchorStyles.Top|AnchorStyles.Bottom;
			this.yourCashLabel.Anchor=AnchorStyles.Right|AnchorStyles.Bottom;
			this.addCashTextBox.Anchor=AnchorStyles.Right|AnchorStyles.Bottom;
			this.addMarketTextBox.Anchor=AnchorStyles.Right|AnchorStyles.Bottom;
			this.addMarketBtn.Anchor=AnchorStyles.Right|AnchorStyles.Bottom;
			this.addCashBtn.Anchor=AnchorStyles.Right|AnchorStyles.Bottom;
			
		}
		
		private void AddMarketBtnClick (Object sender, EventArgs e) { this.addMarket(this.addMarketTextBox.Text); }
		
		private void ToggleWatchingBtnClick (Object sender, EventArgs e) {
			
			if (this.watchingMarket) {
				
				this.mw.stopWatching();
				this.toggleWatchingBtn.Text="Start watching";
				
			}
			
			else {
				
				this.mw.startWatching();
				this.toggleWatchingBtn.Text="Stop watching";
				
			}
			
			this.watchingMarket=!this.watchingMarket;
			
		}
		
		private void gotMarketUpdate (MarketWatcher sender,MarketUpdateEventArgs args) {
			
			foreach (MarketSummary sum in args.info) {
				
				foreach (TabPage tp in this.marketTabs.TabPages) {
					
					if (tp.Name==sum.market) {
						
						foreach (Control c in tp.Controls) {
							
							if (c.Name==(sum.market+"_PriceLbl"))
								c.Invoke(new Action(()=>{c.Text="Price: "+sum.value.ToString();}));
							else if (c.Name==(sum.market+"_DifferenceLbl"))
								c.Invoke(new Action(()=>{c.Text="Difference: "+((sum.difference>0)?"+"+sum.difference.ToString()+" (↑)":sum.difference.ToString()+" (↓)");}));
							else if (c.Name==(sum.market+"_UpdateTimeLbl"))
								c.Invoke(new Action(()=>{c.Text="Last update time: "+args.time.ToString();}));
							
						}
						
						break;
						
					}
					
				}
				
			}
			
		}
		
		private void LockAddingCashBtnClick (Object sender, EventArgs e) {
			
			if (!(this.triedLockingCash)) {
				
				MessageBox.Show("Attention: If you click the \"Lock adding cash\" button again, you will lose the opportunity to add cash until you restart the application!!");
				this.triedLockingCash=true;
				return;
				
			}
			
			this.portfolioPanel.Controls.Remove(this.lockAddingCashBtn);
			this.portfolioPanel.Controls.Remove(this.addCashTextBox);
			this.portfolioPanel.Controls.Remove(this.addCashBtn);
			
		}
		
		private void MainFormFormClosing (Object sender, FormClosingEventArgs e) { Environment.Exit(0); }
		
	}
	
}