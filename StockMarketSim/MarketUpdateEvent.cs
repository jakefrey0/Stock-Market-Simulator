/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 4:44 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace StockMarketWrapper {
	
	/// <summary>
	/// The market update event handler
	/// </summary>
	public delegate void MarketUpdateEventHandler (MarketWatcher sender,MarketUpdateEventArgs args);
	
	public class MarketUpdateEventArgs : EventArgs {
		
		/// <summary>
		/// The new information about the market
		/// </summary>
		public readonly MarketSummary[] info;
		
		/// <summary>
		/// The time of the update
		/// </summary>
		public readonly DateTime time;
		
		public MarketUpdateEventArgs (MarketSummary[] info,DateTime time) : base () { this.info=info; this.time=time; }
		
	}
	
}
