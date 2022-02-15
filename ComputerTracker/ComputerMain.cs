using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerTracker
{
    class ComputerMain
    {
        public static void Main(string[] args)
        {
            int cloudStorage = 500;
            int networkSpeed = 10000;

            Computer defaultComputer = new Computer("000", 400000000, true, 300000000000, null);
            Computer userComputer = null;

            Console.Write("Please enter the maximum number of computers you will need to track: (between 5 and 20) ");
            int numComputers = Convert.ToInt32(Console.ReadLine());
            GetIntFromUser(ref numComputers, 5, 20, 10);
            Computer[] computers = new Computer[numComputers];
            DisplayOptions(computers, ref defaultComputer, ref userComputer, ref cloudStorage, ref networkSpeed);
            Console.WriteLine("Press any key to close");
            Console.ReadKey();

        }

        public static bool GetIntFromUser(ref int number, int min, int max, int defaultVal)
        {
            if (number < min || number > max)
            {
                Console.WriteLine($"Sorry, the number specified is out of range. Default value given: {defaultVal}");
                number = defaultVal;
                return false;
            }
            return true;
        }

        public static bool DoubleIntNotPastMax(ref int number, int max, bool setToMax)
        {
            int testValue = 2 * number;
            if (testValue > max)
            {
                number = setToMax ? max : number;
                return false;
            }
            number = testValue;
            return true;
        }

        public static bool HalveValueNotPastMin(ref int number, int min, bool setToMin)
        {
            int testValue = number / 2;
            if (testValue < min)
            {
                number = setToMin ? min : number;
                return false;
            }
            number = testValue;
            return true;
        }

        public static void DisplayOptions(Computer[] computers, ref Computer prototype, ref Computer userComputer, ref int cloudStorage, ref int networkSpeed)
        {
            int choice = 0;
            do
            {
                Console.WriteLine("Please choose one of the following options:");
                Console.WriteLine("1. Add a new computer \n2. Specify the prototype" +
                    "\n3. Remove prototype \n4. Upgrade cloud storage \n5. Downgrade cloud storage" +
                    "\n6. Upgrade network speed \n7. Downgrade network speed \n8. Get a summary of" +
                    " a specific computer \n9. Get stats for all computers \n10. Get stats for specific" +
                    " computers \n11. Exit");
                choice = Convert.ToInt32(Console.ReadLine());
                MenuChoice(choice, computers, ref prototype, ref userComputer, ref cloudStorage, ref networkSpeed);
            } while (choice < 11);
        }

        public static void MenuChoice(int choice, Computer[] computers, ref Computer prototype, ref Computer userPrototype, ref int cloudStorage, ref int networkSpeed)
        {
            // choice = Convert.ToInt32(Console.ReadLine());
            while (choice < 1 || choice > 11)
            {
                Console.WriteLine("Invalid entry. Please enter a number between 1 and 11.");
                choice = Convert.ToInt32(Console.ReadLine());
            }
            switch (choice)
            {
                case 1:
                    AddComputer(computers);
                    break;
                case 2:
                    CreatePrototype(ref userPrototype);
                    break;
                case 3:
                    RemovePrototype(ref userPrototype);
                    break;
                case 4:
                    UpgradeStorage(ref cloudStorage);
                    break;
                case 5:
                    DowngradeStorage(ref cloudStorage);
                    break;
                case 6:
                    UpgradeNetworkSpeed(ref networkSpeed);
                    break;
                case 7:
                    DowngradeNetworkSpeed(ref networkSpeed);
                    break;
                case 8:
                    GetComputerSummary(computers, prototype, cloudStorage, networkSpeed);
                    break;
                case 9:
                    GetSummaryStats(computers, ref cloudStorage, ref networkSpeed);
                    break;
                case 10:
                    GetStatsSpecificLocations(computers, userPrototype, prototype, ref cloudStorage, ref networkSpeed);
                    break;
                case 11:
                    break;
            }

        }

        public static Computer buildComputer()
        {
            Console.WriteLine("Please enter the information about your computer in the " +
                "following order, separating each piece of information with commas: ");
            Console.WriteLine("Computer ID, RAM amount, Has antenna (optional), " +
                "Storage capacity (optional), list of licenses separated by commas (optional):");
            string info = Console.ReadLine();
            string[] values = info?.Split(',');
            string id = values[0];
            int ram = Convert.ToInt32(values[1]);

            //hasAntenna is optional
            bool userInput;
            bool? hasAntenna = null;
            if (bool.TryParse(values[2], out userInput))
            {
                hasAntenna = userInput;
            }
            //storage capacity is optional
            double storageInput = 0;
            double? storage = null;
            if (double.TryParse(values[3], out storageInput))
            {
                storage = storageInput;
            }
            //software licenses is optional
            int licenseInput = 0;
            int nullLicenses = 0;

            //user must put in commas even if not setting all licenses
            int?[] licenses = new int?[5];
            for (int i = 0; i < licenses.Length; i++)
            {
                if (int.TryParse(values[i + 4], out licenseInput))
                {
                    licenses[i] = licenseInput;
                    if (licenses[i] == null)
                    {
                        nullLicenses++;
                    }
                }
            }

            if (nullLicenses == licenses.Length)
            {
                licenses = null;
            }

            return new Computer(id, ram, hasAntenna, storage, licenses);
        }

        public static void AddComputer(Computer[] computers)
        {
            for (int i = 0; i < computers.Length; i++)
            {
                if (computers[i] == null)
                {
                    computers[i] = buildComputer();
                    break;
                }
                else if (i == (computers.Length - 1))
                {
                    Console.WriteLine("Maximum computers reached!");
                }
            }
        }

        //Specify their "Prototype" computer.
        public static void CreatePrototype(ref Computer userPrototype)
        {
            userPrototype = buildComputer();
        }

        public static void RemovePrototype(ref Computer userPrototype)
        {
            userPrototype = new Computer(null, 0, null, null, null);
        }

        // Upgrade their cloud storage up to 16000. If exceeds, do not upgrade.
        public static void UpgradeStorage(ref int cloudStorage)
        {
            int max = 16000;
            if (!DoubleIntNotPastMax(ref cloudStorage, max, false))
            {
                Console.WriteLine("Maximum storage exceeded. Storage not upgraded.");
            }
            else
            {
                Console.WriteLine($"Cloud storage upgraded to {cloudStorage}.");
            }
        }

        // Downgrade their cloud storage, with 500 minimum
        public static void DowngradeStorage(ref int cloudStorage)
        {
            int min = 500;
            if (!HalveValueNotPastMin(ref cloudStorage, min, true))
            {
                Console.WriteLine($"Minimum storage reached. Storage downgraded to {min}.");
            }
            else
            {
                Console.WriteLine($"Cloud storage downgraded to {cloudStorage}.");
            }
        }

        // Upgrade the network speed up to 250000. If exceeds, set to 250000.
        public static void UpgradeNetworkSpeed(ref int networkSpeed)
        {
            int max = 250000;
            if (!DoubleIntNotPastMax(ref networkSpeed, max, true))
            {
                Console.WriteLine($"Maximum speed reached. Speed upgraded to {max}.");
            }
            else
            {
                Console.WriteLine($"Network speed upgraded to {networkSpeed}.");
            }
        }

        // Downgrade the network speed until 10000, otherwise do not downgrade
        public static void DowngradeNetworkSpeed(ref int networkSpeed)
        {
            int min = 10000;
            if (!HalveValueNotPastMin(ref networkSpeed, min, false))
            {
                Console.WriteLine("Minimum network speed reached. Network speed not downgraded.");
            }
            else
            {
                Console.WriteLine($"Network speed downgraded to {networkSpeed}.");
            }

        }

        // Ask for a summary of a specific computer by array index.If null, use your prototype(not user's)
        public static void GetComputerSummary(Computer[] computers, Computer prototype, int cloudStorage, int networkSpeed)
        {
            Console.WriteLine("Please enter the index of the computer you wish to see: ");
            int index = Convert.ToInt32(Console.ReadLine());
            string summary = computers?[index]?.ToString() ?? prototype.ToString();
            Console.WriteLine(summary + $"Cloud Storage: {cloudStorage}\nNetwork Speed: {networkSpeed}");
        }

        /*
         * Ask for a summary of statistics on all computers entered in the array so far. 
         * Includes: avg RAM, percent with antennae, avg hard drive capacity, avg total licenses,
         * avg license per software program, cloud storage, network speed
         */
        public static void GetSummaryStats(Computer[] computers, ref int cloudStorage, ref int networkSpeed)
        {
            double avgRAM = GetAverageRAM(computers);
            double percentAntenna = GetPercentAntenna(computers);
            double avgHardDriveCap = GetAvgHardDriveCap(computers);
            double avgTotalLicenses = GetAvgTotalLicenses(computers);
            string avgLicensesSpecificProgram = GetAvgLicensesSpecificProgram(computers);
            StringBuilder info = new StringBuilder();
            info.Append($"Average RAM: {avgRAM}\n");
            info.Append($"Percent computers with antennae: {percentAntenna}\n");
            info.Append($"Average hard drive capacity: {avgHardDriveCap}\n");
            info.Append($"Average licenses across all software: {avgTotalLicenses}\n");
            info.Append($"Average licenses for each program: {avgLicensesSpecificProgram}\n");
            info.Append($"Cloud storage: {cloudStorage}\n");
            info.Append($"Network speed: {networkSpeed}\n");
            Console.WriteLine(info);
        }

        public static string GetAvgLicensesSpecificProgram(Computer[] computers)
        {
            //There are 5 available software slots
            double[] avgPerProgram = new double[5];
            foreach (Computer computer in computers)
            {
                if (!(computer?.licenses == null))
                {
                    for (int i = 0; i < computer.licenses.Length; i++)
                    {
                        avgPerProgram[i] += computer?.licenses?[i] ?? 0;
                    }
                }
            }

            for (int i = 0; i < avgPerProgram.Length; i++)
            {
                avgPerProgram[i] = avgPerProgram[i] / computers.Length;
            }
            string licenseString = "";
            for (int i = 0; i < avgPerProgram.Length; i++)
            {
                licenseString += (!(avgPerProgram[i] + " ").Equals(" ")) ? avgPerProgram[i] + " " : "0 ";
            }
            return (licenseString == "") ? "N/A" : licenseString;
        }
        public static double GetAvgTotalLicenses(Computer[] computers)
        {
            int sumLicenses = 0;
            foreach (Computer computer in computers)
            {
                sumLicenses += computer?.GetNumLicenses() == null ? 0 : computer.GetNumLicenses();
            }
            return (double)sumLicenses / computers.Length;
        }

        public static double GetAverageRAM(Computer[] computers)
        {
            double sumRAM = 0;
            foreach (Computer computer in computers)
            {
                sumRAM += computer == null ? 0 : computer.Ram;
            }
            return (double)sumRAM / computers.Length;
        }

        public static double GetPercentAntenna(Computer[] computers)
        {
            int numAntennas = 0;
            int numNulls = 0;
            foreach (Computer computer in computers)
            {
                if (computer?.HasAntenna == null)
                {
                    numNulls++;
                }
                if (computer?.HasAntenna ?? false)
                {
                    numAntennas++;
                }
            }
            int nonnullAntenna = computers.Length - numNulls;
            if (nonnullAntenna > 0)
            {
                return (double)numAntennas / nonnullAntenna;
            }
            else
            {
                return 0;
            }
        }

        public static double GetAvgHardDriveCap(Computer[] computers)
        {
            double sumCapacity = 0;
            int nullCapacity = 0;
            foreach (Computer computer in computers)
            {
                if (computer?.StorageCap == null)
                {
                    nullCapacity++;
                }
                else
                {
                    sumCapacity += computer.StorageCap ?? 0;
                }
            }
            int nonnull = computers.Length - nullCapacity;
            return (nonnull == 0) ? 0 : (double)sumCapacity / nonnull;
        }


        // Ask for the same summary, but specify the exact locations in the array that should
        // be included.
        public static void GetStatsSpecificLocations(Computer[] computers, Computer userPrototype, Computer defaultComputer, ref int cloudStorage, ref int networkSpeed)
        {
            Console.WriteLine("Please enter the indices of computers for which you want the stats: ");
            string[] indicesString = Console.ReadLine().Split(',');
            Computer[] userList = new Computer[indicesString.Length];
            for (int i = 0; i < userList.Length; i++)
            {
                userList[i] = computers[Convert.ToInt32(indicesString[i])] ?? userPrototype ?? defaultComputer;
            }
            GetSummaryStats(userList, ref cloudStorage, ref networkSpeed);
        }
    }
}