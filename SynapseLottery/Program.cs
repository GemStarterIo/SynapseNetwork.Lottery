class App
{
    /// <summary>
    /// Console application created by Synapse.Network to make possible for every user to check pool lottery results.
    /// If you are running this app from Visual Studio or similar tool, please note that in Properties/launchSettings.json file we put example arguments for application. Of course, you are free to change it.
    /// However, we strongly recommend to run our example first to be sure, that everything is working properly and your results list is the same as in ExpectedExampleUsersOutput.txt file.
    /// </summary>
    /// <param name="args">You need to pass 3 required and 1 optional arguments:
    ///     - chance to win expressed in percents: for example 30
    ///     - seed for random number generator, it should be the last before registration ends block number on Ethereum Network. In our example, you should pass 12345546</param>
    ///     - path to file with user list - see our ExampleUsers.txt file to see expected format: every user in new line, no separators
    ///     - optional path to destination file, where result list will be saved. Application need to have permissions to write in selected path. File doesn't need to exists before app run.
    ///     Our example arguments are: 30 12345546 ExampleUsers.txt UsersOutput.txt
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    static async Task Main(string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException("Missing arguments. Chance percent, seed, users source file path are required. The last one, destination file, is optional.");

        var chancePercent = int.Parse(args[0]);
        var seed = int.Parse(args[1]);

        var users = await File.ReadAllLinesAsync(args[2]);
        var usersHashSet = new HashSet<string>(users);
        var pickCount = (int)Math.Floor(users.Length * (chancePercent / 100.0));

        var results = PickRandomWallets(usersHashSet, pickCount, seed);

        if (args.Length > 3 )
            await File.WriteAllLinesAsync(args[3], results);
        
        foreach (var user in results)
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
        Console.Write("Press any key to close application...");
        Console.ReadKey();
    }

    public static HashSet<string> PickRandomWallets(HashSet<string> userTickets, int pickCount, int seed)
    {
        var sorted = userTickets.OrderBy(ut => ut);
        var random = new Random(seed);
        var shuffled = sorted.OrderBy(x => random.Next()).ToArray();
        return new HashSet<string>(shuffled.Take(pickCount).OrderBy(u => u));
    }
}