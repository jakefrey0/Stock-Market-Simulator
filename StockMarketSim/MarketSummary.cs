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
	
	public struct MarketSummary {
		
		/// <summary>
		/// How much the market is up or down by
		/// </summary>
		public readonly Single difference;
		
		/// <summary>
		/// The price of the market at the time of the summary
		/// </summary>
		public readonly Single value;
		
		/// <summary>
		/// Initialize a market summary.
		/// </summary>
		public MarketSummary (Single difference,Single value) {
			
			this.difference=difference;
			this.value=value;
			
		}
		
	}
	
}
