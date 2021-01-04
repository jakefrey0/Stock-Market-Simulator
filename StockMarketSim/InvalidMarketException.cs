/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 5:13 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace StockMarketSim {
	
	/// <summary>
	/// The exception for handling non-existant/invalid market
	/// </summary>
	internal class InvalidMarketException : Exception {
		
		/// <summary>
		/// The constructor for exception for handling non-existant/invalid market
		/// </summary>
		/// <param name="msg">The exception message</param>
		internal InvalidMarketException (String msg) : base ("InvalidMarketException: "+msg) { }
		
	}
	
}