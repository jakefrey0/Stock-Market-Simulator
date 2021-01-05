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

Task: Actively watch and update markets then display results.
-------------------------------------------------------------------------------------------------------------
```C#
using StockMarketWrapper; //Import the necessary namespace

Console.WriteLine("MarketWatcher Test");
Console.Title="MarketWatcher Test Application";
			
MarketWatcher mw=new MarketWatcher(new []{"GOOGL","AAPL"},30000); //Initialize the MarketWatcher for two different markets, and give it a 30 second (30000 millisecond) refresh rate.
			
mw.onMarketUpdate+=gotMarketUpdate; //Tell the MarketWatcher to call our function and to bring the desired data whenever it refreshes the markets.
			
mw.startWatching(); //Start watching the market
			
Console.ReadKey(); //Wait for user input to test some other 'features'
			
mw.setDelay(1); //Set delay to 1 so it will update as quick as possible now
mw.addMarket("OSPTX"); //Add a new market
// (All of these things update in real time as it is still watching)
			
Console.ReadKey(); //Wait for user input..
mw.stopWatching(); //Stop watching (after the next update) and conseqeuently exit the application.

// The aforementioned gotMarketUpdate function, where we get the market summary:
public static void gotMarketUpdate (MarketWatcher sender,MarketUpdateEventArgs args) {
			
	Console.WriteLine(Environment.NewLine+"Got update at "+args.time.ToString()); //Print the time of this update
	foreach (MarketSummary sum in args.info) //Loop through every market that was checked
		Console.WriteLine(sum.market+": Price: "+sum.value+" ("+((sum.difference>0)?"↑)":"↓)")); //Display the data
						
}
```
