namespace LinkCutter.Components.LogicAndModel;

public class IDGenerator
{

    public static string CreateShortLinkID()
    {
        do
        {
            DataBaseContext db = new DataBaseContext();

            List<string> candidates = Enumerable.Range(0, 10).Select(_ => GenerateRandomID()).ToList();

            string ID = db.AvailablePrimaryKey(candidates);

            if (!string.IsNullOrEmpty(ID))
                return ID;
        }
        while (true);
    }

    private static string GenerateRandomID(int length = 4)
    {
        string id = "";
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            id += random.Next(1, 3) switch
            {
                1 => Num(),
                2 => LowChar(),
                _ => UpperChar()
            };
        }
        return id;
    }

    private static string Num()
    {
        return new Random().Next(0, 9).ToString();
    }
    private static string LowChar()
    {
        return ((char)new Random().Next(97, 122)).ToString();
    }
    private static string UpperChar()
    {
        return ((char)new Random().Next(65, 90)).ToString();
    }
}
