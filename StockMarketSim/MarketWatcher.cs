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

namespace StockMarketWrapper {
	
	public class MarketWatcher {
		
		/// <summary>
		/// Called whenever the MarketWatcher updates
		/// </summary>
		public event MarketUpdateEventHandler onMarketUpdate;
		
		/// <summary>
		/// Initialize a market watcher that can watch and update multiple markets
		/// </summary>
		/// <param name="markets">The markets to watch (i.e OSPTX, DAX)</param>
		/// <param name="updateDelay">The update delay in milliseconds</param>
		public MarketWatcher (String markets,UInt16 updateDelay) {
			
			
			
		}
		
	}
	
}