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
		
		private Single cash=0,cashAdded=0;
		private MarketWatcher mw;
		private Byte markets=0;
		private Boolean changedDelay,watchingMarket,triedLockingCash;
		private Dictionary<String,UInt32> stocks;
		private Decimal profit=0;
		
		public MainForm () {
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.InitializeComponent();
			
			this.addCashTextBox.Text="100";
			this.addMarketTextBox.Text="GOOGL";
			this.offsetCash(100,true);
			this.mw=new MarketWatcher(new []{"OSPTX","AAPL","BBY"},30000);
			this.markets=3;
			this.changedDelay=false;
			this.watchingMarket=true;
			this.triedLockingCash=false;
			this.mw.onMarketUpdate+=this.gotMarketUpdate;
			this.stocks=new Dictionary<String,UInt32>();
			
		}
		
		private void AddCashBtnClick (Object sender,EventArgs e) {
			
			Single num=0;
			if (Single.TryParse(addCashTextBox.Text,out num))
				this.offsetCash(num,true);
			
			this.addCashTextBox.Text="";
			
		}
		
		private void offsetCash (Single value,Boolean illegitimate=false) {
			
			this.cash+=value;
			if ((this.cash>999999)&&(this.yourCashLabel.Font.Size==18F)) {
				this.yourCashLabel.Font=new Font("Microsoft Sans Serif",12F);
				this.yourCashLabel.Location=new Point(yourCashLabel.Location.X,yourCashLabel.Location.Y+6);
			}
			this.yourCashLabel.Text="Your cash: "+Math.Floor((Decimal)this.cash).ToString();
			
			if (illegitimate) {
				
				this.cashAdded+=value;
				this.cashAddedLbl.Text="Cash added:"+this.cashAdded.ToString();
				
			}
			
			else {
				this.profit+=(Decimal)value;
				this.profitLbl.Text="Profit: "+this.profit.ToString();
			}
			
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
			tp.Controls.Add(new Label(){Name=market+"_YourStocksLbl",Text="Your stocks: 0",Font=new Font("Verdana",12F),Size=new Size(this.marketTabs.Size.Width-4,20),Location=new Point(2,68)});
			tp.Controls.Add(new Button(){Name=market+"_PurchaseBtn",Text="Purchase stocks",Font=new Font("Verdana",12F),Size=new Size(100,45),Location=new Point(this.marketTabs.Size.Width-120,15),UseVisualStyleBackColor=true});
			Button btn=(tp.Controls.Cast<Control>().Where(x=>x.GetType()==typeof(Button)).First() as Button);
			TextBox tb=new TextBox(){Name=market+"_PurchaseStocksNumberTextBox",Text="1",Size=new Size(100,30),Location=new Point(this.marketTabs.Size.Width-120,60)};
			tp.Controls.Add(tb);
			tb.BringToFront();
			btn.BringToFront();
			btn.Click+=delegate { 
				
				UInt32 amount;
				if (UInt32.TryParse(tb.Text,out amount))
					this.purchaseStocks(market,amount);
				
				tb.Text="";
				
			};
			Button btn0=new Button(){Name=market+"_SellBtn",Text="Sell stocks",Font=new Font("Verdana",12F),Size=new Size(100,45),Location=new Point(this.marketTabs.Size.Width-120,100),UseVisualStyleBackColor=true};
			tp.Controls.Add(btn0);
			btn0.BringToFront();
			TextBox tb0=new TextBox(){Name=market+"_SellStocksNumberTextBox",Text="1",Size=new Size(100,30),Location=new Point(this.marketTabs.Size.Width-120,145)};
			tp.Controls.Add(tb0);
			tb0.BringToFront();
			btn0.Click+=delegate { 
				
				UInt32 amount;
				if (UInt32.TryParse(tb0.Text,out amount))
					this.sellStocks(market,amount);
				
				tb0.Text="";
				
			};
			
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
		
		private void AddMarketBtnClick (Object sender, EventArgs e) { this.addMarket(this.addMarketTextBox.Text.ToUpper()); }
		
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
		
		private void purchaseStocks (String market,UInt32 amount) {
			
			TabPage tp=this.marketTabs.TabPages.Cast<TabPage>().Where(x=>x.Name==market).First();
			
			Single price;
			Label priceLbl=(tp.Controls.Cast<Control>().Where(x=>x.Name==market+"_PriceLbl").First() as Label);
			if (Single.TryParse(priceLbl.Text.Split(':')[1].Replace(" ",""),out price)&&((price*amount)<this.cash))
				this.offsetCash((-price*amount));
			else if (priceLbl.Text.Contains("update..)")) {
				
				MessageBox.Show("Please wait for the update before purchasing stocks..");
				return;
				
			}
			else {
				
				MessageBox.Show("You cannot afford this stock! You have "+this.cash+" but you need "+(price*amount)+". (if you have the right amount of money on the dot, get 1 more cent because the precision isn't 100%)");
				return;
				
			}
			
			if (this.stocks.ContainsKey(market)) this.stocks[market]+=amount;
			else this.stocks.Add(market,amount);
			tp.Controls.Cast<Control>().Where(x=>x.Name==(market+"_YourStocksLbl")).First().Text="Your stocks: "+this.stocks[market].ToString();
			
		}
		
		private void sellStocks (String market,UInt32 amount) {
			
			TabPage tp=this.marketTabs.TabPages.Cast<TabPage>().Where(x=>x.Name==market).First();
			
			Single price;
			Label priceLbl=(tp.Controls.Cast<Control>().Where(x=>x.Name==market+"_PriceLbl").First() as Label);
			if (Single.TryParse(priceLbl.Text.Split(':')[1].Replace(" ",""),out price)&&(this.stocks[market]>=amount)) {
				
				this.offsetCash((price*amount));
				this.stocks[market]-=amount;
				tp.Controls.Cast<Control>().Where(x=>x.Name==(market+"_YourStocksLbl")).First().Text="Your stocks: "+this.stocks[market].ToString();
				
			}
			else if (priceLbl.Text.Contains("update..)")) {
				
				MessageBox.Show("Please wait for the update before selling stocks..");
				return;
				
			}
			else {
				
				MessageBox.Show("You don't have enough stocks.");
				return;
				
			}
			
			
		}
		
	}
	
}