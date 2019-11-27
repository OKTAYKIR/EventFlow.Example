namespace EventFlowExample
{
    class Program
    {
        static void Main(string[] args)
        {
            new PublishCommand().PublishCommandAsync().Wait();
        }
    }
}

