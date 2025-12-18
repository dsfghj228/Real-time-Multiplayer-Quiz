namespace Back_Quiz.Dtos;

public class ApiResponse
{
    public string Status { get; set; }
    public int Results { get; set; }

    public List<ApiResult> Questions { get; set; }
}