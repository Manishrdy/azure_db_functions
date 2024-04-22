using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GameInformationServices
{

    private readonly ILogger _logger;
    private static DatabaseConnection dbConnection = DatabaseConnection.GetInstance();

    public GameInformationServices(ILogger logger)
    {
        _logger = logger;
        dbConnection = DatabaseConnection.GetInstance();
        // Open the database connection
        dbConnection.OpenConnection();
    }

    public async Task<string> InsertUserProfile(string device_id, string username)
    {
        // Create JObject for user profile object
        JObject userProfileObject = new JObject();
        userProfileObject["device_id"] = device_id;
        userProfileObject["username"] = username;

        // database insertion
        string insertQuery = "INSERT INTO userProfile (device_id, username) VALUES ('"
                         + device_id + "', '"
                         + username + "');";

        await dbConnection.ExecuteQueryAsync(insertQuery);

        // Create success response
        var response = new
        {
            device_id = userProfileObject["device_id"],
            username = userProfileObject["username"],
        };

        // Serialize the response object to JSON
        string jsonResponse = JsonConvert.SerializeObject(response);

        return jsonResponse;
    }

    public async Task<string> InsertUserProgress(string device_id, string username, int questions, int correct_answers, float accuracy, float rate)
    {
        // Create JObject for user progress object
        JObject userProgressObject = new JObject();
        userProgressObject["device_id"] = device_id;
        userProgressObject["username"] = username;
        userProgressObject["questions"] = questions;
        userProgressObject["correct_answers"] = correct_answers;
        userProgressObject["accuracy"] = accuracy;
        userProgressObject["rate"] = rate;

        // database insertion
        string insertQuery = "INSERT INTO userProgress (device_id, username, questions, correct_answers, accuracy, rate) VALUES ('"
                         + device_id + "', '"
                         + username + "', "
                         + questions + ", "
                         + correct_answers + ", "
                         + accuracy + ", "
                         + rate + ");";

        await dbConnection.ExecuteQueryAsync(insertQuery);

        // Create success response
        var response = new
        {
            device_id = userProgressObject["device_id"],
            username = userProgressObject["username"],
            questions = userProgressObject["questions"],
            correct_answers = userProgressObject["correct_answers"],
            accuracy = userProgressObject["accuracy"],
            rate = userProgressObject["rate"],
        };

        // Serialize the response object to JSON
        string jsonResponse = JsonConvert.SerializeObject(response);

        return jsonResponse;
    }

}
