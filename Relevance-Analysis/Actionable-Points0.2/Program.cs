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
            ##Detailed Feedback Analysis for Actionable Insights:
               ##Objective: Analyze user comments to extract and summarize their overall meaning, and categorize the feedback as either generic, specific, and/or actionable.
                    
               ##Instructions:
                  ##Summarize and Interpret Feedback:
                    
                    Review User Comment: Carefully read the feedback provided.
                    Draft Overall Summary: Write a concise summary that captures the essential message or sentiment from the feedback.
                  ##Evaluate and Classify Feedback:
                    
                    Assess Nature of Feedback: Determine if the feedback is generic (broad concerns or opinions) or specific (concrete, detailed observations).
                    Determine Actionability: Identify if the feedback contains clear, implementable suggestions or improvements.
                  ##Document Analysis and Recommendations:
                    
                    Based on the nature and content of the feedback, outline any direct actions that can be taken or consider broader implications for systemic improvements.
              
             ##Product Description:
                {productDescription}

            ##User Feedback:
                {userFeedback}

             ##Expected Output:
                  ##Feedback Summary: A concise interpretation capturing the core message of the user's feedback.
                  ##Classification:
                    Nature: [Generic/Specific] – Indicate whether the feedback addresses broad concepts or specific details.
                    Actionability: [Actionable/Non-Actionable] – State whether the feedback presents clear steps for improvement.
               ##Result:
                    In this section, you would present the analysis of the user's comment based on the instructions provided. For example:
                    
                  ##Feedback Summary: ""The user appreciates the mouse but suggests improvements to the wheel button for better functionality.""
                  ##Classification:
                    Nature: Specific – The feedback points to a particular aspect of the product.
                    Actionability: Actionable – The feedback includes a specific suggestion for product enhancement.
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