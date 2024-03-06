using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// Main class
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configuration setup for API key retrieval
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets<Program>();
        var configuration = configurationBuilder.Build();

        // Semantic Kernel setup
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);
        var kernel = kernelBuilder.Build();

        // Sample product description
        const string productDescription = """
                                           Razor Viper Ultimate - wireless gaming mouse
                                                
                                                Sensitivity: 20,000DPI
                                                Tracking Speed : 650IPS
                                                Resolution accuracy : 99.6%
                                                
                                                74G LIGHTWEIGHT DESIGN
                                                Enjoy faster and smoother control with a lightweight wireless mouse designed for esports. Weighing just 74g, it achieves its weight without compromising on the build strength of its ambidextrous form factor.
                                                
                                                70 HOURS OF BATTERY LIFE
                                                Improved wireless power efficiency keeps it running at peak performance for up to 70 continuous hours—charge it just once a week to power 10 hours of daily gameplay.
                                                
                                                5 ON-BOARD MEMORY PROFILES
                                                Bring your settings anywhere and be match-ready in no time. Activate up to 5 profile configurations from its onboard memory or custom settings via cloud storage.
                                                
                                                8 PROGRAMMABLE BUTTONS
                                                Fully configurable via Razer Synapse 3, the 8 programmable buttons let you access macros and secondary functions so you can execute extended moves with ease. 
                                          """;
        // User feedback input
        Console.Write("Enter your feedback about the product: ");
        var userFeedback = Console.ReadLine();

        // Define the extraction prompt
        var extractionPrompt = $@"
           ##Entity Identification from User Feedback:
             ##Objective: Extract and identify key entities from user feedback, organizing them into key phrases with associated key-value pairs for clearer understanding and action.
               
           ##Instructions:
             ##Extract Key Phrases:
                
                Identify Entities: Pinpoint significant nouns or phrases from the feedback that represent distinct entities, such as product features, issues, or user emotions.
                Key Phrases Extraction: List these as key phrases indicative of the feedback's main topics.
             ##Assign Key-Value Pairs:
                
                For each key phrase, determine and assign a value that best represents the sentiment, frequency, or importance mentioned in the feedback.
             ##Organize Information:
                
                Structure the extracted information into an easy-to-read format, prioritizing by relevance or urgency based on the feedback context.
           
        ##Product Description:
               {productDescription}

        ##User Feedback:
               {userFeedback}

        ##Expected Output:
                A list of key phrases extracted from the user feedback.
                Associated key-value pairs for each phrase, providing additional detail or context.
           ##Result:
             Key Phrases with Key-Value Pairs:
              Example:
                Phrase: ""Battery Life""; Value: ""Short - Negative""
                Phrase: ""Customer Service""; Value: ""Helpful - Positive""
        ";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new FeedbackExtraction { RelevantExtraction = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"{unifiedFeedback.RelevantExtraction}");
    }
    
    public class  FeedbackExtraction
    {
        public string? RelevantExtraction { get; set; }
    }
}