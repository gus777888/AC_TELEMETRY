using System;
using System.Runtime.InteropServices;
using System.Threading; // For using Thread.Sleep()


//define the structure that maps the data from shared memory
[StructLayout(LayoutKind.Sequential)]
public struct ACData
{

    public float speed;
    public float rpm;
    public int gear;

}

class Program
{
    // Import necessary Windows API function for interacting with shared memory

    // Opens an existing named a shared memory object
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenFileMapping(int dwDesiredAccess, bool bInheritHandle, string lpName);

    // Maps a view of a file into the addressspace of the calling process
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, int DesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

    //unmaps a mapped view of a file
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    // the main method is the entry point of the program
    static void Main(String[] args)
    {
        const int FILE_MAP_READ = 0x0004; // Permission to read shared memory

        // Open shared memory
        IntPtr hMapFile = OpenFileMapping(FILE_MAP_READ, false, "Local\\acpmf_physics");
        if (hMapFile == IntPtr.Zero)
        {
            Console.WriteLine("Could not open shared memory.");
            return;
        }

        // Map the view of the file
        IntPtr pData = MapViewOfFile(hMapFile, FILE_MAP_READ, 0, 0, UIntPtr.Zero);
        if (pData == IntPtr.Zero)
        {
            Console.WriteLine("Could not map shared memory.");
            return;
        }
        // Continuously read and display data from shared memory
        Console.WriteLine("Press Ctrl+C to stop...");
        try
        {
            while (true) // Infinite loop to continuously read data
            {
                // Read data from shared memory
                ACData acData = Marshal.PtrToStructure<ACData>(pData);

                // Display data (Speed, RPM, Gear)
                Console.WriteLine($"Speed: {acData.speed} km/h, RPM: {acData.rpm}, Gear: {acData.gear}");

                // Wait for a short period before reading again to avoid flooding the console
                Thread.Sleep(500); // Sleep for 500 milliseconds (0.5 seconds)
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            // Unmap the view of the file when done
            UnmapViewOfFile(pData);
        }
    }
}
