//#define RUN_ALL

var startTime = DateTime.UtcNow;

#if RUN_ALL
Advc2022.Problem01.Start();
Advc2022.Problem02.Start();
Advc2022.Problem03.Start();
Advc2022.Problem04.Start();
Advc2022.Problem05.Start();
Advc2022.Problem06.Start();
Advc2022.Problem07.Start();
Advc2022.Problem08.Start();
Advc2022.Problem09.Start();
Advc2022.Problem10.Start();
Advc2022.Problem11.Start();
Advc2022.Problem12.Start();
Advc2022.Problem13.Start();
Advc2022.Problem14.Start();
Advc2022.Problem15.Start();
Advc2022.Problem16.Start(); // Takes a few minutes
Advc2022.Problem17.Start();
Advc2022.Problem18.Start();
#endif

Advc2022.Problem19.Start(); // Take a couple of minutes

Console.WriteLine($"Elapsed : {(DateTime.UtcNow - startTime).TotalMilliseconds}");