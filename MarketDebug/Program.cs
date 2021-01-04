/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 5:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using StockMarketWrapper;

namespace MarketDebug {
	
	public class Program {
		
		[STAThread]
		public static void Main (String[] args) {
			
			if (args.Length!=0) {
				
				Console.WriteLine("No arguments expected.");
				return;
				
			}
			
			subRoutine:
			
			Console.Write("Market: ");
			
			MarketSummary sum=Market.getMarketSummary(Console.ReadLine());
			
			Console.WriteLine(Environment.NewLine+
			                  "Price:          " +sum.value.ToString()+Environment.NewLine+
			                  "Difference:     " +sum.difference.ToString()+Environment.NewLine);
			
			Console.ReadKey();
			Console.Clear();
			
			goto subRoutine;
			
		}
		
	}
	
}