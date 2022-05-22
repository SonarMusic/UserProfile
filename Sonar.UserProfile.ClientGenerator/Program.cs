using NSwag;
using NSwag.CodeGeneration.CSharp;


Console.WriteLine("Enter path to where you want to save generated file");
// "E:\ITMO prog\Prog\C#\Sonar.UserProfile\Sonar.UserProfile.Client\UserClient.cs"
string filePath = Console.ReadLine();

System.Net.WebClient wclient = new System.Net.WebClient();

var document =
    await OpenApiDocument.FromJsonAsync(wclient.DownloadString("https://localhost:7062/swagger/v1/swagger.json"));

wclient.Dispose();

var settings = new CSharpClientGeneratorSettings
{
    ClassName = "UserClient",
    CSharpGeneratorSettings =
    {
        Namespace = "Sonar.UserProfile.UserClient"
    }
};


var generator = new CSharpClientGenerator(document, settings);
var code = generator.GenerateFile();

File.WriteAllText(filePath, code);