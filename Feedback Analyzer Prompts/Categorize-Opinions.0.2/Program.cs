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
            Categorize Opinions of User Feedback:
                
                Objective: Precisely categorize the opinions expressed in user feedback to identify prevalent sentiments and areas for potential improvement or reinforcement.
                
                Product Description:
                {productDescription}: Brief description of the product or service being reviewed, providing context for understanding the user feedback.
                
                User Feedback:
                {userFeedback}: Direct input provided by the user about the product, focusing solely on their opinions and experiences.
                
                Instructions for Categorization:
                1. Read and Segment Feedback: Carefully read the user feedback and break it down into individual opinions or statements.
                2. Categorize Opinions: Classify each segment of feedback into predefined categories such as Usability, Performance, Aesthetics, Value, Customer Service, and others relevant to the product or service.
                3. Identify Sentiment: For each categorized opinion, determine the sentiment as positive, negative, or neutral.
                4. Note Specifics and Suggestions: Highlight any specific aspects mentioned within each opinion and note down any direct suggestions for improvement.
                
                Expected Output:
                - Categorized Opinions: A structured list of opinions extracted from the user feedback, categorized by relevant aspects such as Usability, Performance, etc., and tagged with their respective sentiments.
                - Summary of Suggestions: A concise summary of suggestions and improvements mentioned by users, linked to the respective categories."
            ;

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