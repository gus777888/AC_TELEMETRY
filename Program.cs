using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    // Import necessary Windows API functions
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    // Define constants for memory mapping
    const uint FILE_MAP_READ = 0x0004;
    const string SHARED_MEMORY_NAME = "Local\\acpmf_physics"; // Ensure this matches the correct shared memory name

    // Define the ACData structure to match the layout from Assetto Corsa
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SPageFilePhysics
    {
        public int packetId;                // Packet ID
        public float gas;                   // Gas pedal position
        public float brake;                 // Brake pedal position
        public float fuel;                  // Fuel level
        public int gear;                    // Current gear
        public int rpms;                    // RPM
        public float steerAngle;            // Steering angle
        public float speedKmh;              // Speed in km/h
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] velocity;            // Velocity (x, y, z)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] accG;                // Acceleration (x, y, z)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelSlip;           // Wheel slip for each wheel
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelLoad;           // Wheel load for each wheel
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelsPressure;      // Tire pressure for each wheel
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelAngularSpeed;   // Wheel angular speed for each wheel
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreWear;            // Tire wear for each tire
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreDirtyLevel;      // Tire dirt level for each tire
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreCoreTemperature; // Tire core temperature for each tire
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] camberRAD;           // Camber angles for each tire
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionTravel;     // Suspension travel for each tire
        public float drs;                   // DRS status
        public float tc;                    // Traction control
        public float heading;               // Car heading
        public float pitch;                 // Car pitch
        public float roll;                  // Car roll
        public float cgHeight;              // Center of gravity height
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public float[] carDamage;           // Car damage values
        public int numberOfTyresOut;       // Number of tires out
        public int pitLimiterOn;            // Pit limiter status
        public float abs;                   // ABS status
        public float kersCharge;            // KERS charge
        public float kersInput;             // KERS input
        public int autoShifterOn;           // Auto shifter status
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] rideHeight;          // Ride height
        public float turboBoost;            // Turbo boost
        public float ballast;                // Ballast
        public float airDensity;            // Air density
    }

    static void Main(string[] args)
    {
        // Open the shared memory
        IntPtr hMapFile = OpenFileMapping(FILE_MAP_READ, false, SHARED_MEMORY_NAME);
        if (hMapFile == IntPtr.Zero)
        {
            Console.WriteLine("Could not open file mapping.");
            return;
        }

        // Size of the data structure
        int dataSize = Marshal.SizeOf(typeof(SPageFilePhysics));

        while (true) // Continuous loop to read data
        {
            // Map the view of the file
            IntPtr pData = MapViewOfFile(hMapFile, FILE_MAP_READ, 0, 0, new UIntPtr((uint)dataSize));
            if (pData != IntPtr.Zero)
            {
                // Read the data into the SPageFilePhysics structure
                SPageFilePhysics acData = Marshal.PtrToStructure<SPageFilePhysics>(pData);

                // Display the relevant telemetry data
                Console.WriteLine($"Speed: {acData.speedKmh} km/h, RPM: {acData.rpms}, Gear: {acData.gear}");

                // Unmap the view of the file when done
                UnmapViewOfFile(pData);
            }
            else
            {
                Console.WriteLine("Could not map shared memory.");
            }

            // Wait for a short interval before the next read
            Thread.Sleep(100); // Adjust the interval as needed (e.g., 100 ms)
        }

        // Clean up handles (not reachable in this infinite loop)
        CloseHandle(hMapFile);
    }
}
