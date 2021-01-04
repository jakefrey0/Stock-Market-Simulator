# Stock Trading Simulator
 Practice gambling.
 <br />  
StockMarketWrapper class library usage:

Namespace: StockMarketWrapper, Class: Market, File: Market.cs, Task: Get and display information on a market.
-------------------------------------------------------------------------------------------------------------
 
```
Console.Write("Market: ");
			
Thread thr=new Thread(()=> {
			
	MarketSummary sum=default(MarketSummary);
				
	try { sum=Market.getMarketSummary(Console.ReadLine()); } // Expected input: GOOGL, OSPTX, etc..
	catch (InvalidMarketException) {
					
		Console.WriteLine("Invalid market.");
		return;
					
	}
	catch (Exception) {
					
		Console.WriteLine("An unexpected exception occurred.");
		return;
					
	}
				
	Console.WriteLine(Environment.NewLine+
		"Price:      " +sum.value.ToString()+Environment.NewLine+
		"Difference: " +((sum.difference>0)?"+"+sum.difference.ToString()+" (↑)":sum.difference.ToString()+" (↓)")+Environment.NewLine);
				
});
			
thr.SetApartmentState(ApartmentState.STA);
thr.Start();
```
