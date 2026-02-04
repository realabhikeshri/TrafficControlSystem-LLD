using TrafficControlSystem_LLD.Enums;
using TrafficControlSystem_LLD.Services;

namespace TrafficControlSystem;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Traffic Control System (C# LLD) ===");

        ITrafficController controller = TrafficController.CreateDefault();

        controller.Start();

        // Let the controller run for some time in normal mode
        Thread.Sleep(TimeSpan.FromSeconds(20));

        Console.WriteLine("Switching to ALL_RED mode (emergency)...");
        controller.SwitchMode(IntersectionMode.AllRed);
        Thread.Sleep(TimeSpan.FromSeconds(10));

        Console.WriteLine("Switching back to NORMAL mode...");
        controller.SwitchMode(IntersectionMode.Normal);
        Thread.Sleep(TimeSpan.FromSeconds(15));

        controller.Stop();
        Console.WriteLine("Controller stopped. Demo finished.");
    }
}
