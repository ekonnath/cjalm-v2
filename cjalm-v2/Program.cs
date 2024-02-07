using cjalm_v2.data;
using cjalm_v2.domain;

class Program
{
    static void Main(string[] args)
    {
        bool programActive = true;
        Console.WriteLine("Welcome to the Corporate Jargon and Language Manager (C-JALM)\n");
        printLogo(250);
       
        Console.WriteLine("Establishing connection to the database...");
        Thread.Sleep(500);
        using (CJALMContext context = new CJALMContext())
        {
            context.Database.EnsureCreated();
        }
      
        while (programActive)
        {
            switch (handleMenu())
            {
                case 1:
                    addOrUpdateHandler();
                    programActive = returnToMenu() ? true : false;
                    break;
                case 2:
                    searchHandler();
                    programActive = returnToMenu() ? true : false;
                    break;
                case 3:
                    viewAllHandler();
                    programActive = returnToMenu() ? true : false;
                    break;
                case 4:
                    removeHandler();
                    programActive = returnToMenu() ? true : false;
                    break;
                case 5:
                    programActive = false;
                    break;
            }
        }

        Console.WriteLine("\n\nDeactivating session for C-JALM...");
        Thread.Sleep(2000);
        Console.WriteLine("Goodbye.");
        Thread.Sleep(500);
    }

    public static void addOrUpdateHandler()
    {
        Console.Write("\n\nPlease enter a word: ");
        try
        {
            string userInput = Console.ReadLine()!.Trim();
            string upperInput = char.ToUpper(userInput[0]) + userInput.Substring(1);

            var searchEntry = searchSpecificEntry(upperInput);
            if (searchEntry != null)
            {
                incrementUseCount(upperInput);
                Console.WriteLine($"Entry found. You've heard this term {searchEntry.UseCount + 1} time(s).");
            }
            else
            {
                Console.Write("New entry detected. Please enter a definition: ");
                try
                {
                    string inputDefinition = Console.ReadLine()!.Trim();
                    addEntry(upperInput, inputDefinition);
                    Console.WriteLine("A new entry has successfully been created.");
                }
                catch
                {
                    Console.WriteLine("You can't just say nothing and expect us to help. Restarting...");
                    return;
                }
            }
        }
        catch
        {
            Console.WriteLine("You can't just say nothing and expect us to help. Restarting...");
            return;
        }
    }

    public static void searchHandler()
    {
        Console.Write("\n\nPlease enter the word or definition you are looking for: ");
        try
        {
            string userInput = Console.ReadLine()!.Trim();
            string upperInput = char.ToUpper(userInput[0]) + userInput.Substring(1);
            
            List<Entry> searchResults = searchEntries(upperInput);
            Console.WriteLine($"The search contains {searchResults.Count} results.\n");

            foreach (var result in searchResults)
            {
                Console.WriteLine(result.EntryId + " | " + result.Word + " | " + result.Definition + " | " + result.UseCount + " use(s)");
            }
        }
        catch
        {
            Console.WriteLine("You can't just say nothing and expect us to help. Taking you back to the menu...");
            Thread.Sleep(1000);
            return;
        }
    }

    public static void viewAllHandler()
    {
        
        List<Entry> entries = viewAllEntries();

        if (entries.Count > 0)
        {
            Console.WriteLine("\n\nThe following entries were found in your C-JALM-Database:\n");
            foreach (var entry in entries)
            {
                Console.WriteLine(entry.EntryId + " | " + entry.Word + " | " + entry.Definition + " | " + entry.UseCount + " use(s)");
            }
        } else
        {
            Console.WriteLine("\nNothing to view. Add entries to track corporate jargon.");
        }
    }

    public static void removeHandler()
    {
        Console.Write("\n\nPlease enter the word you would like to remove from CJALM-v2.Database: ");
        try
        {
            string userInput = Console.ReadLine()!.Trim();
            string upperInput = char.ToUpper(userInput[0]) + userInput.Substring(1);

            var searchEntry = searchSpecificEntry(upperInput);
            if (searchEntry != null)
            {
                Console.WriteLine($"We found the following entry:");
                Console.WriteLine("\n" + searchEntry.EntryId + " | " + searchEntry.Word + " | " + searchEntry.Definition + " | " + searchEntry.UseCount + " use(s)");
                Console.Write("\n\nAre you sure you would like to remove this entry from CJALM-v2.Database? (y/n) ");
                if (getYorN())
                {
                    removeEntry(upperInput);
                    Console.WriteLine("\nThe entry has been removed.");
                } else
                {
                    Console.WriteLine("\nRemove action aborted.");
                }
            }
            else
            {
                Console.WriteLine("Sorry, we couldn't find that word.");
            }
        }
        catch
        {
            Console.WriteLine("You can't just say nothing and expect us to help. Returning to the menu...");
            return;
        }
    }


    public static void addEntry(string word, string definition)
    {
        using var context = new CJALMContext();

        var entry = new Entry { Word = word, Definition = definition, UseCount = 1 };

        context.Entries.Add(entry);
        context.SaveChanges();
    }

    public static List<Entry> searchEntries(string word)
    {
        using var context = new CJALMContext();
        
        var searchResults = context.Entries.Where(w => w.Word.Contains(word) || w.Definition.Contains(word)).ToList();

        return searchResults;
    }

    public static Entry searchSpecificEntry(string word)
    {
        using var context = new CJALMContext();

        var result = context.Entries.FirstOrDefault(w => w.Word.Contains(word));

        return result;
    }

    public static List<Entry> viewAllEntries()
    {
        using var context = new CJALMContext();

        var entries = context.Entries.ToList();

        return entries;
    }

    public static void removeEntry(string word)
    {
        using var context = new CJALMContext();
        var entryToRemove = context.Entries.FirstOrDefault(e => e.Word == word);

        if (entryToRemove != null)
        {
            context.Entries.Remove(entryToRemove);
            context.SaveChanges();
        }
    }

    public static void incrementUseCount(string word)
    {
        using var context = new CJALMContext();
        var entryToIncrement = context.Entries.FirstOrDefault(e => e.Word == word);
        
        if (entryToIncrement != null)
        {
            entryToIncrement.UseCount += 1;
            context.SaveChanges();
        }
    }

    private static int handleMenu()
    {
        string[] menuItems = ["New entry or updated entry", "Search for entry", "View all entries", "Remove entry", "Exit C-JALM"];

        Console.WriteLine("\nPlease select a menu option: ");
        Thread.Sleep(500);
        bool optionSelected = false;
        int option = 0;

        while (!optionSelected)
        {
            generateMenu(6, menuItems);
            Console.Write("\nEnter selection: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            option = Convert.ToInt32(keyInfo.Key);

            Thread.Sleep(500);
            if ((option > 48 && option < 58) && (option - 48 <= menuItems.Length))
            {
                optionSelected = true;
            }
            else
            {
                Console.WriteLine("\n\nPlease enter a valid menu option.");
            }
        }

        return option - 48;
    }

    private static void generateMenu(int minSpacing, string[] menuItems)
    {
        int maxLength = 0;
        foreach (string menuItem in menuItems)
        {
            int currentLength = menuItem.Length;
            if (currentLength > maxLength)
            {
                maxLength = currentLength;
            }
        }
        int totalSpacing = maxLength + minSpacing;

        // foreach is readonly
        for (int x = 0; x < menuItems.Length; x++)
        {
            int addedSpace = totalSpacing - menuItems[x].Length;

            for (int y = 0; y < addedSpace; y++)
            {
                menuItems[x] += ".";
            }
            Console.WriteLine(menuItems[x] + (x + 1));
            Thread.Sleep(250);
        }

        return;
    }

    private static bool getYorN()
    {
        bool optionSelected = false;

        while (!optionSelected)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int option = Convert.ToInt32(keyInfo.Key);

            Thread.Sleep(500);
            Console.WriteLine();

            if (option == 121 || option == 89)
            {
                optionSelected = true;
                return true;
            }
            else if (option == 110 || option == 78)
            {
                optionSelected = true;
                return false;
            }
            else
            {
                Console.Write("\nPlease enter a valid option (y/n): ");
            }
        }
        return false;
    }

    private static bool returnToMenu()
    {
        Console.Write("\nWould you like to make any additional transcations? (y/n): ");
        return getYorN();
    }

    private static void printLogo(int sleepTime)
    {
        Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░░█████╗░░░░░░░░░░░░██╗░█████╗░██╗░░░░░███╗░░░███╗░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░██╔══██╗░░░░░░░░░░░██║██╔══██╗██║░░░░░████╗░████║░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░██║░░╚═╝█████╗░░░░░██║███████║██║░░░░░██╔████╔██║░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░██║░░██╗╚════╝██╗░░██║██╔══██║██║░░░░░██║╚██╔╝██║░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░╚█████╔╝░░░░░░╚█████╔╝██║░░██║███████╗██║░╚═╝░██║░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░░╚════╝░░░░░░░░╚════╝░╚═╝░░╚═╝╚══════╝╚═╝░░░░░╚═╝░░");
        Thread.Sleep(sleepTime);
        Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░\n");
    }
}