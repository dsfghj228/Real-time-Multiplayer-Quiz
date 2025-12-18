namespace Back_Quiz.Dtos;

public class ApiResult
{
        public string Id { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }

        public List<string> Options { get; set; }

        public string Answer { get; set; }
        public int CorrectIndex { get; set; }

        public string Difficulty { get; set; }
        public string Type { get; set; }
}
