using VimRenaissance;

Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };
var result = ChooseLayout.Run();
var layout = MappingCommands.Run(result);
new Editor().Run(layout);
Console.Clear();