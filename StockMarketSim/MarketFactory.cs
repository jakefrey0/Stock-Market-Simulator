/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 2:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Windows.Forms;
using System.Linq;
using System.Threading;

namespace StockMarketWrapper {
	
	public static class MarketFactory {
		
		/// <summary>
		/// Get the current summary of a market
		/// Disclaimer: there is a delay on some markets (https://www.google.com/intl/en_ca/googlefinance/disclaimer)
		/// Expected to run in a STAThread environment
		/// </summary>
		/// <param name="market">The market (i.e OSPTX)</param>
		/// <returns>The updated summary of the market</returns>
		public static MarketSummary getMarketSummary (String market) {
			
			String value="0.0 0.0";
			
			if (Thread.CurrentThread.GetApartmentState()!=ApartmentState.STA)
				throw new ThreadStateException("Market#getMarketSummary should be run in a STA thread environment.");
		
			using (WebClient wc=new WebClient())
			using (WebBrowser wb=new WebBrowser(){DocumentText="",ScriptErrorsSuppressed=true}) {
				
				HtmlDocument doc=wb.Document.OpenNew(true);
				doc.Write(wc.DownloadString("https://www.google.com/search?&q="+market));
				
				if (!(doc.GetElementsByTagName("html")[0].OuterHtml.Contains("https://www.google.com/intl/en_ca/googlefinance/disclaimer")))
					throw new InvalidMarketException ("Invalid market: \""+market+'"');
				
				value=doc.GetElementsByTagName("div").Cast<HtmlElement>().Where(x=>x.GetAttribute("className")=="BNeawe iBp4i AP7Wnd").First().InnerText;
			
			}
			
			return new MarketSummary(Single.Parse(value.Split(' ')[1]),Single.Parse(value.Split(' ')[0]),market);
			
		}
		
	}
	
}
