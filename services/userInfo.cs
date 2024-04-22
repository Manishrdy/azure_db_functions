using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class userInformationServices
{

    private readonly ILogger _logger;
    private static DatabaseConnection dbConnection = DatabaseConnection.GetInstance();

    public userInformationServices(ILogger logger)
    {
        _logger = logger;
        dbConnection = DatabaseConnection.GetInstance();
        // Open the database connection
        dbConnection.OpenConnection();
    }


    public async Task<JArray> GetAllUserProfiles()
    {
        string query = "SELECT * FROM userProfile";
        List<string> userDetails = await dbConnection.ExecuteQueryAsync(query);

        if (userDetails.Count > 0)
        {
            var userList = new JArray();
            foreach (var user in userDetails)
            {
                var userObject = JObject.Parse(user);
                userList.Add(userObject);
            }
            return userList;
        }

        return null;
    }

    public async Task<JArray> GetUserProgressByUsername(string username)
    {
        string query = "SELECT * FROM userProgress WHERE username = '" + username + "'";
        List<string> userProgress = await dbConnection.ExecuteQueryAsync(query);

        if (userProgress.Count > 0)
        {
            var progressList = new JArray();
            foreach (var progress in userProgress)
            {
                var progressObject = JObject.Parse(progress);
                progressList.Add(progressObject);
            }
            return progressList;
        }

        return null;
    }


    public async Task<JArray> GetTop5UsersByCorrectAnswers()
    {
        string query = @"
        SELECT TOP 5
            username,
            SUM(correct_answers) AS TotalCorrectAnswers,
            COUNT(*) AS TotalQuestions
        FROM
            userProgress
        GROUP BY
            device_id,
            username
        ORDER BY
            TotalCorrectAnswers DESC,
            TotalQuestions DESC";
        List<string> topUsers = await dbConnection.ExecuteQueryAsync(query);

        if (topUsers.Count > 0)
        {
            var userList = new JArray();
            foreach (var user in topUsers)
            {
                var userObject = JObject.Parse(user);
                userList.Add(userObject);
            }
            return userList;
        }

        return null;
    }


}
