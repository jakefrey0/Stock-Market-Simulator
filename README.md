# Stock Trading Simulator
 Practice gambling.
 <br />  
# StockMarketWrapper class library usage:

Task: Get and display information on a market.
-------------------------------------------------------------------------------------------------------------
 
```C#

using StockMarketWrapper; // Import the necessary namespace

Console.Write("Market: "); // UI for user
			
Thread thr=new Thread(()=> { // Create new thread so we can give it the required attributes, instead of sacrificing the current thread
			
	MarketSummary sum=default(MarketSummary); //Give it default value since it will never be used as is.
				
	try { sum=Market.getMarketSummary(Console.ReadLine()); } // Expected input: GOOGL, OSPTX, etc.. 
	catch (InvalidMarketException) { // If the market is invalid
					
		Console.WriteLine("Invalid market."); // Tell the user it's invalid
		return; // Don't continue
					
	}
	catch (Exception) { // Unexpected error ? Probably error with user's net. You can double check for exceptions, if your application requires such features.
					
		Console.WriteLine("An unexpected exception occurred.");  // Tell the user
		return; // Don't continue
					
	}
				
        // Write all the information we collected:
	Console.WriteLine(Environment.NewLine+
		"Price:      " +sum.value.ToString()+Environment.NewLine+
		"Difference: " +((sum.difference>0)?"+"+sum.difference.ToString()+" (↑)":sum.difference.ToString()+" (↓)")+Environment.NewLine);
				
});
			
thr.SetApartmentState(ApartmentState.STA); // Market#getMarketSummary is expected to be run in a STA thread, otherwise you will get a ThreadStateException
thr.Start(); // Start the thread.
```
