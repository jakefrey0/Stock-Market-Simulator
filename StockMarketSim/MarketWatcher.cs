/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 2:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace StockMarketWrapper {
	
	public class MarketWatcher {
		
		/// <summary>
		/// Called whenever the MarketWatcher updates
		/// </summary>
		public event MarketUpdateEventHandler onMarketUpdate;
		
		/// <summary>
		/// This is the list of markets that will be watched
		/// i.e OSPTX, DAX, GOOGL
		/// </summary>
		private List<String> markets;
		
		/// <summary>
		/// The market refresh delay
		/// </summary>
		private UInt32 updateDelay;
		
		/// <summary>
		/// This is set to true when Market#stopWatching is called
		/// Then, in the task where the watching takes place, it will
		/// check if it is true and cancel itself if so per iteration.
		/// </summary>
		private Boolean stopWatchingRequested;
		
		/// <summary>
		/// Initialize a market watcher that can watch and update multiple markets
		/// </summary>
		/// <param name="markets">The markets to watch (i.e OSPTX, DAX)</param>
		/// <param name="updateDelay">The minimum update delay in milliseconds</param>
		public MarketWatcher (IEnumerable<String> markets,UInt32 updateDelay) {
			
			this.markets=new List<String>(markets);
			this.updateDelay=updateDelay;
			
		}
		
		/// <summary>
		/// Start watching the market
		/// </summary>
		public void startWatching () {
			
			this.stopWatchingRequested=false;
			Thread t=new Thread(watch);
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			
		}
		
		/// <summary>
		/// Request to stop watching the market
		/// </summary>
		public void stopWatching () { this.stopWatchingRequested=true; }
		
		/// <summary>
		/// This is not meant to be called outside of MarketWatcher#startWatching
		/// and certainly not outside of it's own thread.
		/// </summary>
		private void watch () {
			
			DateTime start;
			List<MarketSummary> summaries;
			
			reWatch:
			
			start=DateTime.UtcNow;
			summaries=new List<MarketSummary>(this.markets.Count);
			
			if (this.stopWatchingRequested) {
				
				this.stopWatchingRequested=false;
				return;
				
			}
			
			foreach (String s in this.markets) {
				
				try { summaries.Add(Market.getMarketSummary(s)); }
				catch (InvalidMarketException) { this.markets.RemoveAll(x=>x==s); }
				catch (Exception e) { /* ??? */ throw e; }
				
			}
			
			if (!(this.onMarketUpdate==null))
				this.onMarketUpdate.Invoke(this,new MarketUpdateEventArgs(summaries.ToArray(),DateTime.Now));
			
			retry:
			TimeSpan ts=DateTime.UtcNow-start;
			if (ts.TotalMilliseconds<this.updateDelay) {
				
				Thread.Sleep(1500);
				goto retry;
				
			}
			
			goto reWatch;
			
		}
		
		/// <summary>
		/// Add a market to the watcher, working in real time.
		/// </summary>
		/// <param name="market">The market to add (i.e GOOGL)</param>
		public void addMarket (String market) {
			
			if (!(this.markets.Contains(market)))
				this.markets.Add(market);
			
		}
		
		/// <summary>
		/// Change the delay, updates in real time
		/// </summary>
		/// <param name="newDelay">The new delay in milliseconds</param>
		public void setDelay (UInt32 newDelay) { this.updateDelay=newDelay; }
		
	}
	
}