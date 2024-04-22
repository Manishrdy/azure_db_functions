using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GameInformationRouter
{
    private readonly ILogger _logger;
    private GameInformationServices _gameService;

    public GameInformationRouter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GameInformationRouter>();
        _gameService = new GameInformationServices(_logger);
    }

    [Function("createUserProfile")]
    public async Task<IActionResult> CreateNewGame(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "game/create")] HttpRequestData req)
    {
        try
        {
            _logger.LogInformation("Processing a request to create a new user profile");

            string? device_id = req.Query["device_id"];
            string? username = req.Query["username"];

            if (string.IsNullOrEmpty(device_id) || string.IsNullOrEmpty(username))
            {
                var errorResponse = new { message = "Error: device_id and username are required." };
                string errorJsonResponse = JsonConvert.SerializeObject(errorResponse);
                return new BadRequestObjectResult(errorJsonResponse);
            }

            string jsonResponse = await _gameService.InsertUserProfile(device_id, username);

            return new ContentResult
            {
                Content = jsonResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing the request");
            return new BadRequestObjectResult("Error: Invalid request.");
        }
    }

    [Function("InsertUserProgress")]
    public async Task<IActionResult> InsertUserProgress(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "game/progress")] HttpRequestData req)
    {
        try
        {
            string? device_id = req.Query["device_id"];
            string? username = req.Query["username"];
            int? questions = Int32.Parse(req.Query["questions"]);
            int? correct_answers = Int32.Parse(req.Query["correct_answers"]);
            float? accuracy = Single.Parse(req.Query["accuracy"]);
            float? rate = Single.Parse(req.Query["rate"]);

            if (string.IsNullOrEmpty(device_id) || string.IsNullOrEmpty(username) || questions == null || correct_answers == null || accuracy == null || rate == null)
            {
                var errorResponse = new { message = "Error: All fields are required." };
                string errorJsonResponse = JsonConvert.SerializeObject(errorResponse);
                return new BadRequestObjectResult(errorJsonResponse);
            }

            string jsonResponse = await _gameService.InsertUserProgress(device_id, username, questions.Value, correct_answers.Value, accuracy.Value, rate.Value);

            return new ContentResult
            {
                Content = jsonResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult("Error: Invalid request.");
        }
    }
}