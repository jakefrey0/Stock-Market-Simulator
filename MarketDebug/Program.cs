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
			
			MarketSummary sum=default(MarketSummary);
			
			try { sum=MarketFactory.getMarketSummary(Console.ReadLine()); }
			catch (InvalidMarketException) {
				
				Program.writeError("Invalid market.");
				goto subRoutine;
				
			}
			catch (Exception) {
				
				Program.writeError("An unexpected exception occurred.");
				goto subRoutine;
				
			}
			
			Console.WriteLine(Environment.NewLine+
			                  "Price:      " +sum.value.ToString()+Environment.NewLine+
			                  "Difference: " +((sum.difference>0)?"+"+sum.difference.ToString()+" (↑)":sum.difference.ToString()+" (↓)")+Environment.NewLine);
			
			Console.ReadKey();
			Console.Clear();
			
			goto subRoutine;
			
		}
		
		public static void writeError (String msg) {
		
			Console.Clear();
			Console.ForegroundColor=ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ForegroundColor=ConsoleColor.Gray;
			
		}
		
	}
	
}