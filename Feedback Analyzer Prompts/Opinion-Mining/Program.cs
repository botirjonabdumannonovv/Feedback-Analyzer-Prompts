using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// Main class
public class Program
{
    public static async Task Main(string[] args)
    {

        //create configurations
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets<Program>();
        var configuration = configurationBuilder.Build();

        //create kernel builder 
        var kernelBuilder = Kernel.CreateBuilder();

        //Add OpenAI connector
        kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo",
            apiKey: configuration["OpenAiApiSettings:ApiKey"]!);

        //Build kernel
        var kernel = kernelBuilder.Build();

        // Sample product description
        const string productDescription =
            "The Tesla Model Y: A symbol of innovation and sustainability in the automotive industry.";

        // User feedback input
        Console.Write("Enter your feedback about the product: ");
        var userFeedback = Console.ReadLine();

        // Define the extraction prompt
        var extractionPrompt = $@"
            ## Opinion Mining
        
            Product Description:
            {productDescription}
            
            User Feedback: {userFeedback}
            
            Analyze the sentiment of the user feedback.
            
            You must return only negative,positive or neutral. 
            ";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new OpinionMining { SentimentAnalysis = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"Sentiment Analysis: {unifiedFeedback.SentimentAnalysis}");
    }

    public class  OpinionMining
    {
        public string? SentimentAnalysis { get; set; }
    }
}