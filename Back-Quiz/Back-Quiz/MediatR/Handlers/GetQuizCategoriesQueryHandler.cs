using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Queries;
using MediatR;

namespace Back_Quiz.MediatR.Handlers;

public class GetQuizCategoriesQueryHandler : IRequestHandler<GetQuizCategoriesQuery, List<string>>
{
    private readonly IQuizService _quizService;

    public GetQuizCategoriesQueryHandler(IQuizService quizService)
    {
        _quizService = quizService;
    }
    
    public async Task<List<string>> Handle(GetQuizCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _quizService.GetCategoriesAsync();
    }
}