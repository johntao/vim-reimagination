Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };
// Console.WriteLine($"{Console.WindowWidth},{Console.WindowHeight}");
// new Editor().Run();
var result = ChooseLayout.Run();
