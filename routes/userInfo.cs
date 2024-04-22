using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UserInformationRouter
{
    private readonly ILogger _logger;
    private userInformationServices _userInfoService;

    public UserInformationRouter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UserInformationRouter>();
        _userInfoService = new userInformationServices(_logger);
    }


    [Function("GetAllUserProfiles")]
    public async Task<IActionResult> GetAllUserProfiles(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/profiles")] HttpRequestData req)
    {
        try
        {
            _logger.LogInformation("Processing a request to retrieve all user profiles");


            JArray profiles = await _userInfoService.GetAllUserProfiles();
            string profilesJson = JsonConvert.SerializeObject(profiles);

            // Construct the JSON object string with the "profiles" key
            string jsonObject = $"{{\"profiles\": {profilesJson} }}";

            return new ContentResult
            {
                Content = jsonObject,
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

    [Function("GetUserProgress")]
    public async Task<IActionResult> GetUserProgress(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/progress/{username}")] HttpRequestData req, string username)
    {
        try
        {
            _logger.LogInformation("Processing a request to retrieve user progress for username: " + username);

            JArray progress = await _userInfoService.GetUserProgressByUsername(username);
            string progressJson = JsonConvert.SerializeObject(progress);

            // Construct the JSON object string with the "progress" key
            string jsonObject = $"{{ \"progress\": {progressJson} }}";

            return new ContentResult
            {
                Content = jsonObject,
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

    [Function("GetTop5UsersByCorrectAnswers")]
    public async Task<IActionResult> GetTop5UsersByCorrectAnswers(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/top5")] HttpRequestData req)
    {
        try
        {
            _logger.LogInformation("Processing a request to retrieve top 5 users by correct answers");

            JArray top5Users = await _userInfoService.GetTop5UsersByCorrectAnswers();
            string top5UsersJson = JsonConvert.SerializeObject(top5Users);

            // Construct the JSON object string with the "top5Users" key
            string jsonObject = $"{{ \"top5Users\": {top5UsersJson} }}";

            return new ContentResult
            {
                Content = jsonObject,
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

}