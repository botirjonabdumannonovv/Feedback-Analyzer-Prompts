// Correct namespaces and using directives
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// Create configurations
var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddUserSecrets<Program>(); // Ensure 'Program' is the correct class representing your startup configuration
var configuration = configurationBuilder.Build();

// Create kernel builder
var kernelBuilder = Kernel.CreateBuilder(); // Ensure 'Kernel' is the correct class and namespace

// Add OpenAI connector
kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);

// Build kernel
var kernel = kernelBuilder.Build();

// Sample product description
const string productDescription = "The Tesla Model Y: A symbol of innovation and sustainability in the automotive industry.";

// User feedback input
Console.Write("Enter your feedback about the product: ");
var userFeedback = Console.ReadLine() ?? ""; // Handle null case for ReadLine

// Define the extraction prompt
var extractionPrompt = $@"
    ## Opinion Mining

    Product Description:
    {productDescription}

    User Feedback: {userFeedback}

    Extract positive, negative, and neutral opinion points from the user feedback.

    Opinion Points:
";

// Invoke the Semantic Kernel for feedback extraction
var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

// Create and fill the UnifiedFeedback object
var unifiedFeedback = new OpinionMining { OpinionPoints = new List<(string, OpinionLabel)>() };

// Parse extractionResponse to extract positive, negative, and neutral opinion points and add them to the collection
if (!string.IsNullOrEmpty(extractionResponse))
{
    // Split the extractionResponse into individual opinion points
    var opinionPoints = extractionResponse.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
    
    foreach (var point in opinionPoints)
    {
        // Remove the indicators from the opinion point and identify label
        var cleanPoint = point.Replace("Positive: ", "").Replace("Negative: ", "").Replace("Neutral: ", "").Trim();
        var label = point.Contains("Positive:") ? OpinionLabel.Positive :
            point.Contains("Negative:") ? OpinionLabel.Negative :
            OpinionLabel.Neutral; // Default to Neutral if no indicator

        unifiedFeedback.OpinionPoints.Add((cleanPoint, label));
    }
}

// Output the extracted feedback for verification
Console.WriteLine("Opinion Points:");
foreach (var (opinion, _) in unifiedFeedback.OpinionPoints)
{
    Console.WriteLine($"{opinion}");
}

public enum OpinionLabel
{
    Positive,
    Negative,
    Neutral
}

public class OpinionMining
{
    public List<(string, OpinionLabel)> OpinionPoints { get; set; }
}
