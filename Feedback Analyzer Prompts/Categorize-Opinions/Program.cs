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
    ## Organize Opinions of Feedback Analysis

    Given the product description and user feedback, analyze the feedback to determine user key points.

    Product Description:
    {productDescription}

    User Feedback: 
    {userFeedback}

    ## Instructions
    - Identify key points made by the user about the product.

    ## Expected Output
    - Key Points: ";

// Invoke the Semantic Kernel for feedback extraction
var response = await kernel.InvokePromptAsync<string>(extractionPrompt);

// Create and fill the FeedbackAnalysis object
var feedbackAnalysis = new FeedbackAnalysis { KeyPoints = new List<string>() };

// Parse extractionResponse to extract key points and add them to the collection
if (!string.IsNullOrEmpty(response))
{
    // Remove unnecessary whitespaces and line breaks
    response = response.Trim();
    // Add key points to the collection
    feedbackAnalysis.KeyPoints.Add(response);
}

// Output the extracted key points for verification
Console.WriteLine("Key Points:");
foreach (var keyPoint in feedbackAnalysis.KeyPoints)
{
    Console.WriteLine($"{keyPoint}");
}

public class FeedbackAnalysis
{
    public List<string> KeyPoints { get; set; }
}
