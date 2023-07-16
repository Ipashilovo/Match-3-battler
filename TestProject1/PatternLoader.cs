using Newtonsoft.Json;
using TestProject1.GridPatterns;

namespace TestProject1
{
    public static class PatternLoader
    {
        public static GridPattern[] LoadPattern(string name)
        {
            string directiry = Environment.CurrentDirectory;
            var split = directiry.Split(@"\");
            string lastName = split[^1];
            while (lastName != "TestProject1")
            {
                directiry = Directory.GetParent(directiry).FullName;
                split = directiry.Split(@"\");
                lastName = split[^1];
            }
            var path = Path.Combine(directiry,$"GridPatterns/{name}.json");
            var text = File.ReadAllText(path);
            
            return JsonConvert.DeserializeObject<GridPattern[]>(text);
        }

        public static void Save(string name, string item)
        {
            string directiry = Environment.CurrentDirectory;
            var split = directiry.Split(@"\");
            string lastName = split[^1];
            while (lastName != "TestProject1")
            {
                directiry = Directory.GetParent(directiry).FullName;
                split = directiry.Split(@"\");
                lastName = split[^1];
            }
            var path = Path.Combine(directiry,$"CustomTestResults/{name}");
            File.WriteAllText(path, item);
        }


    }
}